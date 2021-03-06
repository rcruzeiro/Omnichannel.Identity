﻿namespace Omnichannel.Identity.Platform.Infrastructure.Providers.Security.Models
{
    public sealed class TokenData
    {
        public string Company { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string CPF { get; private set; }

        public TokenData(string company, string name, string email, string cpf)
        {
            Company = company;
            Name = name;
            Email = email;
            CPF = cpf;
        }
    }
}
