using System.Threading;
using ChessLogicSharp.DataStructures;

namespace ChessLogicSharp.ChessPlayers
{
    public class AIChessPlayer : ChessPlayer
    {
        private BoardPieceMove _bestMove;
        private bool _finishedCalculating;
    
        public AIChessPlayer(Board board, Player player) : base(board, player)
        {
            if (board.PlayerTurn == _player)
            {
                ThreadPool.QueueUserWorkItem((state) => CalculateAndMove());
            }
        }

        protected override void BoardOnOnTurnSwapped(Player player)
        {
            if (player == _player)
            {
                ThreadPool.QueueUserWorkItem((state) => CalculateAndMove());
            }
        }

        public override void Update(float deltaTime)
        {
            if (_finishedCalculating)
            {
                // Can only move piece on the unity main thread
                MovePiece(_bestMove);
                _finishedCalculating = false;
            }
        }

        private void CalculateAndMove()
        {
            _bestMove = MinMaxMoveCalc.GetBestMove(_board);
            _finishedCalculating = true;
        }
    }
}