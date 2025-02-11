namespace TicTacToe.Common.CQRS;

public delegate Task<TResponse> HandlerDelegate<TResponse>();

public interface IPipelineBehavior<in TRequest, TResponse>
{
    Task<TResponse> Execute(TRequest request, HandlerDelegate<TResponse> next,
        CancellationToken cancellationToken);
}