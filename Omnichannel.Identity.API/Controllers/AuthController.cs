using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Omnichannel.Identity.Platform.Application.Users;
using Omnichannel.Identity.Platform.Application.Users.Queries.DTOs;
using Omnichannel.Identity.Platform.Application.Users.Queries.Filters;

namespace Omnichannel.Identity.API.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        readonly IUserAppService _userAppService;

        public AuthController([FromServices]IUserAppService userAppService)
        {
            _userAppService = userAppService ?? throw new ArgumentNullException(nameof(userAppService));
        }
        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="filter">Login filter.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        [ProducesResponseType(typeof(UserDTO), 200)]
        [ProducesResponseType(500)]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginFilter filter, CancellationToken cancellationToken = default)
        {
            try
            {
                var user = await _userAppService.Login(filter, cancellationToken);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
        /// <summary>
        /// Logout user.
        /// </summary>
        /// <param name="token">Logged user token.</param>
        [Authorize]
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        [HttpDelete]
        public IActionResult Logout([FromHeader]string token)
        {
            try
            {
                _userAppService.Logout(token);

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
