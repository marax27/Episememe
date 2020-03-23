using Episememe.Api.Configuration;
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
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructure(Configuration);

            services.AddJwtAuthentication(Configuration);
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDatabaseMigrationService databaseMigration)
        {
            databaseMigration.Migrate();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(o => {
                    o.AllowAnyOrigin();
                    o.AllowAnyHeader();
                    o.AllowAnyMethod();
                });
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
