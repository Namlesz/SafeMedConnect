using Microsoft.AspNetCore.Mvc;
using SafeMedConnect.Api.Attributes;
using SafeMedConnect.Api.Interfaces;
using SafeMedConnect.Application.Commands.BloodSugar;
using SafeMedConnect.Application.Queries.BloodSugar;
using SafeMedConnect.Domain.Entities;

namespace SafeMedConnect.Api.Routes;

[ApiRoute("blood-sugar")]
internal sealed class BloodSugarRoutes : IRoutes
{
    public void RegisterRoutes(RouteGroupBuilder group)
    {
        group.RequireAuthorization();

        group.MapGet("/", async (
                    CancellationToken cnl,
                    IResponseHandler responseHandler
                ) => await responseHandler.SendAndHandle(new GetBloodSugarQuery(), cnl)
            )
            .WithSummary("Get all blood sugar measurements")
            .Produces<List<BloodSugarMeasurementDto>>()
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (
                    [FromBody] AddBloodSugarCommand command,
                    CancellationToken cnl,
                    IResponseHandler responseHandler
                ) => await responseHandler.SendAndHandle(command, cnl)
            )
            .WithSummary("Add a new blood sugar measurement")
            .Produces<List<BloodSugarMeasurementEntity>>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

        group.MapDelete("/", async (
                    [FromBody] DeleteBloodSugarCommand command,
                    CancellationToken cnl,
                    IResponseHandler responseHandler
                ) => await responseHandler.SendAndHandle(command, cnl)
            )
            .WithSummary("Delete a blood sugar measurement")
            .Produces<List<BloodSugarMeasurementEntity>>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);
    }
}