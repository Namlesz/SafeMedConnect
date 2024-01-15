using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeMedConnect.Api.Attributes;
using SafeMedConnect.Api.Interfaces;
using SafeMedConnect.Application.Queries;
using static SafeMedConnect.Common.Constants.ApiPrefixConstants;

namespace SafeMedConnect.Api.Routes;

internal class AccountRoutes : IRoutes
{
    public void RegisterRoutes(RouteGroupBuilder root)
    {
        var group = root.MapGroup(BaseApiPrefix + "/account")
            .WithOpenApi()
            .WithTags("Account");

        group.MapPost("/hello", HelloWorld)
            .WithSummary("Test summary")
            .WithDescription("Test description")
            .Produces<string>();
    }

    private static async Task<IResult> HelloWorld(
        [Validate][FromBody] GetAccountNameQuery query,
        IMediator mediator
    )
    {
        var res = await mediator.Send(query);
        return Results.Ok(res);
    }
}