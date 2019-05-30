using ChessLogicSharp.DataStructures;

namespace ChessLogicSharp.ChessPlayers
{
    /// <summary>
    /// The base class for all ChessPlayers. ChessPlayers have various optional virtual functions that they can override.
    /// To make a move, the player needs to be added to the Board and the MovePiece function called.
    /// </summary>
    public abstract class ChessPlayer
    {
        public event MoveDelegate OnMoveMade;
        
        protected readonly Player Player;
        protected readonly Board Board;

        protected ChessPlayer(Board board, Player player)
        {
            Board = board;
            Player = player;
            
            board.OnTurnSwapped += OnTurnSwapped;
            board.OnGameStateChanged += GameStateChanged;
        }

        private void GameStateChanged(GameState state)
        {
            if (state == GameState.Ended)
            {
                Board.OnTurnSwapped -= OnTurnSwapped;
                Board.OnGameStateChanged -= GameStateChanged;
            }
            
            OnGameStateChanged(state);
        }
        
        public virtual void Update(float deltaTime)
        {
            
        }
        
        protected virtual void OnGameStateChanged(GameState state)
        {

        }

        protected virtual void OnTurnSwapped(Player player)
        {
            
        }

        /// <summary>
        /// Lets the board know that this player has made a move.
        /// </summary>
        /// <param name="boardPieceMove"></param>
        /// <returns></returns>
        protected bool MovePiece(BoardPieceMove boardPieceMove)
        {
            // Make sure it is our turn
            if (Board.PlayerTurn != Player)
            {
                return false;
            }
            
            // Apply move to the core data structure
            return OnMoveMade != null && OnMoveMade.Invoke(boardPieceMove);
        }
    }
}