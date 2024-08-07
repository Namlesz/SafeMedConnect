using Microsoft.AspNetCore.Mvc;
using SafeMedConnect.Api.Abstract;
using SafeMedConnect.Api.Attributes;
using SafeMedConnect.Application.Commands.HeartRate;
using SafeMedConnect.Application.Queries.HeartRate;
using SafeMedConnect.Domain.Entities;

namespace SafeMedConnect.Api.Routes;

[ApiRoute("heart-rate")]
internal sealed class HeartRateRoutes : IRoutes
{
    public void RegisterRoutes(RouteGroupBuilder group)
    {
        group.RequireAuthorization();

        group.MapGet("/", async (
                    CancellationToken cnl,
                    IResponseFactory responseHandler
                ) => await responseHandler.SendAndHandle(new GetHeartRateMeasurementsQuery(), cnl)
            )
            .WithSummary("Get all heart rate measurements")
            .Produces<List<HeartRateMeasurementEntity>>()
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (
                    [FromBody] AddHeartRateMeasurementCommand command,
                    CancellationToken cnl,
                    IResponseFactory responseHandler
                ) => await responseHandler.SendAndHandle(command, cnl)
            )
            .WithSummary("Add a new heart rate measurement")
            .Produces<List<HeartRateMeasurementEntity>>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

        group.MapDelete("/", async (
                    [FromBody] DeleteHeartRateMeasurementCommand command,
                    CancellationToken cnl,
                    IResponseFactory responseHandler
                ) => await responseHandler.SendAndHandle(command, cnl)
            )
            .WithSummary("Delete a heart rate measurement")
            .Produces<List<HeartRateMeasurementEntity>>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);
    }
}