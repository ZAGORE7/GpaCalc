using System.Text;
using Microsoft.EntityFrameworkCore;
using Ozbul.Application.Portal.Repository;
using Ozbul.Application.Portal.Repository.Interfaces;
using Ozbul.Application.Portal.Services;
using Ozbul.Application.Portal.Services.Abstractions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Configuration;
using Microsoft.OpenApi.Models;

namespace Ozbul.Application.Portal.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        RegisterControllers(services);
        RegisterSwagger(services);
        RegisterServices(services);
        

        return services;
    }

    public static IServiceCollection ConfigureOptions (this IServiceCollection services, IConfiguration configuration)
    {
       services.Configure<WebProtocolSettings>(configuration.GetSection("WebProtocolSettings"));

        return services;
    }
    public static IServiceCollection RegisterDatabaseConnection (this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("AppDbContext");

        if (connectionString is null)
        {
            Console.Error.WriteLine("ERROR: Connection string not found");
            return services;
        }


      
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));


        return services;
    }

    private static void RegisterControllers(IServiceCollection services) => services.AddControllers();

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork<AppDbContext>));
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<DesKeyManager>();
        services.AddScoped<RSAHelper>();
    }



    private static void RegisterSwagger(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();


    }
   
}