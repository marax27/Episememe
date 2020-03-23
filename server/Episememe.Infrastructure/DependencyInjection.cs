using Episememe.Application.Interfaces;
using Episememe.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Episememe.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration configuration)
        {
            var cs = configuration.GetSection("Database")["ConnectionString"];

            services.AddDbContext<ApplicationDbContext>(options
                => options.UseSqlite(cs));

            services.AddScoped<IApplicationContext>(provider
                => provider.GetService<ApplicationDbContext>());

            services.AddTransient<IDatabaseMigrationService, ApplicationDbContext>();

            return services;
        }
    }
}
