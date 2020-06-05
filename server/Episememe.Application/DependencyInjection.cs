using System.Reflection;
using Episememe.Application.Graphs.Interfaces;
using Episememe.Application.Graphs.Tags;
using Episememe.Domain.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Episememe.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddTransient<IGraph<Tag>, TagGraph>();

            return services;
        }
    }
}
