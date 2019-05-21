using ChessLogicSharp.DataStructures;

namespace ChessLogicSharp.ChessPlayers
{
    public abstract class ChessPlayer
    {
        protected readonly Player _player;
        protected readonly Board _board;

        protected ChessPlayer(Board board, Player player)
        {
            _board = board;
            _player = player;
            
            board.OnTurnSwapped += BoardOnOnTurnSwapped;
        }

        protected abstract void BoardOnOnTurnSwapped(Player player);

        public virtual void Update(float deltaTime)
        {
            
        }

        protected void MovePiece(BoardPieceMove boardPieceMove)
        {
            // Make sure it is our turn
            if (_board.PlayerTurn != _player)
            {
                return;
            }
            
            // Apply move to the core data structure
            _board.ApplyMove(boardPieceMove);
        }
    }
}