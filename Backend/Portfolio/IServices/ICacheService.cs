namespace Portfolio.IServices
{
    public interface ICacheService
    {
        Task SaveTokenAsync(string key, string value, int expiresInSeconds);
        Task<string?> GetTokenAsync(string key);
        Task RemoveTokenAsync(string key);
    }
}
