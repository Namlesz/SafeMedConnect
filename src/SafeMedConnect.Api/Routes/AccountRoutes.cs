using Microsoft.AspNetCore.Mvc;
using SafeMedConnect.Api.Attributes;
using SafeMedConnect.Api.Interfaces;
using SafeMedConnect.Application.Commands;

namespace SafeMedConnect.Api.Routes;

internal class AccountRoutes : IRoutes
{
    private const string RouteName = "Account";

    public void RegisterRoutes(RouteGroupBuilder root)
    {
        var group = root.MapGroup(RouteName)
            .WithOpenApi()
            .WithTags(RouteName);

        group.MapPost("/register", RegisterApplicationUser)
            // .AllowAnonymous()
            .WithDescription("Register a new user")
            .Produces<string>();

        group.MapPost("/login", LoginApplicationUser)
            // .AllowAnonymous()
            .WithDescription("Login user")
            .Produces<string>();
    }

    // TODO: Implement token generation
    private static async Task<IResult> LoginApplicationUser(
        [Validate][FromBody] LoginApplicationUserCommand command,
        IResponseHandler responseHandler,
        CancellationToken cnl
    ) => await responseHandler.SendAndHandle(command, cnl);

    private static async Task<IResult> RegisterApplicationUser(
        [Validate][FromBody] RegisterApplicationUserCommand command,
        IResponseHandler responseHandler,
        CancellationToken cnl
    ) => await responseHandler.SendAndHandle(command, cnl);
}