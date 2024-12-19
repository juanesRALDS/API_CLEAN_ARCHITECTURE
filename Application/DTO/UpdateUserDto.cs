
namespace SagaAserhi.Application.DTO;
public class UpdateUserDto
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }

    

}

public class UpdateUserResponseDto : UserDto
{
    public string? Tokens { get; set; }
}
