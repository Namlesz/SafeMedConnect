using MediatR;
using SafeMedConnect.Api.Interfaces;
using SafeMedConnect.Domain.Responses;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static SafeMedConnect.Domain.Responses.ResponseTypes;

namespace SafeMedConnect.Api.Helpers;

internal sealed class ResponseHandler(IMediator mediator) : IResponseHandler
{
    public async Task<IResult> SendAndHandle(IRequest<ResponseWrapper> request, CancellationToken cnl = default)
    {
        return await mediator.Send(request, cnl) switch
        {
            { ResponseType: Success }
                => Results.NoContent(),

            { ResponseType: NotFound } res
                => Results.Problem(statusCode: Status404NotFound, detail: res.Message),

            { ResponseType: Conflict } res
                => Results.Problem(statusCode: Status409Conflict, detail: res.Message),

            { ResponseType: Forbidden } res =>
                Results.Problem(statusCode: Status403Forbidden, detail: res.Message),

            { ResponseType: InvalidRequest } res
                => Results.Problem(statusCode: Status400BadRequest, detail: res.Message),

            { ResponseType: Error } res
                => Results.Problem(statusCode: Status500InternalServerError, detail: res.Message),

            _ => Results.Problem(statusCode: Status500InternalServerError, detail: "Unknown error occurred")
        };
    }

    public async Task<IResult> SendAndHandle<T>(IRequest<ResponseWrapper<T>> request, CancellationToken cnl) where T : class
    {
        return await mediator.Send(request, cnl) switch
        {
            { ResponseType: Success } res
                => Results.Ok(res.Data),

            { ResponseType: NotFound } res
                => Results.Problem(statusCode: Status404NotFound, detail: res.Message),

            { ResponseType: Conflict } res
                => Results.Problem(statusCode: Status409Conflict, detail: res.Message),

            { ResponseType: Forbidden } res =>
                Results.Problem(statusCode: Status403Forbidden, detail: res.Message),

            { ResponseType: InvalidRequest } res
                => Results.Problem(statusCode: Status400BadRequest, detail: res.Message),

            { ResponseType: Error } res
                => Results.Problem(statusCode: Status500InternalServerError, detail: res.Message),

            _ => Results.Problem(statusCode: Status500InternalServerError, detail: "Unknown error occurred")
        };
    }
}