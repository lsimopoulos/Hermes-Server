using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Hermes.Classes;
using IdentityServer4.Test;
using Hermes.Models;
using System;

namespace Hermes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersManagers _usersManagers;

        public UsersController(UsersManagers usersManagers)
        {
            _usersManagers = usersManagers;
        }

        [HttpPost]
        [Route("Register")]
        public ActionResult Register([FromBody] HermesUser testUser)
        {
            if (! _usersManagers.AddUser(testUser))
                return BadRequest("The user aldready exists");
            return Ok();
        }
    }
}
