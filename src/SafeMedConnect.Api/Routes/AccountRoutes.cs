using Microsoft.AspNetCore.Mvc;
using SafeMedConnect.Api.Attributes;
using SafeMedConnect.Api.Interfaces;
using SafeMedConnect.Application.Commands.Account;
using SafeMedConnect.Application.Dto;

namespace SafeMedConnect.Api.Routes;

[ApiRoute("account")]
internal sealed class AccountRoutes : IRoutes
{
    public void RegisterRoutes(RouteGroupBuilder group)
    {
        group.MapPost("/register", async (
                    [FromBody] RegisterApplicationUserCommand command,
                    CancellationToken cnl,
                    IResponseHandler responseHandler
                ) => await responseHandler.SendAndHandle(command, cnl)
            )
            .AllowAnonymous()
            .WithSummary("Register a new user")
            .WithDescription("Create a new user account in the system")
            .Produces(StatusCodes.Status204NoContent);

        group.MapPost("/login", async (
                    [FromBody] LoginApplicationUserCommand command,
                    CancellationToken cnl,
                    IResponseHandler responseHandler
                ) => await responseHandler.SendAndHandle(command, cnl)
            )
            .AllowAnonymous()
            .WithSummary("Login user")
            .WithDescription("Login a user and return a JWT token")
            .Produces<TokenResponseDto>();
    }
}