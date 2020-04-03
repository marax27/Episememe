using Microsoft.AspNetCore.Builder;

namespace Episememe.Api.Configuration
{
    public static class Cors
    {
        public static IApplicationBuilder EnableCors(this IApplicationBuilder app)
        {
            app.UseCors(o => {
                o.AllowAnyOrigin();
                o.AllowAnyHeader();
                o.AllowAnyMethod();
            });
            return app;
        }
    }
}
