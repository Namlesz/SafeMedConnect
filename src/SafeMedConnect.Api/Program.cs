using SafeMedConnect.Api.Startup;
using SafeMedConnect.Application;
using SafeMedConnect.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Host.ConfigureAndUseSerilog();
builder.Services.RegisterApiServices(configuration);
builder.Services.RegisterInfrastructureServices(configuration);
builder.Services.RegisterApplicationServices();

var app = builder
    .Build()
    .RegisterAppMiddlewares();

app.Run();