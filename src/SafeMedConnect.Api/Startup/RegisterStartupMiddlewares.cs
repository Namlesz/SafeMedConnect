using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeMedConnect.Api.Endpoints;
using SafeMedConnect.Application.Queries;

namespace SafeMedConnect.Api.Startup;

internal static class RegisterStartupMiddlewares
{
    public static WebApplication RegisterAppMiddlewares(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        // Only test purposes
        var mediator = app.Services.GetService<IMediator>();
        app.MapPost("/test", ([FromBody] GetAccountNameQuery query) => mediator!.Send(query));

        //? (maybe change to use reflection or pass mediator as parameter)
        // Map endpoints
        app.RegisterAccountEndpoints();
        return app;
    }
}