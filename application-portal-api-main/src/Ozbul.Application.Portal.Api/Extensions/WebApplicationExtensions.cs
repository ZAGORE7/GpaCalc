using System;
using Microsoft.EntityFrameworkCore;
using Ozbul.Application.Portal.Api.Middlewares;
using Ozbul.Application.Portal.Repository;

namespace Ozbul.Application.Portal.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication RegisterEndpoints(this WebApplication app)
    {
        RegisterSwagger(app);

        RegisterHttps(app);

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }

    private static void RegisterHttps(WebApplication app)
    {
        app.UseHttpsRedirection();
    }

    public static WebApplication RegisterMiddlewares(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();

        return app;
    }

    private static void RegisterSwagger(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
    
    public static async Task InitializeAsync(this WebApplication app)
    {
        using var serviceScope = app.Services.CreateScope();
        var services = serviceScope.ServiceProvider;
        try
        {
            var logger = services.GetRequiredService<ILogger<WebApplication>>();
            var environment = services.GetRequiredService<IWebHostEnvironment>();
            var dbContext = services.GetRequiredService<AppDbContext>();

            logger.LogInformation($"Hosting environment: {environment.EnvironmentName}");
            logger.LogInformation("Initializing the database and applying migrations");

            await dbContext.Database.MigrateAsync();

            await dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            var logger = services.GetRequiredService<ILogger<WebApplication>>();
            logger.LogError(e.Message, e);
        }
    }
}