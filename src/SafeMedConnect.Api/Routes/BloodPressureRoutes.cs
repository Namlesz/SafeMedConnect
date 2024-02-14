using Microsoft.AspNetCore.Mvc;
using SafeMedConnect.Api.Attributes;
using SafeMedConnect.Api.Interfaces;
using SafeMedConnect.Application.Commands.BloodPressure;
using SafeMedConnect.Application.Queries.BloodPressure;
using SafeMedConnect.Domain.Entities;

namespace SafeMedConnect.Api.Routes;

[ApiRoute("blood-pressure")]
internal sealed class BloodPressureRoutes : IRoutes
{
    public void RegisterRoutes(RouteGroupBuilder group)
    {
        group.RequireAuthorization();

        group.MapGet("/", async (
                    CancellationToken cnl,
                    IResponseHandler responseHandler
                ) => await responseHandler.SendAndHandle(new GetBloodPressureQuery(), cnl)
            )
            .WithSummary("Get all blood pressure measurements")
            .Produces<List<BloodPressureMeasurementEntity>>()
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (
                    [FromBody] AddBloodPressureCommand command,
                    CancellationToken cnl,
                    IResponseHandler responseHandler
                ) => await responseHandler.SendAndHandle(command, cnl)
            )
            .WithSummary("Add a new blood pressure measurement")
            .Produces<List<BloodPressureMeasurementEntity>>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

        group.MapDelete("/", async (
                    [FromBody] DeleteBloodPressureCommand command,
                    CancellationToken cnl,
                    IResponseHandler responseHandler
                ) => await responseHandler.SendAndHandle(command, cnl)
            )
            .WithSummary("Delete a blood pressure measurement")
            .Produces<List<BloodPressureMeasurementEntity>>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);
    }
}