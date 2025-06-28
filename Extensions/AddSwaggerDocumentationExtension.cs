using Microsoft.OpenApi.Models;

namespace Ctor.Extensions
{
    public static class AddSwaggerDocumentationExtension
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Babago API",
                    Description = "Dispatch & Logistics Services",
                    Version = "v1"
                });
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger(); // Serves swagger.json

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Babago API V1"); // UI page
            });

            return app;
        }
    }
}
