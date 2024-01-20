using MediatR;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Api.Interfaces;

public interface IResponseHandler
{
    public Task<IResult> SendAndHandle(IRequest<ResponseWrapper> request);

    public Task<IResult> SendAndHandle<T>(IRequest<ResponseWrapper<T>> request) where T : class;
}