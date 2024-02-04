using Microsoft.AspNetCore.Mvc;
using SafeMedConnect.Api.Attributes;
using SafeMedConnect.Api.Interfaces;
using SafeMedConnect.Application.Commands.Share;
using SafeMedConnect.Application.Queries.Share;

namespace SafeMedConnect.Api.Routes;

[ApiRoute("share")]
internal sealed class ShareRoutes : IRoutes
{
    public void RegisterRoutes(RouteGroupBuilder group)
    {
        /* TODO: (POST) share/share-data
         - Select what data to share
         - Use JWT token to share data
         - Requires token
         - (Optional) Generate redirect URL with token
         - Policy: UserPolicy
        */
        group.MapPost("share-data", async (
                [FromBody] ShareDataCommand command,
                CancellationToken cnl,
                IResponseHandler responseHandler
            ) => await responseHandler.SendAndHandle(command, cnl)
        );

        /* TODO: (GET) share/get-shared-data
         - Get data shared from user via token
         - Requires token
         - Get user id from token
         - Get what data is shared from token
         - Returns shared data
         - Returns 404 if no data is shared
         - Policy: GuestPolicy
        */
        group.MapGet("get-shared-data", async (
                [FromQuery] string token,
                CancellationToken cnl,
                IResponseHandler responseHandler
            ) => await responseHandler.SendAndHandle(new GetSharedDataQuery(), cnl)
        );
    }
}