

namespace api_completa_mongodb_net_6_0.Application.DTO;

public class UserDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
}

public class UserDtoResponse : UserDto
{
    
}