using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Portfolio.DataContext;
using Portfolio.Entities;
using Portfolio.IServices;
using Portfolio.Modals;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Portfolio.Services
{
    public class AuthService(UserDbContext context, IConfiguration configuration) : IAuthService
    {
        public async Task<User> RegisterAsync(UserDTO request)
        {
            if (await context.Users.AnyAsync(u => u.Email == request.Email))
            {
                throw new Exception("User already exists");
            }
            User user = new User();
            var passwrodHash = new PasswordHasher<User>().HashPassword(user, request.Password);

            user.Email = request.Email;
            user.PasswordHash = passwrodHash;
            user.UserRole = request.UserRole;
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user;
        }
        public async Task<string> LoginAsync(UserDTO request)
        {
            User user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
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
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("Issuer"),
                audience: configuration.GetValue<string>("Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public async Task<string> LoginWithGmail(string email)
        {
            User user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                // Register new user
                user = new User
                {
                    Email = email,
                    UserRole = "Admin",
                    PasswordHash = ""
                };
                context.Users.Add(user);
                await context.SaveChangesAsync();
            }
            string token = CreateToken(user);
            return token;
        }

    }
}
