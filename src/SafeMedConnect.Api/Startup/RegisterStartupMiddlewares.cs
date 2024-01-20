using SafeMedConnect.Api.Extensions;
using SafeMedConnect.Api.Helpers;

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
            .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory)
            .MapGroup("api");

        root.MapRoutes();

        return app;
    }
}