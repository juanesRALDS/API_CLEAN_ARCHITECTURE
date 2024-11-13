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
    public class DeleteControllerUsers : ControllerBase
    {
        private readonly UserServices _userService;

        public DeleteControllerUsers(UserServices userService)
        {
            _userService = userService;
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            User? user = await _userService.GetAsync(id);
            if (user is null) return NotFound();

            await _userService.RemoveAsync(id);

            return NoContent();
        }
    }
}