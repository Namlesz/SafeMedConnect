using Microsoft.AspNetCore.Mvc;
using SafeMedConnect.Api.Attributes;
using SafeMedConnect.Api.Interfaces;
using SafeMedConnect.Application.Commands.BloodPressure;
using SafeMedConnect.Domain.Entities;

namespace SafeMedConnect.Api.Routes;

[ApiRoute("blood-pressure")]
internal sealed class BloodPressureRoutes : IRoutes
{
    public void RegisterRoutes(RouteGroupBuilder group)
    {
        group.MapPost("/", async (
                    [Validate][FromBody] AddBloodPressureCommand command,
                    CancellationToken cnl,
                    IResponseHandler responseHandler
                ) => await responseHandler.SendAndHandle(command, cnl)
            )
            .WithSummary("Add a new blood pressure measurement")
            .Produces<List<BloodPressureMeasurementEntity>>()
            .Produces(StatusCodes.Status500InternalServerError);

        // TODO: Implement GET and DELETE routes
    }
}