using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Framework.Cqrs.Queries;
using Core.Framework.Repository;
using Omnichannel.Identity.Platform.Domain;

namespace Omnichannel.Identity.Platform.Application.Users.Queries.Filters
{
    public class GetUserFilter : BaseSpecification<User>, IFilter
    {
        [Required]
        public string Company { get; set; }

        [Required]
        public string Email { get; set; }

        public GetUserFilter(string company, string email)
            : base(u => u.Company == company &&
                        u.Email == email)
        {
            Company = company;
            Email = email;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Company))
                yield return new ValidationResult("invalid company.", new[] { nameof(Company) });

            if (string.IsNullOrEmpty(Email))
                yield return new ValidationResult("invalid email.", new[] { nameof(Email) });
        }
    }
}