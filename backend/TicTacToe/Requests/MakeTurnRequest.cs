using TicTacToe.Common.CQRS;

namespace TicTacToe.Requests;

public class MakeTurnRequest(string UserName, int x, int y) : IRequest<IResult>;