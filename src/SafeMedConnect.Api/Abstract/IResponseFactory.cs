using MediatR;
using SafeMedConnect.Domain.Models;

namespace SafeMedConnect.Api.Abstract;

public interface IResponseFactory
{
    public Task<IResult> SendAndHandle(IRequest<ApiResponse> request, CancellationToken cnl = default);

    public Task<IResult> SendAndHandle<T>(IRequest<ApiResponse<T>> request, CancellationToken cnl = default) where T : class;
}