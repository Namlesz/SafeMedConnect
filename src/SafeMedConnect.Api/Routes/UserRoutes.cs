using Microsoft.AspNetCore.Mvc;
using SafeMedConnect.Api.Attributes;
using SafeMedConnect.Api.Interfaces;
using SafeMedConnect.Application.Commands.User;
using SafeMedConnect.Domain.Entities;

namespace SafeMedConnect.Api.Routes;

internal sealed class UserRoutes : IRoutes
{
    private const string RouteName = "User";

    public void RegisterRoutes(RouteGroupBuilder root)
    {
        var group = root.MapGroup(RouteName)
            .WithOpenApi()
            .WithTags(RouteName);

        group.MapPost("", UpdateUserInformation)
            .WithDescription("Update user information")
            .Produces<UserEntity>();
    }

    private static async Task<IResult> UpdateUserInformation(
        [Validate][FromBody] UpdateUserInformationCommand command,
        IResponseHandler responseHandler,
        CancellationToken cnl
    ) => await responseHandler.SendAndHandle(command, cnl);
}