using System.Collections.Generic;
using ChessLogicSharp.DataStructures;

namespace ChessLogicSharp
{
    public enum Player
    {
        None,
        PlayerOne,
        PlayerTwo
    }

    public delegate void PlayerDelegate(Player player);
    public delegate void BoardActionsDelegate(List<BoardAction> actions);

    public class Board
    {
        public BoardPiece[,] BoardPieces;
        public Stack<BoardAction> Actions;
        public Player PlayerTurn;
        public bool GameOver;

        public event PlayerDelegate OnTurnSwapped;
        public event PlayerDelegate OnPlayerCheckmate;
        public event PlayerDelegate OnPlayerStalemate;
        public event PlayerDelegate OnPlayerInCheck;
        public event BoardActionsDelegate OnBoardChanged;

        public const int BOARD_DIMENSIONS = 8;

        public void ApplyMove(BoardPieceMove move)
        {
            // Check the moving piece belongs to the correct player and is a valid move
            if (BoardPieces[move.From.x, move.From.y].PieceOwner != PlayerTurn ||
                !ValidMovesCalc.IsMoveValid(move, PlayerTurn, this) || GameOver)
            {
                // Move is invalid
                return;
            }

            List<BoardAction> boardChanges = ApplyMoveToBoard(move);
            CheckPawnPromotion(move, boardChanges);

            // Store actions
            for (int i = 0; i < boardChanges.Count; i++)
            {
                Actions.Push(boardChanges[i]);
            }

            OnBoardChanged?.Invoke(boardChanges);

            if (ValidMovesCalc.IsPlayerInCheck(BoardPieces, BoardHelpers.GetOpponentPlayer(PlayerTurn)))
            {
                if (ValidMovesCalc.PlayerCanMove(this, BoardHelpers.GetOpponentPlayer(PlayerTurn)))
                {
                    // CHECKMATE
                    OnPlayerCheckmate?.Invoke(PlayerTurn);
                    GameOver = true;
                    return;
                }
                else
                {
                    // IN CHECK
                    OnPlayerInCheck?.Invoke(BoardHelpers.GetOpponentPlayer(PlayerTurn));
                }
            }
            else
            {
                if (ValidMovesCalc.PlayerCanMove(this, BoardHelpers.GetOpponentPlayer(PlayerTurn)))
                {
                    // STALEMATE
                    OnPlayerStalemate?.Invoke(PlayerTurn);
                    GameOver = true;
                    return;
                }
            }

            ClearEnPassant();
            SwapPlayerTurns();
        }

        private void CheckPawnPromotion(BoardPieceMove move, List<BoardAction> actions)
        {
            //Check if we need to promote a pawn.
            if (BoardPieces[move.To.x, move.To.y].PieceType == PieceType.Pawn &&
                (move.To.y == 0 || move.To.y == 7))
            {
                // Time to promote.
                actions.Add(new PromotePawnAction
                {
                    Type = BoardActionType.PromotePawn,
                    NewPieceType = PieceType.Queen,
                    Player = PlayerTurn,
                    PawnPosition = move.To
                });
                BoardPieces[move.To.x, move.To.y].PieceType = PieceType.Queen;
            }
        }

        private List<BoardAction> ApplyMoveToBoard(BoardPieceMove move)
        {
            List<BoardAction> boardChanges = new List<BoardAction>();
            //If this was an en'passant move the taken piece will not be in the square we moved to.
            if (BoardPieces[move.From.x, move.From.y].PieceType == PieceType.Pawn)
            {
                //If the pawn is on its start position and it double jumps, then en'passant may be available for opponent.
                if ((move.From.y == 1 && move.To.y == 3) ||
                    (move.From.y == 6 && move.To.y == 4))
                {
                    BoardPieces[move.From.x, move.From.y].CanEnPassant = true;
                }
            }

            //En'Passant removal of enemy pawn.
            //If our pawn moved into an empty position to the left or right, then must be En'Passant.
            if (BoardPieces[move.From.x, move.From.y].PieceType == PieceType.Pawn &&
                BoardPieces[move.To.x, move.To.y].PieceType == PieceType.None &&
                ((move.From.x < move.To.x) || (move.From.x > move.To.x)))
            {
                boardChanges.Add(EnPassantMove(move));
            }
            // Special king moves including castling
            else if (BoardPieces[move.From.x, move.From.y].PieceType == PieceType.King)
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
            board[move.From.x, move.From.y].HasMoved = true;
            board[move.To.x, move.To.y] = board[move.From.x, move.From.y];
            board[move.From.x, move.From.y] = new BoardPiece();
        }

