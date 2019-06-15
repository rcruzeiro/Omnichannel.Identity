using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Framework.Cqrs.Queries;
using Core.Framework.Repository;
using Omnichannel.Identity.Platform.Domain;

namespace Omnichannel.Identity.Platform.Application.Users.Queries.Filters
{
    public class LoginFilter : BaseSpecification<User>, IFilter
    {
        [Required]
        public string Company { get; set; }

        [Required]
        public string CPF { get; set; }

        [Required]
        public string Password { get; set; }

        public LoginFilter(string company, string cpf, string password)
            : base(u => u.Company == company &&
                        u.CPF == cpf &&
                        u.Password == User.CreatePasswordHash(password))
        {
            Company = company;
            CPF = cpf;
            Password = password;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Company))
                yield return new ValidationResult("invalid company.", new[] { nameof(Company) });

            if (string.IsNullOrEmpty(CPF))
                yield return new ValidationResult("invalid CPF.", new[] { nameof(CPF) });

            if (string.IsNullOrEmpty(Password))
                yield return new ValidationResult("invalid password.", new[] { nameof(Password) });
        }
    }
}
