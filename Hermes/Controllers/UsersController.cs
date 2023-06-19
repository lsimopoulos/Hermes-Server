using Hermes.Classes;
using Hermes.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

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
        public async Task<ActionResult> Register([FromBody] HermesUser testUser)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            if (!await _usersManagers.AddUserAsync(testUser, cancellationTokenSource.Token))
                return BadRequest("The user aldready exists");
            return Ok();
        }
    }
}
