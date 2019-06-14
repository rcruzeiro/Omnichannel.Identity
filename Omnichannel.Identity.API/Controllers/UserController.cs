using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Framework.API.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Omnichannel.Identity.API.Messages.User;
using Omnichannel.Identity.Platform.Application.Users;
using Omnichannel.Identity.Platform.Application.Users.Commands.Actions;
using Omnichannel.Identity.Platform.Application.Users.Queries.Filters;

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
        /// <param name="request">Create user command.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <response code="201">User created with success.</response>
        [AllowAnonymous]
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(CreateUserResponse), 500)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateUserCommand request, CancellationToken cancellationToken = default)
        {
            var response = new CreateUserResponse();

            try
            {
                await _userAppService.Create(request, cancellationToken);

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Messages.Add(ResponseMessage.Create(ex, ""));
                return StatusCode(500, response);
            }
        }
        /// <summary>
        /// Get the specified token user.
        /// </summary>
        [Authorize("Bearer")]
        [ProducesResponseType(typeof(GetUserResponse), 200)]
        [ProducesResponseType(typeof(GetUserResponse), 500)]
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
        {
            var response = new GetUserResponse();

            try
            {
                // get company and email data from jwt
                var company = _httpContext.HttpContext.User.Claims.First(c => c.Type == "company")?.Value;
                var email = _httpContext.HttpContext.User.Claims.First(c => c.Type == "username")?.Value;
                var filter = new GetUserFilter(company, email);

                var user = await _userAppService.GetUser(filter, cancellationToken);

                response.StatusCode = 200;
                response.Data = user;

                return Ok(response);
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
