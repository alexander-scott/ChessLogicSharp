using ChessLogicSharp.DataStructures;

namespace ChessLogicSharp.ChessPlayers
{
    public class BasicPlayer : ChessPlayer
    {
        public BasicPlayer(Board board, Player player) : base(board, player)
        {
        }

        protected override void OnTurnSwapped(Player player)
        {
        }

        public bool ApplyMove(BoardPieceMove move)
        {
            return MovePiece(move);
        }
        
        public void ApplyMove(int fromX, int fromY, int toX, int toY)
        {
            MovePiece(new BoardPieceMove(fromX, fromY, toX, toY));
        }
    }
}