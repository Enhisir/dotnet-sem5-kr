using System.Collections.Concurrent;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;

namespace TicTacToe.Common.CQRS;


public class ServiceMediator(IServiceProvider serviceProvider) : IMediator
{
    private ConcurrentBag<Type> RegisteredRequestsImpl { get; } = new();

    internal void AddRequestImpl(Type requestTypeType)
    {
        RegisteredRequestsImpl.Add(requestTypeType);
    }

    private IServiceProvider ServiceProvider { get; } = serviceProvider;

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        await using var scope = ServiceProvider.CreateAsyncScope();
        foreach (var requestImpl in RegisteredRequestsImpl)
            if (requestImpl.IsAssignableTo(request.GetType()))
            {
                var handler =
                    scope.ServiceProvider.GetService(
                        typeof(IHandler<,>).MakeGenericType(requestImpl, typeof(TResponse)));
                if (handler == null)
                    continue;
                var func = Expression.Lambda<HandlerDelegate<TResponse>>(Expression.Call(
                    Expression.Constant(handler),
                    handler.GetType().GetMethod("Execute", [requestImpl, typeof(CancellationToken)])!,
                    Expression.Constant(request), Expression.Constant(cancellationToken))).Compile();

                var pipeline = func;
                var pipelines = scope.ServiceProvider.GetServices(
                        typeof(IPipelineBehavior<,>).MakeGenericType(requestImpl, typeof(TResponse)))
                    .ToArray();
                if (pipelines.Length > 0)
                    foreach (var pipe in pipelines.Reverse())
                    {
                        if (pipe == null)
                            continue;
                        pipeline = Expression.Lambda<HandlerDelegate<TResponse>>(Expression.Call(
                            Expression.Constant(pipe),
                            pipe.GetType().GetMethod("Execute",
                                new[]
                                {
                                    requestImpl, typeof(HandlerDelegate<TResponse>), typeof(CancellationToken)
                                })!, Expression.Constant(request), Expression.Constant(pipeline),
                            Expression.Constant(cancellationToken))).Compile();
                    }

                return await pipeline();
            }

        throw new NotSupportedException(
            $"Request handler for type {typeof(IHandler<IRequest<TResponse>, TResponse>)} was not registered.");
    }

    public async Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest
    {
        await using var scope = ServiceProvider.CreateAsyncScope();
        var handler =
            scope.ServiceProvider.GetService(typeof(IHandler<TRequest>)) as
                IHandler<TRequest>;
        if (handler == null)
            throw new NotSupportedException(
                $"Request handler for type {typeof(IHandler<TRequest>)} was not registered.");
        await handler.Execute(request, cancellationToken);
    }

    public async Task<dynamic?> Send(dynamic request, CancellationToken cancellationToken = default)
    {
        await using var scope = ServiceProvider.CreateAsyncScope();
        var handler = scope.ServiceProvider.GetService(typeof(IHandler<,>).MakeGenericType(request.GetType()));
        return handler == null ? null : await handler.Execute(request, cancellationToken);
    }
}