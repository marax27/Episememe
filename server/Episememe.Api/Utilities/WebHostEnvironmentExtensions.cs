using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Episememe.Api.Utilities
{
    public static class WebHostEnvironmentExtensions
    {
        public static bool IsDesktop(this IWebHostEnvironment environment)
            => environment.IsEnvironment("Desktop");
    }
}
