using Microsoft.AspNetCore.Mvc;
using SafeMedConnect.Api.Attributes;
using SafeMedConnect.Api.Interfaces;
using SafeMedConnect.Application.Commands.Temperature;
using SafeMedConnect.Application.Queries.Temperature;
using SafeMedConnect.Domain.Entities;

namespace SafeMedConnect.Api.Routes;

[ApiRoute("temperature")]
internal sealed class TemperatureRoutes : IRoutes
{
    public void RegisterRoutes(RouteGroupBuilder group)
    {
        group.RequireAuthorization();

        group.MapGet("/", async (
                    CancellationToken cnl,
                    IResponseHandler responseHandler
                ) => await responseHandler.SendAndHandle(new GetTemperatureQuery(), cnl)
            )
            .WithSummary("Get all temperature measurements")
            .Produces<List<TemperatureMeasurementEntity>>()
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (
                    [FromBody] AddTemperatureCommand command,
                    CancellationToken cnl,
                    IResponseHandler responseHandler
                ) => await responseHandler.SendAndHandle(command, cnl)
            )
            .WithSummary("Add a new temperature measurement")
            .Produces<List<TemperatureMeasurementEntity>>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

        group.MapDelete("/", async (
                    [FromBody] DeleteTemperatureCommand command,
                    CancellationToken cnl,
                    IResponseHandler responseHandler
                ) => await responseHandler.SendAndHandle(command, cnl)
            )
            .WithSummary("Delete a temperature measurement")
            .Produces<List<TemperatureMeasurementEntity>>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);
    }
}