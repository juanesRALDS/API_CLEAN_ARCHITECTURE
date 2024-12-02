namespace api_completa_mongodb_net_6_0.Application.DTOs
{
    public class UpdatePasswordRequest
    {
        public string NewPassword { get; set; } = string.Empty;
        public string Tokens { get; set; } = string.Empty;
    }
}
