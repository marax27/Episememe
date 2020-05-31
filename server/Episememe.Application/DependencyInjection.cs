using System.Reflection;
using Episememe.Application.TagGraph;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Episememe.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddTransient<ITagGraphService, TagGraphService>();

            return services;
        }
    }
}
