using System.Threading;
using ChessLogicSharp.DataStructures;

namespace ChessLogicSharp.ChessPlayers
{
    /// <summary>
    /// A example of an AI chess player. When it's its turn it creates a new thread and calculates the best move and then makes the move.
    /// </summary>
    public class AIChessPlayer : ChessPlayer
    {
        private readonly MinMaxMoveCalc _moveCalc;
    
        public AIChessPlayer(Board board, Player player, int searchDepth) : base(board, player)
        {
            _moveCalc = new MinMaxMoveCalc(searchDepth);
            
            if (board.PlayerTurn == Player)
            {
                ThreadPool.QueueUserWorkItem((state) => CalculateAndMove());
            }
        }

        protected override void OnTurnSwapped(Player player)
        {
            if (player == Player)
            {
                ThreadPool.QueueUserWorkItem((state) => CalculateAndMove());
            }
        }
        
        private void CalculateAndMove()
        {
            MovePiece(_moveCalc.GetBestMove(Board));
        }
    }
}