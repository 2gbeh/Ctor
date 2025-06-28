using Microsoft.EntityFrameworkCore;
using Ctor.Data;

namespace Ctor.Extensions
{
    public static class AddAppDbContextExtension
    {
        public static IServiceCollection AddAppDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            return services;
        }
    }
}
