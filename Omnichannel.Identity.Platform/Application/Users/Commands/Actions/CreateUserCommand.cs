using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Framework.Cqrs.Commands;
using Omnichannel.Identity.Platform.Domain;

namespace Omnichannel.Identity.Platform.Application.Users.Commands.Actions
{
    public class CreateUserCommand : ICommand
    {
        [Required]
        public string Company { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string CPF { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public CreateUserCommand(string company, string name, string cpf, string email, string password)
        {
            Company = company;
            Name = name;
            CPF = cpf;
            Email = email;
            Password = password;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Company))
                yield return new ValidationResult("invalid company.", new[] { nameof(Company) });

            if (string.IsNullOrEmpty(Name))
                yield return new ValidationResult("invalid name.", new[] { nameof(Name) });

            if (string.IsNullOrEmpty(CPF))
                yield return new ValidationResult("invalid CPF.", new[] { nameof(CPF) });

            if (string.IsNullOrEmpty(Email))
                yield return new ValidationResult("invalid e-mail.", new[] { nameof(Email) });

            if (string.IsNullOrEmpty(Password))
                yield return new ValidationResult("invalid password.", new[] { nameof(Password) });
        }
    }

    public static class CreateUserCommandExtensions
    {
        public static User ToDomain(this CreateUserCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            return User.Create(
               command.Company, command.Name, command.CPF, command.Email, command.Password);
        }
    }
}
