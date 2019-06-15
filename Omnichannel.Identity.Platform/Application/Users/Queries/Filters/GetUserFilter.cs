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
        public string CPF { get; set; }

        public GetUserFilter(string company, string cpf)
            : base(u => u.Company == company &&
                        u.CPF == cpf)
        {
            Company = company;
            CPF = cpf;
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