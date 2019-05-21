namespace ChessLogicTests
{
    public static class TestHelpers
    {
        public static char[,] RotateArray(this char[,] board)
        {
            char[,] ret = new char[board.Length, board.Length];

            for (int i = 0; i < 8; ++i) {
                for (int j = 0; j < 8; ++j) {
                    ret[i, j] = board[8 - j - 1, i];
                }
            }

            return ret;
        }
    }
}