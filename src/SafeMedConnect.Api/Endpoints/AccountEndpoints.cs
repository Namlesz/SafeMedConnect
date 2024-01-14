using static SafeMedConnect.Common.Constants.ApiPrefixConstants;

namespace SafeMedConnect.Api.Endpoints;

internal static class AccountEndpoints
{
    public static void RegisterAccountEndpoints(this WebApplication app)
    {
        var group = app.MapGroup(BaseApiPrefix + "/account")
            .WithOpenApi()
            .WithTags("Account");

        group.MapGet("/hello", () => "Hello World!")
            .WithSummary("Test summary")
            .WithDescription("Test description")
            .Produces<string>();
    }
}