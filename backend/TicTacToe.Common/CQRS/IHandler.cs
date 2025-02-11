namespace TicTacToe.Common.CQRS;


public interface IHandler<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> Execute(TRequest request, CancellationToken cancellationToken);
}

public interface IHandler<in TRequest>
    where TRequest : IRequest
{
    Task Execute(TRequest request, CancellationToken cancellationToken);
}