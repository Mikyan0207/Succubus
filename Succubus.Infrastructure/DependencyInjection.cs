using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Succubus.Infrastructure.Database;

namespace Succubus.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<Context>(opt =>
                opt.UseSqlite(configuration.GetConnectionString("Default"),
                    b => b.MigrationsAssembly(typeof(Context).Assembly.FullName)));

            return services;
        }
    }
}