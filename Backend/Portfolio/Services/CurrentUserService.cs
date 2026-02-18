using System.Security.Claims;
using Portfolio.IServices;
using Portfolio.Models;

namespace Portfolio.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? User =>
            _httpContextAccessor.HttpContext?.User;

        public bool IsAuthenticated()
        {
            return User?.Identity?.IsAuthenticated ?? false;
        }

        public string GetUserId()
        {
            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return string.IsNullOrEmpty(userId) ? "" : userId;
        }

        public string GetEmail()
        {
            return User?.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
        }

        public bool IsInRole(string role)
        {
            return User?.IsInRole(role) ?? false;
        }

        public CurrentUser GetCurrentUser()
        {
            return new CurrentUser
            {
                UserId = GetUserId(),
                Email = GetEmail(),
                Role = User?.FindFirstValue(ClaimTypes.Role) ?? string.Empty,
                IsAuthenticated = IsAuthenticated()
            };
        }
    }
}
