using Microsoft.AspNetCore.Mvc;
using SafeMedConnect.Api.Attributes;
using SafeMedConnect.Api.Interfaces;
using SafeMedConnect.Application.Commands.Account;
using SafeMedConnect.Application.Dto;

namespace SafeMedConnect.Api.Routes;

[ApiRoute("account")]
internal class AccountRoutes : IRoutes
{
    public void RegisterRoutes(RouteGroupBuilder group)
    {
        group.MapPost("/register", RegisterApplicationUser)
            .AllowAnonymous()
            .WithSummary("Register a new user")
            .WithDescription("Create a new user account in the system")
            .Produces(StatusCodes.Status204NoContent);

        group.MapPost("/login", LoginApplicationUser)
            .AllowAnonymous()
            .WithSummary("Login user")
            .WithDescription("Login a user and return a JWT token")
            .Produces<TokenResponseDto>();
    }

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