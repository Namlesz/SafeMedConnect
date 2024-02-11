using Microsoft.AspNetCore.Mvc;
using SafeMedConnect.Api.Attributes;
using SafeMedConnect.Api.Interfaces;
using SafeMedConnect.Application.Commands.Share;
using SafeMedConnect.Application.Dto;
using SafeMedConnect.Application.Queries.Share;
using SafeMedConnect.Domain.Authorization;

namespace SafeMedConnect.Api.Routes;

[ApiRoute("share")]
internal sealed class ShareRoutes : IRoutes
{
    public void RegisterRoutes(RouteGroupBuilder group)
    {
        group.MapPost("share-data", async (
                    [FromBody] ShareDataCommand command,
                    CancellationToken cnl,
                    IResponseHandler responseHandler
                ) => await responseHandler.SendAndHandle(command, cnl)
            )
            .WithSummary("Generate a token to share data with a guest user")
            .Produces<TokenResponseDto>()
            .RequireAuthorization();

        group.MapGet("get-shared-data", async (
                    CancellationToken cnl,
                    IResponseHandler responseHandler
                ) => await responseHandler.SendAndHandle(new GetSharedDataQuery(), cnl)
            )
            .WithSummary("Get data shared from user via token")
            .Produces(StatusCodes.Status401Unauthorized)
            .RequireAuthorization(PolicyNames.GuestPolicy);
    }
}