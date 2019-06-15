using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Framework.API.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Omnichannel.Identity.API.Messages.Auth;
using Omnichannel.Identity.Platform.Application.Users;
using Omnichannel.Identity.Platform.Application.Users.Commands.Actions;
using Omnichannel.Identity.Platform.Application.Users.Queries.Filters;

namespace Omnichannel.Identity.API.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        readonly IHttpContextAccessor _httpContext;
        readonly IUserAppService _userAppService;

        public AuthController([FromServices] IHttpContextAccessor httpContext,
            [FromServices]IUserAppService userAppService)
        {
            _httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _userAppService = userAppService ?? throw new ArgumentNullException(nameof(userAppService));
        }
        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="request">Request data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponse), 200)]
        [ProducesResponseType(typeof(LoginResponse), 404)]
        [ProducesResponseType(typeof(LoginResponse), 500)]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginRequest request, CancellationToken cancellationToken = default)
        {
            var response = new LoginResponse();

            try
            {
                var filter = new LoginFilter(request.Company, request.CPF, request.Password);
                var user = await _userAppService.Login(filter, cancellationToken);

                if (user == null)
                {
                    response.StatusCode = 404;
                    response.Messages.Add(ResponseMessage.Create("", "Cannot find user with this parameters."));
                    return NotFound(response);
                }

                response.StatusCode = 200;
                response.Data = user.Token;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Messages.Add(ResponseMessage.Create(ex, ""));
                return StatusCode(500, response);
            }
        }
        /// <summary>
        /// Logout user.
        /// </summary>
        [Authorize("Bearer")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(LogoutResponse), 500)]
        [HttpDelete]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken = default)
        {
            var response = new LogoutResponse();

            try
            {
                // get company and email data from jwt
                var company = _httpContext.HttpContext.User.Claims.First(c => c.Type == "company")?.Value;
                var email = _httpContext.HttpContext.User.Claims.First(c => c.Type == "username")?.Value;
                var command = new LogoutUserCommand(company, email);

                await _userAppService.Logout(command, cancellationToken);

                return NoContent();
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Messages.Add(ResponseMessage.Create(ex, ""));
                return StatusCode(500, response);
            }
        }
    }
}
