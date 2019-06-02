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
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public LoginFilter(string company, string email, string password)
            : base(u => u.Company == company &&
                        u.Email == email &&
                        u.Password == User.CreatePasswordHash(password))
        {
            Company = company;
            Email = email;
            Password = password;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Company))
                yield return new ValidationResult("invalid company.", new[] { nameof(Company) });

            if (string.IsNullOrEmpty(Email))
                yield return new ValidationResult("invalid email.", new[] { nameof(Email) });

            if (string.IsNullOrEmpty(Password))
                yield return new ValidationResult("invalid password.", new[] { nameof(Password) });
        }
    }
}
