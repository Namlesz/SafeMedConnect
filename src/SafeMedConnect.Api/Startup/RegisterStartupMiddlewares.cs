using SafeMedConnect.Api.Endpoints;

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

        // Map endpoints
        //? (maybe change to use reflection)
        app.RegisterAccountEndpoints();
        return app;
    }
}