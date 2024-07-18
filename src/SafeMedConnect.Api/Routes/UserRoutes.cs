using Microsoft.AspNetCore.Mvc;
using SafeMedConnect.Api.Abstract;
using SafeMedConnect.Api.Attributes;
using SafeMedConnect.Application.Commands.User;
using SafeMedConnect.Application.Dto;
using SafeMedConnect.Application.Queries.User;

namespace SafeMedConnect.Api.Routes;

[ApiRoute("user")]
internal sealed class UserRoutes : IRoutes
{
    public void RegisterRoutes(RouteGroupBuilder group)
    {
        group.RequireAuthorization();

        group.MapPost("", async (
                    [FromBody] UpdateUserInformationCommand command,
                    CancellationToken cnl,
                    IResponseFactory responseHandler
                ) => await responseHandler.SendAndHandle(command, cnl)
            )
            .WithSummary("Update user information")
            .WithDescription("Replace all user information with the provided information (including null values)")
            .Produces<UserDto>()
            .Produces(StatusCodes.Status500InternalServerError);

        group.MapGet("", async (
                    CancellationToken cnl,
                    IResponseFactory responseHandler
                ) => await responseHandler.SendAndHandle(new GetUserInformationQuery(), cnl)
            )
            .WithSummary("Get user information")
            .WithDescription("Get all user information from the database")
            .Produces<UserDto>()
            .Produces(StatusCodes.Status404NotFound);
    }
}