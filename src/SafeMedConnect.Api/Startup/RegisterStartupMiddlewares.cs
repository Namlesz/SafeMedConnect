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

        // TODO: Maybe initialize via reflection(?)
        // Map endpoints
        app.RegisterAccountEndpoints();
        return app;
    }
}