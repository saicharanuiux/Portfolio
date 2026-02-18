using Portfolio.Models;

namespace Portfolio.IServices
{
    public interface ICurrentUserService
    {
        CurrentUser GetCurrentUser();
        string GetUserId();
        string GetEmail();
        bool IsInRole(string role);
        bool IsAuthenticated();
    }
}
