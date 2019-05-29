using System.Threading;
using ChessLogicSharp.DataStructures;

namespace ChessLogicSharp.ChessPlayers
{
    public class AIChessPlayer : ChessPlayer
    {
        private BoardPieceMove _bestMove;
        private bool _finishedCalculating;

        private readonly MinMaxMoveCalc _moveCalc;
    
        public AIChessPlayer(Board board, Player player, int searchDepth) : base(board, player)
        {
            _moveCalc = new MinMaxMoveCalc(searchDepth);
            if (board.PlayerTurn == _player)
            {
                ThreadPool.QueueUserWorkItem((state) => CalculateAndMove());
            }
        }

        protected override void OnTurnSwapped(Player player)
        {
            if (player == _player)
            {
                ThreadPool.QueueUserWorkItem((state) => CalculateAndMove());
            }
        }

        public override void Update(float deltaTime)
        {
            if (_finishedCalculating && _board.CanMove)
            {
                // Can only move piece on the unity main thread
                MovePiece(_bestMove);
                _finishedCalculating = false;
            }
        }

        private void CalculateAndMove()
        {
            _bestMove = _moveCalc.GetBestMove(_board);
            _finishedCalculating = true;
        }
    }
}