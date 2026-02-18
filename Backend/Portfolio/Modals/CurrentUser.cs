namespace Portfolio.Models
{
    public class CurrentUser
    {
        public string UserId { get; init; }
        public string Email { get; init; } = string.Empty;
        public string Role { get; init; } = string.Empty;
        public bool IsAuthenticated { get; init; }
    }
}
