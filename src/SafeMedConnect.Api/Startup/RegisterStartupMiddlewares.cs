using SafeMedConnect.Api.Extensions;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

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
        app.UseStatusCodePages();

        app.UseAuthentication();
        app.UseAuthorization();

        var root = app
            .MapGroup(string.Empty)
            .RequireAuthorization()
            .AddFluentValidationAutoValidation()
            .MapGroup("api");

        root.MapRoutes();

        // TODO: Temperature
        // TODO: Mental health
        // TODO: Sleep
        // TODO: Blood sugar
        // TODO: Medical visits
        return app;
    }
}