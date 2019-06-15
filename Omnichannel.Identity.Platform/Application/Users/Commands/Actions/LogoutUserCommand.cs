using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Framework.Cqrs.Commands;

namespace Omnichannel.Identity.Platform.Application.Users.Commands.Actions
{
    public class LogoutUserCommand : ICommand
    {
        [Required]
        public string Company { get; set; }

        [Required]
        public string CPF { get; set; }

        public LogoutUserCommand(string company, string email)
        {
            Company = company;
            CPF = email;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Company))
                yield return new ValidationResult("invalid company.", new[] { nameof(Company) });

            if (string.IsNullOrEmpty(CPF))
                yield return new ValidationResult("invalid CPF.", new[] { nameof(CPF) });
        }
    }
}