using System.Collections.Generic;
using ChessLogicSharp.ChessPlayers;
using ChessLogicSharp.DataStructures;

namespace ChessLogicSharp
{
    public enum Player
    {
        None,
        PlayerOne,
        PlayerTwo
    }

    public enum GameState
    {
        Playing,
        Ended,
        Paused,
        Resumed,
        WonByCheckmate,
        WonByStaleMate,
    }

    public delegate void PlayerDelegate(Player player);

    public delegate void BoardChangesDelegate(List<BoardChange> changes);

    public delegate bool MoveDelegate(BoardPieceMove move);

    public delegate void BoardGameStateDelegate(GameState state);

    public class Board
    {
        public BoardPiece[,] BoardPieces;
        public Stack<BoardChange> GameChanges;
        public Player PlayerTurn;
        public GameState GameState;

        /// <summary>
        /// Called when a player makes their move and its parameter is the current players go. 
        /// </summary>
        public event PlayerDelegate OnTurnSwapped;

        /// <summary>
        /// Called when a player is in checkmate and its parameter is the player in check.
        /// </summary>
        public event PlayerDelegate OnPlayerInCheck;

        /// <summary>
        /// Called when a something on the board has changed and its parameter is a list of changes.
        /// </summary>
        public event BoardChangesDelegate OnBoardChanged;

        /// <summary>
        /// Called when the state of the game changes, such as when a game is paused, resumed or ended.
        /// </summary>
        public event BoardGameStateDelegate OnGameStateChanged;

        public const int BOARD_DIMENSIONS = 8;

        private readonly List<ChessPlayer> _players = new List<ChessPlayer>();

        public void ChangeGameState(GameState gameState)
        {
            OnGameStateChanged?.Invoke(gameState);

            // Send the event for resumed but then set the actual state back to playing to make it less confusing
            if (gameState == GameState.Resumed) 
            {
                gameState = GameState.Playing;
            }

            GameState = gameState;
        }

        #region Players

        /// <summary>
        /// Adds an instance of a player to the board and subscribe to its on move made event.
        /// </summary>
        /// <param name="player"></param>
        public void AddPlayer(ChessPlayer player)
        {
            _players.Add(player);
            player.OnMoveMade += PlayerOnOnMoveMade;
        }

        /// <summary>
        /// Remove the players from the board and unsubscribe from when the make moves, removing their ability to make moves.
        /// </summary>
        public void ClearPlayers()
        {
            for (int i = 0; i < _players.Count; i++)
            {
                _players[i].OnMoveMade -= PlayerOnOnMoveMade;
            }

            _players.Clear();
        }
        
        /// <summary>
        /// Called when a player makes a move and returns if the move was valid or not.
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        private bool PlayerOnOnMoveMade(BoardPieceMove move)
        {
            return ApplyMove(move);
        }

        /// <summary>
        /// Updates the chess players.
        /// </summary>
        /// <param name="deltaTime"></param>
        public void UpdatePlayers(float deltaTime)
        {
            if (_players == null || GameState != GameState.Playing)
                return;

            for (int i = 0; i < _players.Count; i++)
            {
                _players[i].Update(deltaTime);
            }
        }

        #endregion
        
        #region Apply Moves

