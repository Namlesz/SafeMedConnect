using SafeMedConnect.Api.Extensions;

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

        var root = app.MapGroup(string.Empty);
        root.AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory);
        root.MapRoutes();

        return app;
    }
}