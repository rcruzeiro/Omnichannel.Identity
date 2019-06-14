using System;
using System.Security.Cryptography;
using System.Text;
using Core.Framework.Entities;

namespace Omnichannel.Identity.Platform.Domain
{
    public class User : IAggregationRoot, IEntity
    {
        public int ID { get; set; }

        public string Company { get; private set; }

        public string Name { get; private set; }

        public string CPF { get; private set; }

        public string Email { get; private set; }

        public string Password { get; private set; }

        public bool Active { get; private set; }

        public string LogInToken { get; private set; }

        public DateTimeOffset? LastLogin { get; private set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }

        protected User()
        { }

        public void Login(string token)
        {
            LogInToken = token;
            LastLogin = DateTimeOffset.Now;
            UpdatedAt = DateTimeOffset.Now;
        }

        public void Logout()
        {
            LogInToken = string.Empty;
            UpdatedAt = DateTimeOffset.Now;
        }

        public static User Create(string company, string name, string cpf, string email, string password)
        {
            var user = new User
            {
                Company = company,
                Name = name,
                CPF = cpf,
                Email = email,
                Active = true,
                CreatedAt = DateTimeOffset.Now
            };

            user.Password = CreatePasswordHash(password);

            return user;
        }

        internal static string CreatePasswordHash(string password)
        {
            SHA256 sHA256 = SHA256.Create();
            byte[] input = Encoding.ASCII.GetBytes(password);
            byte[] output = sHA256.ComputeHash(input);

            return Convert.ToBase64String(output);
        }
    }
}