        private bool ApplyMove(BoardPieceMove move)
        {
            // Check the moving piece belongs to the correct player and is a valid move
            if (BoardPieces[move.From.X, move.From.Y].PieceOwner != PlayerTurn ||
                !ValidMovesCalc.IsMoveValid(move, PlayerTurn, this) ||
                (GameState != GameState.Playing))
            {
                // Move is invalid
                return false;
            }

            List<BoardChange> boardChanges = ApplyMoveToBoard(move);
            CheckPawnPromotion(move, boardChanges);

            // Store changes
            for (int i = 0; i < boardChanges.Count; i++)
            {
                GameChanges.Push(boardChanges[i]);
            }

            OnBoardChanged?.Invoke(boardChanges);

            if (ValidMovesCalc.IsPlayerInCheck(BoardPieces, BoardHelpers.GetOpponentPlayer(PlayerTurn)))
            {
                if (ValidMovesCalc.PlayerCanMove(this, BoardHelpers.GetOpponentPlayer(PlayerTurn)))
                {
                    // CHECKMATE
                    ChangeGameState(GameState.WonByCheckmate);
                    return true;
                }

                // IN CHECK
                OnPlayerInCheck?.Invoke(BoardHelpers.GetOpponentPlayer(PlayerTurn));
            }
            else
            {
                if (ValidMovesCalc.PlayerCanMove(this, BoardHelpers.GetOpponentPlayer(PlayerTurn)))
                {
                    // STALEMATE
                    ChangeGameState(GameState.WonByStaleMate);
                    return true;
                }
            }

            ClearEnPassant();
            SwapPlayerTurns();

            return true;
        }
        
        private void SwapPlayerTurns()
        {
            PlayerTurn = PlayerTurn == Player.PlayerOne ? Player.PlayerTwo : Player.PlayerOne;
            OnTurnSwapped?.Invoke(PlayerTurn);
        }

        private void ClearEnPassant()
        {
            for (int x = 0; x < BOARD_DIMENSIONS; x++)
            {
                for (int y = 0; y < BOARD_DIMENSIONS; y++)
                {
                    // Clear opponents en'Passant, not ours. Ours needs to be available for the opponents turn.
                    if (BoardPieces[x, y].PieceType == PieceType.Pawn &&
                        BoardPieces[x, y].PieceOwner != PlayerTurn)
                    {
                        BoardPieces[x, y].CanEnPassant = false;
                    }
                }
            }
        }

        private void CheckPawnPromotion(BoardPieceMove move, List<BoardChange> changes)
        {
            //Check if we need to promote a pawn.
            if (BoardPieces[move.To.X, move.To.Y].PieceType == PieceType.Pawn &&
                (move.To.Y == 0 || move.To.Y == 7))
            {
                // Time to promote.
                changes.Add(new PromotePawnChange
                {
                    Type = BoardChangeType.PromotePawn,
                    NewPieceType = PieceType.Queen,
                    Player = PlayerTurn,
                    PawnPosition = move.To
                });
                BoardPieces[move.To.X, move.To.Y].PieceType = PieceType.Queen;
            }
        }

        private List<BoardChange> ApplyMoveToBoard(BoardPieceMove move)
        {
            List<BoardChange> boardChanges = new List<BoardChange>();
            //If this was an en'passant move the taken piece will not be in the square we moved to.
            if (BoardPieces[move.From.X, move.From.Y].PieceType == PieceType.Pawn)
            {
                //If the pawn is on its start position and it double jumps, then en'passant may be available for opponent.
                if ((move.From.Y == 1 && move.To.Y == 3) ||
                    (move.From.Y == 6 && move.To.Y == 4))
                {
                    BoardPieces[move.From.X, move.From.Y].CanEnPassant = true;
                }
            }

            //En'Passant removal of enemy pawn.
            //If our pawn moved into an empty position to the left or right, then must be En'Passant.
            if (BoardPieces[move.From.X, move.From.Y].PieceType == PieceType.Pawn &&
                BoardPieces[move.To.X, move.To.Y].PieceType == PieceType.None &&
                ((move.From.X < move.To.X) || (move.From.X > move.To.X)))
            {
                boardChanges.Add(EnPassantMove(move));
            }
            // Special king moves including castling
            else if (BoardPieces[move.From.X, move.From.Y].PieceType == PieceType.King)
            {
                boardChanges.Add(KingMove(move));
            }
            // Standard move or take
            else
            {
                boardChanges.Add(StandardMove(move));
            }

            return boardChanges;
        }

        private static void StandardMoveBoardChange(BoardPiece[,] board, BoardPieceMove move)
        {
            board[move.From.X, move.From.Y].HasMoved = true;
            board[move.To.X, move.To.Y] = board[move.From.X, move.From.Y];
            board[move.From.X, move.From.Y] = new BoardPiece();
        }

