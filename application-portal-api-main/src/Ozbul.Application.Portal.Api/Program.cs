using Ozbul.Application.Portal.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLogger();

builder.Services
    .RegisterDatabaseConnection(builder.Configuration)
    .RegisterApplicationServices()
    .ConfigureOptions(builder.Configuration);

var app = builder.Build();
app.RegisterMiddlewares();
app.RegisterEndpoints();

await app.InitializeAsync();




app.Run();