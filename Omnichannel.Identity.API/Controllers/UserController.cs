using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Omnichannel.Identity.Platform.Application.Users;
using Omnichannel.Identity.Platform.Application.Users.Commands.Actions;
using Omnichannel.Identity.Platform.Application.Users.Queries.DTOs;

namespace Omnichannel.Identity.API.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class UserController : Controller
    {
        readonly IUserAppService _userAppService;

        public UserController([FromServices]IUserAppService userAppService)
        {
            _userAppService = userAppService ?? throw new ArgumentNullException(nameof(userAppService));
        }
        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="command">Create user command.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <response code="201">User created with success.</response>
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateUserCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                await _userAppService.Create(command, cancellationToken);

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
        /// <summary>
        /// Get the specified token user.
        /// </summary>
        /// <param name="authorization">Logged user token.</param>
        //[Authorize("Bearer")]
        [ProducesResponseType(typeof(UserDTO), 200)]
        [ProducesResponseType(500)]
        [HttpGet]
        public IActionResult Get([FromHeader]string authorization)
        {
            try
            {
                var token = authorization.Replace("Bearer ", "");
                var user = _userAppService.GetUser(token);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
