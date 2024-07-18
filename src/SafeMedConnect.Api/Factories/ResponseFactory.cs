using MediatR;
using SafeMedConnect.Api.Abstract;
using SafeMedConnect.Domain.Models;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static SafeMedConnect.Domain.Enums.ApiResponseTypes;

namespace SafeMedConnect.Api.Factories;

internal sealed class ResponseFactory(ISender mediator) : IResponseFactory
{
    public async Task<IResult> SendAndHandle(IRequest<ApiResponse> request, CancellationToken cnl = default)
    {
        return await mediator.Send(request, cnl) switch
        {
            { ApiResponseType: Success }
                => Results.NoContent(),

            { ApiResponseType: NotFound } res
                => Results.Problem(statusCode: Status404NotFound, detail: res.Message),

            { ApiResponseType: Conflict } res
                => Results.Problem(statusCode: Status409Conflict, detail: res.Message),

            { ApiResponseType: Forbidden } res =>
                Results.Problem(statusCode: Status403Forbidden, detail: res.Message),

            { ApiResponseType: InvalidRequest } res
                => Results.Problem(statusCode: Status400BadRequest, detail: res.Message),

            { ApiResponseType: Error } res
                => Results.Problem(statusCode: Status500InternalServerError, detail: res.Message),

            _ => Results.Problem(statusCode: Status500InternalServerError, detail: "Unknown error occurred")
        };
    }

    public async Task<IResult> SendAndHandle<T>(IRequest<ApiResponse<T>> request, CancellationToken cnl) where T : class
    {
        return await mediator.Send(request, cnl) switch
        {
            { ApiResponseType: Success } res
                => Results.Ok(res.Data),

            { ApiResponseType: NotFound } res
                => Results.Problem(statusCode: Status404NotFound, detail: res.Message),

            { ApiResponseType: Conflict } res
                => Results.Problem(statusCode: Status409Conflict, detail: res.Message),

            { ApiResponseType: Forbidden } res =>
                Results.Problem(statusCode: Status403Forbidden, detail: res.Message),

            { ApiResponseType: InvalidRequest } res
                => Results.Problem(statusCode: Status400BadRequest, detail: res.Message),

            { ApiResponseType: Error } res
                => Results.Problem(statusCode: Status500InternalServerError, detail: res.Message),

            { ApiResponseType: Unauthorized } res
                => Results.Problem(statusCode: Status401Unauthorized, detail: res.Message),

            _ => Results.Problem(statusCode: Status500InternalServerError, detail: "Unknown error occurred")
        };
    }
}