        private BoardChange StandardMove(BoardPieceMove move)
        {
            BoardChange change;

            //Move the piece into new position.
            BoardChangeType changeType = BoardPieces[move.To.X, move.To.Y].PieceType != PieceType.None
                ? BoardChangeType.TakePiece
                : BoardChangeType.MovePiece;
            if (changeType == BoardChangeType.MovePiece)
            {
                // Piece movement
                change = new MovePieceChange
                {
                    Type = changeType,
                    Move = move,
                    Player = PlayerTurn,
                    MovedPieceType = BoardPieces[move.From.X, move.From.Y].PieceType
                };
            }
            else
            {
                // Piece take
                change = new TakePieceChange
                {
                    Type = changeType,
                    Move = move,
                    Player = PlayerTurn,
                    TakingPieceType = BoardPieces[move.From.X, move.From.Y].PieceType,
                    TakenPieceType = BoardPieces[move.To.X, move.To.Y].PieceType
                };
            }

            StandardMoveBoardChange(BoardPieces, move);

            return change;
        }

        private BoardChange KingMove(BoardPieceMove move)
        {
            BoardChange change;
            //Are we moving 2 spaces??? This indicates CASTLING.
            if (move.To.X - move.From.X == 2)
            {
                //Moving 2 spaces to the right - Move the ROOK on the right into its new position.
                BoardPieces[move.From.X + 3, move.From.Y].HasMoved = true;
                BoardPieces[move.From.X + 1, move.From.Y] = BoardPieces[move.From.X + 3, move.From.Y];
                BoardPieces[move.From.X + 3, move.From.Y] = new BoardPiece();

                // Castling
                change = (new CastlingChange
                {
                    Type = BoardChangeType.Castling,
                    CastleMove = new BoardPieceMove(new Vector2I(move.From.X + 3, move.From.Y),
                        new Vector2I(move.From.X + 1, move.From.Y)),
                    KingMove = move,
                    Player = PlayerTurn
                });

                // Move the king
                StandardMoveBoardChange(BoardPieces, move);
            }
            else if (move.To.X - move.From.X == -2)
            {
                //Moving 2 spaces to the left - Move the ROOK on the left into its new position.
                //Move the piece into new position.
                BoardPieces[move.From.X - 4, move.From.Y].HasMoved = true;
                BoardPieces[move.From.X - 1, move.From.Y] = BoardPieces[move.From.X - 4, move.From.Y];
                BoardPieces[move.From.X - 4, move.From.Y] = new BoardPiece();

                // Castling
                change = (new CastlingChange
                {
                    Type = BoardChangeType.Castling,
                    CastleMove = new BoardPieceMove(new Vector2I(move.From.X - 4, move.From.Y),
                        new Vector2I(move.From.X - 1, move.From.Y)),
                    KingMove = move,
                    Player = PlayerTurn
                });

                // Move the king
                StandardMoveBoardChange(BoardPieces, move);
            }
            else
            {
                // No castling, standard move
                change = StandardMove(move);
            }

            return change;
        }

        private BoardChange EnPassantMove(BoardPieceMove move)
        {
            int pawnDirectionOpposite = BoardHelpers.GetPlayerDirection(BoardHelpers.GetOpponentPlayer(PlayerTurn));
            BoardChange change = (new EnPassantTakePieceChange
            {
                Type = BoardChangeType.EnPassantTakePiece,
                Move = move,
                Player = PlayerTurn,
                TakingPieceType = BoardPieces[move.From.X, move.From.Y].PieceType,
                TakenPawnPosition = new Vector2I(move.To.X, move.To.Y + pawnDirectionOpposite)
            });

            // Move piece
            StandardMoveBoardChange(BoardPieces, move);

            // Remove pawn
            BoardPieces[move.To.X, move.To.Y + pawnDirectionOpposite] = new BoardPiece();

            return change;
        }

        #endregion
    }
}