using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Services;
using Microsoft.AspNetCore.Mvc;
namespace api_completa_mongodb_net_6_0.Controllers.UsersControllers


{
    [ApiController]
    [Route("api/users")]
    public class UpdateUserController : ControllerBase
    {
        private readonly UserServices _userService;

        public UpdateUserController(UserServices userService)
        {
            _userService = userService;

        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, User updatedUser)
        {
            User? existingUser = await _userService.GetAsync(id);
            if (existingUser is null) return NotFound();

            updatedUser.Id = existingUser.Id; // Asegurarse de mantener el mismo ID
            await _userService.UpdateAsync(id, updatedUser);

            return NoContent();
        }
    }
}