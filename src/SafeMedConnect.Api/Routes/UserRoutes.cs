using Microsoft.AspNetCore.Mvc;
using SafeMedConnect.Api.Attributes;
using SafeMedConnect.Api.Interfaces;
using SafeMedConnect.Application.Commands.User;
using SafeMedConnect.Application.Queries.User;
using SafeMedConnect.Domain.Entities;

namespace SafeMedConnect.Api.Routes;

[ApiRoute("user")]
internal sealed class UserRoutes : IRoutes
{
    public void RegisterRoutes(RouteGroupBuilder group)
    {
        group.MapPost("", async (
                [FromBody] UpdateUserInformationCommand command,
                CancellationToken cnl,
                IResponseHandler responseHandler
            ) => await responseHandler.SendAndHandle(command, cnl))
            .WithSummary("Update user information")
            .WithDescription("Replace all user information with the provided information (including null values)")
            .Produces<UserEntity>()
            .Produces(StatusCodes.Status500InternalServerError);

        group.MapGet("", async (
                CancellationToken cnl,
                IResponseHandler responseHandler
            ) => await responseHandler.SendAndHandle(new GetUserInformationQuery(), cnl))
            .WithSummary("Get user information")
            .WithDescription("Get all user information from the database")
            .Produces<UserEntity>()
            .Produces(StatusCodes.Status404NotFound);
    }
}