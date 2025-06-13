namespace BusinessService.Configurations
{
    public class TokenOptions
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public double AccessExpiresHours { get; set; }
        public double RefreshExpiresDays { get; set; }
    }
}
