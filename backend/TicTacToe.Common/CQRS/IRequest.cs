namespace TicTacToe.Common.CQRS;

public interface IRequest : IBaseRequest;

public interface IRequest<out TResponse> : IBaseRequest;

public interface IBaseRequest;