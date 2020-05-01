using System.IO.Abstractions;
using Episememe.Application.Interfaces;
using Episememe.Infrastructure.Database;
using Episememe.Infrastructure.FileSystem;
using Episememe.Infrastructure.Time;
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

            services.AddTransient<ITimeProvider, TimeProvider>();

            services.Configure<FileStorageSettings>(options
                => configuration.GetSection("FileStorage").Bind(options));
            services.AddScoped<IFileSystem, System.IO.Abstractions.FileSystem>();
            services.AddScoped<IFileStorage, FileStorage>();

            return services;
        }
    }
}
