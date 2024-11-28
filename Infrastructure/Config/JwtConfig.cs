
namespace api_completa_mongodb_net_6_0.Infrastructure.Config;
public static class JwtConfig
{
    public static string SecretKey { get; set; } = "TuClaveSecretaMuyLargaDe32Caracteres";
    public static string Issuer { get; set; } = "yourapp";
    public static string Audience { get; set; } = "yourapp";
}
