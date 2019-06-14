using System;
using Omnichannel.Identity.Platform.Domain;

namespace Omnichannel.Identity.Platform.Application.Users.Queries.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public string Name { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public bool Active { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
    }

    public static class UserDTOExtensions
    {
        public static UserDTO ToDTO(this User user)
        {
            if (user == null) return null;

            return new UserDTO
            {
                Id = user.ID,
                Company = user.Company,
                Name = user.Name,
                CPF = user.CPF,
                Email = user.Email,
                Active = user.Active,
                Token = user.LogInToken,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
