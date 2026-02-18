using Azure;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Portfolio.DataContext;
using Portfolio.Entities;
using Portfolio.IServices;
using Portfolio.Modals;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;

namespace Portfolio.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ICacheService _cacheService;
        private readonly ICurrentUserService _currentUserService;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContext;

        public AuthService(UserDbContext context, IConfiguration configuration, ICacheService cacheService, ICurrentUserService currentUserService, HttpClient httpClient, IHttpContextAccessor httpContext)
        {
            _context = context;
            _configuration = configuration;
            _cacheService = cacheService;
            _currentUserService = currentUserService;
            _httpClient = httpClient;
            _httpContext = httpContext;
        }
        public async Task<User> RegisterAsync(UserDTO request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                throw new Exception("User already exists");
            }
            User user = new User();
            var passwrodHash = new PasswordHasher<User>().HashPassword(user, request.Password);

            user.Email = request.Email;
            user.PasswordHash = passwrodHash;
            user.UserRole = request.UserRole;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<string> LoginAsync(UserDTO request)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return null;
            }
            if (string.IsNullOrEmpty(user.UserRole))
                user.UserRole = "Admin";

            var result = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }
            string token = CreateToken(user);
            _httpContext.HttpContext.Response.Cookies.Append("accessToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(15)
            });
            return token;
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.UserRole)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("Issuer"),
                audience: _configuration.GetValue<string>("Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public async Task Logout()
        {
            string userId = _currentUserService.GetUserId().ToString();
            string googleAccessToken = await _cacheService.GetTokenAsync(userId);
            await _httpClient.PostAsync($"https://oauth2.googleapis.com/revoke?token={googleAccessToken}", null);
            await _cacheService.RemoveTokenAsync(userId);
        }

        public async Task<string> LoginWithGmail(AuthenticateResult result)
        {
            if (result == null || !result.Succeeded)
                return "";

            string accessToken = result.Properties.GetTokenValue("access_token");

            string email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;

            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                // Register new user
                user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = email,
                    UserRole = "Admin",
                    PasswordHash = ""
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            await _cacheService.SaveTokenAsync(user.Id.ToString(), accessToken, (int)TimeSpan.FromHours(1).TotalSeconds);

            string jwtToken = CreateToken(user);
            return jwtToken;
        }

        public DriveService CreateDriveService(string accessToken)
        {
            var credential = GoogleCredential
                .FromAccessToken(accessToken);

            return new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "Portfolio App"
            });
        }

        public async Task<byte[]> ReadFileFromDrive(string userId, string fileId)
        {
            string accessToken = await _cacheService.GetTokenAsync(userId);

            DriveService service = CreateDriveService(accessToken);

            var request = service.Files.Get(fileId);
            var stream = new MemoryStream();

            await request.DownloadAsync(stream);

            return stream.ToArray();
        }


    }
}
