using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeMedConnect.Application.Queries;
using static SafeMedConnect.Common.Constants.ApiPrefixConstants;

namespace SafeMedConnect.Api.Endpoints;

internal static class AccountEndpoints
{
    public static void RegisterAccountEndpoints(this WebApplication app)
    {
        var group = app.MapGroup(BaseApiPrefix + "/account")
            .WithOpenApi()
            .WithTags("Account");

        group.MapPost("/hello", HelloWorld)
            .WithSummary("Test summary")
            .WithDescription("Test description")
            .Produces<string>();
    }

    private static async Task<IResult> HelloWorld([FromBody] GetAccountNameQuery query, IMediator mediator)
    {
        var res = await mediator.Send(query);
        return Results.Ok(res);
    }
}