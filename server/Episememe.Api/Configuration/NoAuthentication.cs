using Episememe.Api.Desktop;
using Microsoft.Extensions.DependencyInjection;

namespace Episememe.Api.Configuration
{
    public static class NoAuthentication
    {
        public static IServiceCollection DisableAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication("Desktop")
                .AddScheme<DesktopAuthenticationOptions, DesktopAuthenticationHandler>("Desktop", null);

            return services;
        }
    }
}
