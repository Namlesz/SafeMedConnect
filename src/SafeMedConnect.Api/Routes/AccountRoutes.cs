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
            .WithSummary("Register a new user")
            .WithDescription("Create a new user account in the system")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .AllowAnonymous();

        group.MapPost("/login", async (
                    [FromBody] LoginApplicationUserCommand command,
                    CancellationToken cnl,
                    IResponseHandler responseHandler
                ) => await responseHandler.SendAndHandle(command, cnl)
            )
            .WithSummary("Login user")
            .WithDescription("Login a user and return a JWT token")
            .Produces<TokenResponseDto>()
            .Produces(StatusCodes.Status400BadRequest)
            .AllowAnonymous();

        group.MapPost("/add-mfa-authenticator", async (
                    CancellationToken cnl,
                    IResponseHandler responseHandler
                ) => await responseHandler.SendAndHandle(new AddMfaAuthenticatorCommand(), cnl)
            )
            .WithSummary("Add MFA to user")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization();

        group.MapPost("/verify-mfa-authenticator", async (
                    [FromBody] VerifyMfaAuthenticatorCommand command,
                    CancellationToken cnl,
                    IResponseHandler responseHandler
                ) => await responseHandler.SendAndHandle(command, cnl)
            )
            .WithSummary("Verify MFA configuration")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();

        // TODO: Remove mfa endpoint
    }
}