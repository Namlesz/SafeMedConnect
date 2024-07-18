using Microsoft.AspNetCore.Mvc;
using SafeMedConnect.Api.Abstract;
using SafeMedConnect.Api.Attributes;
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
                    IResponseFactory responseHandler
                ) => await responseHandler.SendAndHandle(command, cnl)
            )
            .WithSummary("Register a new user")
            .WithDescription("Create a new user account in the system")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status500InternalServerError)
            .AllowAnonymous();

        group.MapPost("/login", async (
                    [FromBody] LoginApplicationUserCommand command,
                    CancellationToken cnl,
                    IResponseFactory responseHandler
                ) => await responseHandler.SendAndHandle(command, cnl)
            )
            .WithSummary("Login user")
            .WithDescription(
                "(400 - Bad request) "
                + "(401 - Invalid login or password) "
                + "(403 - Invalid MFA code) "
                + "(500 - MFA not configured | Unknown error occurred)"
            )
            .Produces<TokenResponseDto>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status500InternalServerError)
            .AllowAnonymous();

        group.MapPost("/add-mfa-authenticator", async (
                    CancellationToken cnl,
                    IResponseFactory responseHandler
                ) => await responseHandler.SendAndHandle(new AddMfaAuthenticatorCommand(), cnl)
            )
            .WithSummary("Add MFA to user")
            .Produces<QrCodeDto>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();

        group.MapPost("/verify-mfa-authenticator", async (
                    [FromBody] VerifyMfaAuthenticatorCommand command,
                    CancellationToken cnl,
                    IResponseFactory responseHandler
                ) => await responseHandler.SendAndHandle(command, cnl)
            )
            .WithSummary("Verify MFA configuration")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();

        group.MapPost("/remove-mfa-authenticator", async (
                    CancellationToken cnl,
                    IResponseFactory responseHandler
                ) => await responseHandler.SendAndHandle(new RemoveMfaAuthenticatorCommand(), cnl)
            )
            .WithSummary("Remove MFA from user")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
    }
}