using System;
using Core.Framework.Cache;
using Core.Framework.Cache.Redis;
using Core.Framework.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Omnichannel.Identity.Platform.Application.Users;
using Omnichannel.Identity.Platform.Application.Users.Commands;
using Omnichannel.Identity.Platform.Application.Users.Queries;
using Omnichannel.Identity.Platform.Domain;
using Omnichannel.Identity.Platform.Infrastructure.Database;
using Omnichannel.Identity.Platform.Infrastructure.Database.Repositories;
using Omnichannel.Identity.Platform.Infrastructure.Providers.Cache.Models;
using Omnichannel.Identity.Platform.Infrastructure.Providers.Security;
using Omnichannel.Identity.Platform.Infrastructure.Providers.Security.Models;

namespace Omnichannel.Identity.Platform.Infrastructure.IOC
{
    public class IdentityModule
    {
        public IdentityModule(IConfiguration configuration)
            : this(new ServiceCollection(), configuration)
        { }

        public IdentityModule(IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // jwt configuration
            var siginConfiguration = new SigninConfiguration();
            var tokenConfiguration = new TokenConfiguration();
            new ConfigureFromConfigurationOptions<TokenConfiguration>(
                configuration.GetSection("Token")).Configure(tokenConfiguration);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var paramsValidation = options.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = siginConfiguration.Key;
                paramsValidation.ValidAudience = tokenConfiguration.Audience;
                paramsValidation.ValidIssuer = tokenConfiguration.Issuer;

                // validate token signature
                paramsValidation.ValidateIssuerSigningKey = true;

                // validate token lifetime
                paramsValidation.ValidateLifetime = true;

                // set tolerance time for token expiration
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });

            // configure access authorization
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

            // HTTP Context
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // data source
            services.AddSingleton<IDataSource>(provider =>
                new DefaultDataSource(configuration, "IdentityDb"));

            // unit of work
            services.AddScoped<IUnitOfWorkAsync, IdentityContext>();

            // providers
            // cache
            services.AddSingleton<ICacheService>(provider =>
                new RedisCacheService(configuration.GetValue<string>("Redis:Endpoint"),
                                      configuration.GetValue<int>("Redis:Database"))
                { Expires = TimeSpan.FromSeconds(configuration.GetValue<int>("Redis:Expires")) });

            services.AddSingleton(provider =>
                new CacheConfiguration(configuration.GetValue<string>("Redis:CacheKey")));

            // security token
            services.AddScoped<ISecurityTokenService>(provider =>
                new SecurityTokenService(siginConfiguration, tokenConfiguration));

            // repositories
            services.AddScoped<IUserRepository, UserRepository>();

            // command handlers
            services.AddScoped<IUserCommandHandler, UserCommandHandler>();

            // query handlers
            services.AddScoped<IUserQueryHandler, UserQueryHandler>();

            // app services
            services.AddScoped<IUserAppService, UserAppService>();
        }
    }
}
