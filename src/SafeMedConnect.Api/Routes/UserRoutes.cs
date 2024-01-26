using Microsoft.AspNetCore.Mvc;
using SafeMedConnect.Api.Interfaces;
using SafeMedConnect.Application.Commands.User;
using SafeMedConnect.Application.Queries.User;
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
            .WithSummary("Update user information")
            .WithDescription("Replace all user information with the provided information (including null values)")
            .Produces<UserEntity>();

        group.MapGet("", GetUserInformation)
            .WithSummary("Get user information")
            .WithDescription("Get all user information from the database")
            .Produces<UserEntity>();
    }

    private static async Task<IResult> UpdateUserInformation(
        [FromBody] UpdateUserInformationCommand command,
        IResponseHandler responseHandler,
        CancellationToken cnl
    ) => await responseHandler.SendAndHandle(command, cnl);

    private static async Task<IResult> GetUserInformation(
        IResponseHandler responseHandler,
        CancellationToken cnl
    ) => await responseHandler.SendAndHandle(new GetUserInformationQuery(), cnl);
}