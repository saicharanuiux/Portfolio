using Microsoft.AspNetCore.Authentication;
using Portfolio.Entities;
using Portfolio.Modals;

namespace Portfolio.IServices
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(UserDTO request);
        Task<string> LoginAsync(UserDTO request);
        Task Logout();
        Task<string> LoginWithGmail (AuthenticateResult result);
        Task<byte[]> ReadFileFromDrive(string userId, string fileId);
    }
}
