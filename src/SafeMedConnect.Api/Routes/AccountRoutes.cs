using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeMedConnect.Api.Attributes;
using SafeMedConnect.Api.Interfaces;
using SafeMedConnect.Application.Commands;
using static SafeMedConnect.Common.Constants.ApiPrefixConstants;

namespace SafeMedConnect.Api.Routes;

internal class AccountRoutes : IRoutes
{
    public void RegisterRoutes(RouteGroupBuilder root)
    {
        var group = root.MapGroup(BaseApiPrefix + "/account")
            .WithOpenApi()
            .WithTags("Account");

        group.MapPost("/register", RegisterApplicationUser)
            .WithSummary("Register a new user")
            .WithDescription("Register a new user")
            .Produces<string>();

        group.MapPost("/login", LoginApplicationUser)
            .WithSummary("Register a new user")
            .WithDescription("Register a new user")
            .Produces<string>();
    }

    private static async Task<IResult> LoginApplicationUser(
        [Validate][FromBody] LoginApplicationUserCommand command,
        IMediator mediator
    ) => await mediator.Send(command) switch
    {
        _ => Results.Ok()
    };

    // TODO: Implement
    private static async Task<IResult> RegisterApplicationUser(
        [Validate][FromBody] RegisterApplicationUserCommand command,
        IMediator mediator
    ) => await mediator.Send(command) switch
    {
        true => Results.Ok(),
        false => Results.Problem()
    };
}