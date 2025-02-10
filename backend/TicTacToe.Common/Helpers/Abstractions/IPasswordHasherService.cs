namespace TicTacToe.Common.Helpers.Abstractions;

public interface IPasswordHasherService
{
    public string Hash(string password);
    public bool Validate(string passwordHash, string password);
}