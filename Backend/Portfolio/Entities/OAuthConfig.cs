namespace Portfolio.Entities
{
    public class OAuthConfig
    {
        public Guid Id { get; set; }
        public string Provider { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string CallbackPath { get; set; }
    }
}
