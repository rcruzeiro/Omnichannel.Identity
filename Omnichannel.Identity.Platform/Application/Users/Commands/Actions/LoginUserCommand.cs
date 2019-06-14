using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Framework.Cqrs.Commands;

namespace Omnichannel.Identity.Platform.Application.Users.Commands.Actions
{
    public class LoginUserCommand : ICommand
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string Token { get; set; }

        public LoginUserCommand(int userId, string token)
        {
            UserId = userId;
            Token = token;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (UserId == default)
                yield return new ValidationResult("invalid user.", new[] { nameof(UserId) });

            if (string.IsNullOrEmpty(Token))
                yield return new ValidationResult("invalid token.", new[] { nameof(Token) });
        }
    }
}