        private BoardAction StandardMove(BoardPieceMove move)
        {
            BoardAction action;

            //Move the piece into new position.
            BoardActionType actionType = BoardPieces[move.To.x, move.To.y].PieceType != PieceType.None
                ? BoardActionType.TakePiece
                : BoardActionType.MovePiece;
            if (actionType == BoardActionType.MovePiece)
            {
                // Piece movement
                action = new MovePieceAction
                {
                    Type = actionType,
                    Move = move,
                    Player = PlayerTurn,
                    MovedPieceType = BoardPieces[move.From.x, move.From.y].PieceType
                };
            }
            else
            {
                // Piece take
                action = new TakePieceAction
                {
                    Type = actionType,
                    Move = move,
                    Player = PlayerTurn,
                    TakingPieceType = BoardPieces[move.From.x, move.From.y].PieceType,
                    TakenPieceType = BoardPieces[move.To.x, move.To.y].PieceType
                };
            }

            StandardMoveBoardChange(BoardPieces, move);

            return action;
        }

        private BoardAction KingMove(BoardPieceMove move)
        {
            BoardAction action;
            //Are we moving 2 spaces??? This indicates CASTLING.
            if (move.To.x - move.From.x == 2)
            {
                //Moving 2 spaces to the right - Move the ROOK on the right into its new position.
                BoardPieces[move.From.x + 3, move.From.y].HasMoved = true;
                BoardPieces[move.From.x + 1, move.From.y] = BoardPieces[move.From.x + 3, move.From.y];
                BoardPieces[move.From.x + 3, move.From.y] = new BoardPiece();

                // Castling
                action = (new CastlingAction
                {
                    Type = BoardActionType.Castling,
                    CastleMove = new BoardPieceMove(new Vector2I(move.From.x + 3, move.From.y),
                        new Vector2I(move.From.x + 1, move.From.y)),
                    KingMove = move,
                    Player = PlayerTurn
                });

                // Move the king
                StandardMoveBoardChange(BoardPieces, move);
            }
            else if (move.To.x - move.From.x == -2)
            {
                //Moving 2 spaces to the left - Move the ROOK on the left into its new position.
                //Move the piece into new position.
                BoardPieces[move.From.x - 4, move.From.y].HasMoved = true;
                BoardPieces[move.From.x - 1, move.From.y] = BoardPieces[move.From.x - 4, move.From.y];
                BoardPieces[move.From.x - 4, move.From.y] = new BoardPiece();

                // Castling
                action = (new CastlingAction
                {
                    Type = BoardActionType.Castling,
                    CastleMove = new BoardPieceMove(new Vector2I(move.From.x - 4, move.From.y),
                        new Vector2I(move.From.x - 1, move.From.y)),
                    KingMove = move,
                    Player = PlayerTurn
                });

                // Move the king
                StandardMoveBoardChange(BoardPieces, move);
            }
            else
            {
                // No castling, standard move
                action = StandardMove(move);
            }

            return action;
        }

        private BoardAction EnPassantMove(BoardPieceMove move)
        {
            int pawnDirectionOpposite = BoardHelpers.GetPlayerDirection(BoardHelpers.GetOpponentPlayer(PlayerTurn));
            BoardAction action = (new EnPassantTakePieceAction
            {
                Type = BoardActionType.EnPassantTakePiece,
                Move = move,
                Player = PlayerTurn,
                TakingPieceType = BoardPieces[move.From.x, move.From.y].PieceType,
                TakenPawnPosition = new Vector2I(move.To.x, move.To.y + pawnDirectionOpposite)
            });

            // Move piece
            StandardMoveBoardChange(BoardPieces, move);

            // Remove pawn
            BoardPieces[move.To.x, move.To.y + pawnDirectionOpposite] = new BoardPiece();

            return action;
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
    }
}