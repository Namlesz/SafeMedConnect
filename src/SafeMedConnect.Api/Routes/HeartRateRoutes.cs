using Microsoft.AspNetCore.Mvc;
using SafeMedConnect.Api.Attributes;
using SafeMedConnect.Api.Interfaces;
using SafeMedConnect.Application.Commands.HeartRate;
using SafeMedConnect.Application.Queries.HeartRate;
using SafeMedConnect.Domain.Entities;

namespace SafeMedConnect.Api.Routes;

internal sealed class HeartRateRoutes : IRoutes
{
    private const string RouteName = "HeartRate";

    public void RegisterRoutes(RouteGroupBuilder root)
    {
        var group = root.MapGroup(RouteName)
            .WithOpenApi()
            .WithTags(RouteName);

        group.MapPost("/", AddHeartRateMeasurement)
            .WithSummary("Add a new heart rate measurement")
            .Produces<List<HeartRateMeasurementEntity>>();

        group.MapGet("/", GetHeartRateMeasurements)
            .WithSummary("Get all heart rate measurements")
            .Produces<List<HeartRateMeasurementEntity>>();
    }

    private static async Task<IResult> AddHeartRateMeasurement(
        [Validate][FromBody] AddHeartRateMeasurementCommand command,
        IResponseHandler responseHandler,
        CancellationToken cnl
    ) => await responseHandler.SendAndHandle(command, cnl);

    private static async Task<IResult> GetHeartRateMeasurements(
        IResponseHandler responseHandler,
        CancellationToken cnl
    ) => await responseHandler.SendAndHandle(new GetHeartRateMeasurementsQuery(), cnl);
}