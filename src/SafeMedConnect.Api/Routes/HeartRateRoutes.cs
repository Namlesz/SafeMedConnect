using Microsoft.AspNetCore.Mvc;
using SafeMedConnect.Api.Attributes;
using SafeMedConnect.Api.Interfaces;
using SafeMedConnect.Application.Commands.HeartRate;
using SafeMedConnect.Application.Queries.HeartRate;
using SafeMedConnect.Domain.Entities;

namespace SafeMedConnect.Api.Routes;

[ApiRoute("heart-rate")]
internal sealed class HeartRateRoutes : IRoutes
{
    public void RegisterRoutes(RouteGroupBuilder group)
    {
        group.MapPost("/", AddHeartRateMeasurement)
            .WithSummary("Add a new heart rate measurement")
            .Produces<List<HeartRateMeasurementEntity>>();

        group.MapGet("/", GetHeartRateMeasurements)
            .WithSummary("Get all heart rate measurements")
            .Produces<List<HeartRateMeasurementEntity>>();

        group.MapDelete("/", DeleteHeartRateMeasurement)
            .WithSummary("Delete a heart rate measurement")
            .Produces<List<HeartRateMeasurementEntity>>();
    }

    private static async Task<IResult> GetHeartRateMeasurements(
        IResponseHandler responseHandler,
        CancellationToken cnl
    ) => await responseHandler.SendAndHandle(new GetHeartRateMeasurementsQuery(), cnl);

    private static async Task<IResult> AddHeartRateMeasurement(
        [Validate][FromBody] AddHeartRateMeasurementCommand command,
        IResponseHandler responseHandler,
        CancellationToken cnl
    ) => await responseHandler.SendAndHandle(command, cnl);

    private static async Task<IResult> DeleteHeartRateMeasurement(
        [Validate][FromBody] DeleteHeartRateMeasurementCommand command,
        IResponseHandler responseHandler,
        CancellationToken cnl
    ) => await responseHandler.SendAndHandle(command, cnl);
}