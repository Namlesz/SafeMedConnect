using SafeMedConnect.Api.Attributes;
using SafeMedConnect.Api.Interfaces;

namespace SafeMedConnect.Api.Routes;

[ApiRoute("share")]
internal sealed class ShareRoutes : IRoutes
{
    public void RegisterRoutes(RouteGroupBuilder group)
    {
        // TODO: Add routes for sharing data
        /* TODO: (POST) share
         - Select what data to share
         - Use JWT token to share data
         - Requires token
         - (Optional) Generate redirect URL with token
         - Policy: UserPolicy
        */

        /* TODO: (GET) share
         - Get data shared from user via token
         - Requires token
         - Get user id from token
         - Get what data is shared from token
         - Returns shared data
         - Returns 404 if no data is shared
         - Policy: GuestPolicy
        */
    }
}