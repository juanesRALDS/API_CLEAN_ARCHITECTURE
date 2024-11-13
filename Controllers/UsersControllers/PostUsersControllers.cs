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
    public class PostUsersControllers : ControllerBase
    {
        private readonly UserServices _userService;

        public PostUsersControllers(UserServices userService)
        {
            _userService = userService;

        }
        public async Task<ActionResult<List<User>>> Get()
        {
            List<User>? users = await _userService.GetAsync();
            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult> Create(User newUser)
        {
            await _userService.CreateAsync(newUser);
            return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
        }
    }
}