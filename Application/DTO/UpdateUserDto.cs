using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_completa_mongodb_net_6_0.Application.DTO
{
    public class UpdateUserDto
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

    }
}