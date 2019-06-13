using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Omnichannel.Identity.Platform.Application.Users;
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
        /// <param name="data">
        /// User login payload; must be:
        /// {"company": "string", "email": "string", "password": "string"}
        /// </param>
        /// <param name="cancellationToken">Cancellation token.</param>
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(500)]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]JObject data, CancellationToken cancellationToken = default)
        {
            try
            {
                var filter = data.ToObject<LoginFilter>();
                var user = await _userAppService.Login(filter, cancellationToken);

                return Ok(new { user.Token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
        /// <summary>
        /// Logout user.
        /// </summary>
        //[Authorize("Bearer")]
        //[ProducesResponseType(204)]
        //[ProducesResponseType(500)]
        //[HttpDelete]
        //public IActionResult Logout()
        //{
        //    try
        //    {
        //        // get company and email data from jwt
        //        var company = _httpContext.HttpContext.User.Claims.First(c => c.Type == "company")?.Value;
        //        var email = _httpContext.HttpContext.User.Claims.First(c => c.Type == "username")?.Value;

        //        _userAppService.Logout(company, email);

        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex);
        //    }
        //}
    }
}
