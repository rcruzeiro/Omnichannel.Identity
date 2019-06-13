using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Omnichannel.Identity.Platform.Application.Users;
using Omnichannel.Identity.Platform.Application.Users.Commands.Actions;
using Omnichannel.Identity.Platform.Application.Users.Queries.DTOs;

namespace Omnichannel.Identity.API.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("[controller]")]
    public class UserController : Controller
    {
        readonly IHttpContextAccessor _httpContext;
        readonly IUserAppService _userAppService;

        public UserController([FromServices] IHttpContextAccessor httpContext,
            [FromServices]IUserAppService userAppService)
        {
            _httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _userAppService = userAppService ?? throw new ArgumentNullException(nameof(userAppService));
        }
        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="command">Create user command.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <response code="201">User created with success.</response>
        [AllowAnonymous]
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
        //[Authorize("Bearer")]
        //[ProducesResponseType(typeof(UserDTO), 200)]
        //[ProducesResponseType(500)]
        //[HttpGet]
        //public IActionResult Get()
        //{
        //    try
        //    {
        //        // get company and email data from jwt
        //        var company = _httpContext.HttpContext.User.Claims.First(c => c.Type == "company")?.Value;
        //        var email = _httpContext.HttpContext.User.Claims.First(c => c.Type == "username")?.Value;

        //        var user = _userAppService.GetUser(company, email);

        //        return Ok(user);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex);
        //    }
        //}
    }
}
