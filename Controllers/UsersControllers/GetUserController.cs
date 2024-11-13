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
    public class GetUserController : ControllerBase
    {

        private readonly UserServices _userServices;
    
        public GetUserController(UserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            List<User>? users = await _userServices.GetAsync();
            return Ok(users);
        }

        // GET: api/users/{id}
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<User>> Get(string id)
        {
            User? user = await _userServices.GetAsync(id);

            if (user == null)
                return NotFound($"User with ID {id} not found");

            return Ok(user);
        }



    }
}