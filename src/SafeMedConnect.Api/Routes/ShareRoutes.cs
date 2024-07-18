using Microsoft.AspNetCore.Mvc;
using SafeMedConnect.Api.Abstract;
using SafeMedConnect.Api.Attributes;
using SafeMedConnect.Application.Commands.Share;
using SafeMedConnect.Application.Dto;
using SafeMedConnect.Application.Dto.Measurements;
using SafeMedConnect.Application.Queries.Share;
using SafeMedConnect.Domain.Auth;

namespace SafeMedConnect.Api.Routes;

[ApiRoute("share")]
internal sealed class ShareRoutes : IRoutes
{
    public void RegisterRoutes(RouteGroupBuilder group)
    {
        group.MapPost("share-data", async (
                    [FromBody] ShareDataCommand command,
                    CancellationToken cnl,
                    IResponseFactory responseHandler
                ) => await responseHandler.SendAndHandle(command, cnl)
            )
            .WithSummary("Generate a token to share data with a guest user")
            .Produces<TokenResponseDto>()
            .RequireAuthorization();

        group.MapGet("get-shared-data", async (
                    CancellationToken cnl,
                    IResponseFactory responseHandler
                ) => await responseHandler.SendAndHandle(new GetSharedDataQuery(), cnl)
            )
            .WithSummary("Get data shared from user via token")
            .Produces<SharedDataDto>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .RequireAuthorization(PolicyNames.GuestPolicy);

        group.MapGet("user-information", async (
                    CancellationToken cnl,
                    IResponseFactory responseHandler
                ) => await responseHandler.SendAndHandle(new GetSharedUserInformationQuery(), cnl)
            )
            .WithSummary("Get user information shared from user via token")
            .Produces<UserDto>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .RequireAuthorization(PolicyNames.GuestPolicy);

        group.MapGet("blood-pressure", async (
                    CancellationToken cnl,
                    IResponseFactory responseHandler
                ) => await responseHandler.SendAndHandle(new GetSharedBloodPressureQuery(), cnl)
            )
            .WithSummary("Get blood pressure measurements")
            .Produces<List<BloodPressureDto>>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(PolicyNames.GuestPolicy);

        group.MapGet("heart-rate", async (
                    CancellationToken cnl,
                    IResponseFactory responseHandler
                ) => await responseHandler.SendAndHandle(new GetSharedHeartRateQuery(), cnl)
            )
            .WithSummary("Get heart rate measurements")
            .Produces<List<HeartRateDto>>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(PolicyNames.GuestPolicy);

        group.MapGet("temperature", async (
                    CancellationToken cnl,
                    IResponseFactory responseHandler
                ) => await responseHandler.SendAndHandle(new GetSharedTemperatureQuery(), cnl)
            )
            .WithSummary("Get temperature measurements")
            .Produces<List<TemperatureDto>>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(PolicyNames.GuestPolicy);

        group.MapGet("blood-sugar", async (
                    CancellationToken cnl,
                    IResponseFactory responseHandler
                ) => await responseHandler.SendAndHandle(new GetSharedBloodSugarQuery(), cnl)
            )
            .WithSummary("Get blood sugar measurements")
            .Produces<List<BloodSugarDto>>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(PolicyNames.GuestPolicy);
    }
}