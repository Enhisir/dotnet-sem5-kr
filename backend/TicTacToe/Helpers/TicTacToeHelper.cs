using TicTatToe.Data.Enum;

namespace TicTacToe.Helpers;

public class TicTacToeHelper
{
    // Сжимает поле 3x3 в битовую маску (18 бит)
    public static int Compress(int[][] board)
    {
        int mask = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                // Каждая ячейка занимает 2 бита
                mask |= (board[i][j] & 0x03) << (2 * (i * 3 + j));
            }
        }
        return mask;
    }


    public static int[][] Decompress(int mask)
    {
        int[][] board = [
            new int[3],
            new int[3],
            new int[3]
        ];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                // Извлекаем значение ячейки (2 бита)
                board[i][j] = (mask >> (2 * (i * 3 + j))) & 0x03;
            }
        }
        return board;
    }
    
    public static Sign CheckWin(int[][] board)
    {
        for (int i = 0; i < 3; i++)
        {
            if (board[i][0] != (int)Sign.Empty && board[i][0] == board[i][1] && board[i][1] == board[i][2])
                return (Sign)board[i][0]; // Победа в строке
            if (board[0][i] != (int)Sign.Empty && board[0][i] == board[1][i] && board[1][i] == board[2][i])
                return (Sign)board[0][i]; // Победа в столбце
        }
        
        if (board[0][0] != (int)Sign.Empty && board[0][0] == board[1][1] && board[1][1] == board[2][2])
            return (Sign)board[0][0];
        if (board[0][2] != (int)Sign.Empty && board[0][2] == board[1][1] && board[1][1] == board[2][0])
            return (Sign)board[0][2];

        return Sign.Empty;
    }
    
    public static bool IsGameOver(int[][] board)
    {
        for (var i = 0; i < 3; i++)
            if (!IsLineBlocked(board[i][0], board[i][1], board[i][2])
                || !IsLineBlocked(board[0][i], board[1][i], board[2][i]))
                return false;
        
        return IsLineBlocked(board[0][0], board[1][1], board[2][2])
               && IsLineBlocked(board[0][2], board[1][1], board[2][0]);
        // Если все линии заблокированы, игра окончена
    }

    // Проверяет, заблокирована ли линия (есть ли два разных знака)
    private static bool IsLineBlocked(int a, int b, int c)
    {
        const int empty = (int)Sign.Empty;
        const int x = (int)Sign.X;
        const int o = (int)Sign.O;
        if (a == (int)Sign.Empty && b == empty && c == empty)
            return false;

        // Если есть два разных знака, линия заблокирована
        if ((a == x && b == o) || (a == x && c == o) || (b == x && c == o))
            return true;
        
        return (a == o && b == x) || (a == o && c == x) || (b == o && c == x);
    }
}