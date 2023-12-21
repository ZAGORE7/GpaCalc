using Microsoft.Extensions.Configuration;
using Serilog;

namespace Ozbul.Application.Portal.Api.Extensions;

public static class WebHostBuilderExtensions
{
    public static void ConfigureLogger(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(builder.Configuration)
                            .CreateLogger();

        builder.Host.UseSerilog();
    }

    public static void ConfigureSettings(this WebApplicationBuilder builder)
    {
        builder.Configuration
            .AddJsonFile("appsettings.json", false)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true)
#if DEBUG
            .AddJsonFile($"appsettings.Local.json", true)
 #endif
            ;

    }
}