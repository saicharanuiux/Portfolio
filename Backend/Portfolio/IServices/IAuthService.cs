using Portfolio.Entities;
using Portfolio.Modals;

namespace Portfolio.IServices
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(UserDTO request);
        Task<string> LoginAsync(UserDTO request);

        Task<string> LoginWithGmail (string email);
    }
}
