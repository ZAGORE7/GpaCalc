using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ozbul.Application.Portal.Repository.Interfaces;

namespace Ozbul.Application.Portal.Repository.Extensions
{
	public static class ServiceCollectionExtensions
	{
        public static IServiceCollection RegisterDbContext<T>(this IServiceCollection services, string connectionString, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) where T : DbContext
        {
            services.AddScoped<IUnitOfWork, UnitOfWork<T>>();

            services.AddDbContext<T>(options => options.UseSqlServer(connectionString), serviceLifetime);

            return services;
        }
    }
}