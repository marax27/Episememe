using System;
using Episememe.Infrastructure.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Episememe.Api.Configuration
{
    public class AuthenticationConfigurationException : Exception
    {
        public AuthenticationConfigurationException(string message)
            : base(message) { }
    }

    public static class JwtAuthentication
    {
        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var auth0Configuration = GetConfiguration(configuration);
                options.Authority = auth0Configuration.Authority;
                options.Audience = auth0Configuration.Audience;
            });

            return services;
        }

        private static Auth0Configuration GetConfiguration(IConfiguration configuration)
        {
            var result = new Auth0Configuration();
            configuration.Bind("Auth0", result);

            if (result.Audience == null || result.Authority == null)
            {
                throw new AuthenticationConfigurationException("Auth0 configuration cannot be found.");
            }

            return result;
        }
    }
}
