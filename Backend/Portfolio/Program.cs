using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Portfolio.DataContext;
using Portfolio.IServices;
using Portfolio.Services;
using StackExchange.Redis;
using System.Security.Claims;
using System.Text;


// JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
IdentityModelEventSource.ShowPII = true;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();


builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddDbContext<UserDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UserDbContext>();

    var googleConfig = db.OAuthConfigs
        .Where(x => x.Provider == "Google")
        .Select(x => new { x.ClientId, x.ClientSecret, x.CallbackPath })
        .FirstOrDefault();

    if (googleConfig == null)
        throw new Exception("Google OAuth config missing in DB");

    builder.Services.AddAuthentication()
        .AddGoogle("Google", options =>
        {
            options.ClientId = googleConfig.ClientId;
            options.ClientSecret = googleConfig.ClientSecret;
            options.CallbackPath = googleConfig.CallbackPath;
            options.SignInScheme = "External";

            options.Scope.Add("https://www.googleapis.com/auth/drive.readonly");
            options.SaveTokens = true;
        })
     .AddCookie("External");
}


builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = "External";
    })
    .AddCookie(options =>
    {
        options.Cookie.Name = "accessToken";
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.MapInboundClaims = false;

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("JWT FAILED:");
                Console.WriteLine(context.Exception.GetType().Name);
                Console.WriteLine(context.Exception.Message);
                return Task.CompletedTask;
            },
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Cookies["accessToken"];
                if (!string.IsNullOrEmpty(accessToken))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Token"]!)
            ),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            RoleClaimType = ClaimTypes.Role
        };
    });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
});

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = builder.Configuration.GetConnectionString("Redis");

    var options = ConfigurationOptions.Parse(configuration);
   // options.AbortOnConnectFail = false;
    options.ConnectRetry = 5;
    options.ConnectTimeout = 5000;

    return ConnectionMultiplexer.Connect(options);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}
app.UseCors(builder =>
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader());

app.Use(async (context, next) =>
{
    var authHeader = context.Request.Headers["Authorization"].ToString();
    Console.WriteLine($"[DEBUG] Path: {context.Request.Path}");
    Console.WriteLine($"[DEBUG] Auth Header: {authHeader}");
    await next();
});

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
