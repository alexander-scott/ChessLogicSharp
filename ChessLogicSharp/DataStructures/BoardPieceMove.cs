namespace ChessLogicSharp.DataStructures
{
    public struct BoardPieceMove
    {
        public Vector2I From;
        public Vector2I To;

        public BoardPieceMove(int fromX, int fromY, int toX, int toY)
        {
            From = new Vector2I(fromX, fromY);
            To = new Vector2I(toX, toY);
        }

        public BoardPieceMove(Vector2I from, Vector2I to)
        {
            From = from;
            To = to;
        }
    }

    public class BoardPieceMoveScore
    {
        public BoardPieceMove Move;
        public float Score;

        public BoardPieceMoveScore(BoardPieceMove move, float score)
        {
            Move = move;
            Score = score;
        }
    }
}
