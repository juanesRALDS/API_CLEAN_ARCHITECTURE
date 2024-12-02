using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_completa_mongodb_net_6_0.Application.DTO;
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
