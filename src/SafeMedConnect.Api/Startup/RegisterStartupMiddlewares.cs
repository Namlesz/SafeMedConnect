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
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        }
        else
        {
            app.UseCors(x => x.WithOrigins().AllowAnyHeader().AllowAnyMethod());
        }

        app.UseHttpsRedirection();
        app.UseStatusCodePages();

        app.UseAuthentication();
        app.UseAuthorization();

        var root = app
            .MapGroup(string.Empty)
            .AddFluentValidationAutoValidation()
            .MapGroup("api");

        root.MapRoutes();
        return app;
    }
}