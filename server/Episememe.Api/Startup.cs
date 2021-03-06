using Episememe.Api.Configuration;
using Episememe.Api.Json;
using Episememe.Api.Utilities;
using Episememe.Application;
using Episememe.Infrastructure;
using Episememe.Infrastructure.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Episememe.Api
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication();
            services.AddInfrastructure(Configuration);

            if (_env.IsDevelopment())
            {
                services.ConfigureSwagger();
            }

            if (_env.IsDesktop())
            {
                services.DisableAuthentication();
            }
            else
            {
                services.AddJwtAuthentication(Configuration);
            }

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new UtcTimeConverter());
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IDatabaseMigrationService databaseMigration)
        {
            databaseMigration.Migrate();

            if (_env.IsDevelopment() || _env.IsDesktop())
            {
                app.EnableCors();
            }

            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.ExposeSwagger();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
