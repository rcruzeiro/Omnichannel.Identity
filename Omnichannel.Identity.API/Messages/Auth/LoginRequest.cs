using Core.Framework.API.Messages;

namespace Omnichannel.Identity.API.Messages.Auth
{
    public class LoginRequest : BaseRequest
    {
        public string Company { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}