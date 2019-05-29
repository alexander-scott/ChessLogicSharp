using System;
using System.Collections.Generic;
using ChessLogicSharp.DataStructures;

namespace ChessLogicSharp
{
    public class MinMaxMoveCalc
    {
        private readonly int _searchDepth;

        private const int PAWN_SCORE = 20;
        private const int KNIGHT_SCORE = 80;
        private const int BISHOP_SCORE = 100;
        private const int CASTLE_SCORE = 200;
        private const int QUEEN_SCORE = 300;
        private const int KING_SCORE = 3000;

        private const int CHECK_SCORE = 1;
        private const int CHECKMATE_SCORE = 5;
        private const int STALEMATE_SCORE = 1; //Tricky one because sometimes you want this, sometimes you don't.

        private const int PIECE_WEIGHT = 1; //Scores as above.
        private const int MOVE_WEIGHT = 1; //Number of moves available to pieces.
        private const int POSITIONAL_WEIGHT = 1; //Whether in CHECK, CHECKMATE or STALEMATE.

        #region Score tables

        private static readonly float[,] PawnScoreTableAI = {
            {0, 0, 0, 0, 0, 0, 0, 0,},
            {50, 50, 50, 50, 50, 50, 50, 50,},
            {10, 10, 20, 30, 30, 20, 10, 10,},
            {5, 5, 10, 25, 25, 10, 5, 5,},
            {0, 0, 0, 20, 20, 0, 0, 0,},
            {5, -5, -10, 0, 0, -10, -5, 5,},
            {5, 10, 10, -20, -20, 10, 10, 5,},
            {0, 0, 0, 0, 0, 0, 0, 0}
        };

        private static readonly float[,] KnightScoreTableAI = {
            {-50, -40, -30, -30, -30, -30, -40, -50,},
            {-40, -20, 0, 0, 0, 0, -20, -40,},
            {-30, 0, 10, 15, 15, 10, 0, -30,},
            {-30, 5, 15, 20, 20, 15, 5, -30,},
            {-30, 0, 15, 20, 20, 15, 0, -30,},
            {-30, 5, 10, 15, 15, 10, 5, -30,},
            {-40, -20, 0, 5, 5, 0, -20, -40,},
            {-50, -40, -30, -30, -30, -30, -40, -50,}
        };

        private static readonly float[,] BishopScoreTableAI = {
            {-20, -10, -10, -10, -10, -10, -10, -20,},
            {-10, 0, 0, 0, 0, 0, 0, -10,},
            {-10, 0, 5, 10, 10, 5, 0, -10,},
            {-10, 5, 5, 10, 10, 5, 5, -10,},
            {-10, 0, 10, 10, 10, 10, 0, -10,},
            {-10, 10, 10, 10, 10, 10, 10, -10,},
            {-10, 5, 0, 0, 0, 0, 5, -10,},
            {-20, -10, -10, -10, -10, -10, -10, -20,}
        };

        private static readonly float[,] CastleScoreTableAI = {
            {0, 0, 0, 0, 0, 0, 0, 0,},
            {5, 10, 10, 10, 10, 10, 10, 5,},
            {-5, 0, 0, 0, 0, 0, 0, -5,},
            {-5, 0, 0, 0, 0, 0, 0, -5,},
            {-5, 0, 0, 0, 0, 0, 0, -5,},
            {-5, 0, 0, 0, 0, 0, 0, -5,},
            {-5, 0, 0, 0, 0, 0, 0, -5,},
            {0, 0, 0, 5, 5, 0, 0, 0}
        };

        private static readonly float[,] QueenScoreTableAI = {
            {-20, -10, -10, -5, -5, -10, -10, -20,},
            {-10, 0, 0, 0, 0, 0, 0, -10,},
            {-10, 0, 5, 5, 5, 5, 0, -10,},
            {-5, 0, 5, 5, 5, 5, 0, -5,},
            {0, 0, 5, 5, 5, 5, 0, -5,},
            {-10, 5, 5, 5, 5, 5, 0, -10,},
            {-10, 0, 5, 0, 0, 0, 0, -10,},
            {-20, -10, -10, -5, -5, -10, -10, -20}
        };

        private static readonly float[,] KingMiddleGameScoreTableAI = {
            {-30, -40, -40, -50, -50, -40, -40, -30,},
            {-30, -40, -40, -50, -50, -40, -40, -30,},
            {-30, -40, -40, -50, -50, -40, -40, -30,},
            {-30, -40, -40, -50, -50, -40, -40, -30,},
            {-20, -30, -30, -40, -40, -30, -30, -20,},
            {-10, -20, -20, -20, -20, -20, -20, -10,},
            {20, 20, 0, 0, 0, 0, 20, 20,},
            {20, 30, 10, 0, 0, 10, 30, 20}
        };

        private static readonly float[,] KingEndGameScoreTablePlayer = {
            {-50, -40, -30, -20, -20, -30, -40, -50,},
            {-30, -20, -10, 0, 0, -10, -20, -30,},
            {-30, -10, 20, 30, 30, 20, -10, -30,},
            {-30, -10, 30, 40, 40, 30, -10, -30,},
            {-30, -10, 30, 40, 40, 30, -10, -30,},
            {-30, -10, 20, 30, 30, 20, -10, -30,},
            {-30, -30, 0, 0, 0, 0, -30, -30,},
            {-50, -30, -30, -30, -30, -30, -30, -50}
        };

        private static readonly float[,] PawnScoreTablePlayer = {
            {0, 0, 0, 0, 0, 0, 0, 0},
            {5, 10, 10, -20, -20, 10, 10, 5,},
            {5, -5, -10, 0, 0, -10, -5, 5,},
            {0, 0, 0, 20, 20, 0, 0, 0,},
            {5, 5, 10, 25, 25, 10, 5, 5,},
            {10, 10, 20, 30, 30, 20, 10, 10,},
            {50, 50, 50, 50, 50, 50, 50, 50,},
            {0, 0, 0, 0, 0, 0, 0, 0,}
        };

        private static readonly float[,] KnightScoreTablePlayer = {
            {-50, -40, -30, -30, -30, -30, -40, -50,},
            {-40, -20, 0, 5, 5, 0, -20, -40,},
            {-30, 5, 10, 15, 15, 10, 5, -30,},
            {-30, 0, 15, 20, 20, 15, 0, -30,},
            {-30, 5, 15, 20, 20, 15, 5, -30,},
            {-30, 0, 10, 15, 15, 10, 0, -30,},
            {-40, -20, 0, 0, 0, 0, -20, -40,},
            {-50, -40, -30, -30, -30, -30, -40, -50,}
        };

        private static readonly float[,] BishopScoreTablePlayer = {
            {-20, -10, -10, -10, -10, -10, -10, -20,},
            {-10, 5, 0, 0, 0, 0, 5, -10,},
            {-10, 10, 10, 10, 10, 10, 10, -10,},
            {-10, 0, 10, 10, 10, 10, 0, -10,},
            {-10, 5, 5, 10, 10, 5, 5, -10,},
            {-10, 0, 5, 10, 10, 5, 0, -10,},
            {-10, 0, 0, 0, 0, 0, 0, -10,},
            {-20, -10, -10, -10, -10, -10, -10, -20,}
        };

        private static readonly float[,] CastleScoreTablePlayer = {
            {0, 0, 0, 5, 5, 0, 0, 0},
            {-5, 0, 0, 0, 0, 0, 0, -5,},
            {-5, 0, 0, 0, 0, 0, 0, -5,},
            {-5, 0, 0, 0, 0, 0, 0, -5,},
            {-5, 0, 0, 0, 0, 0, 0, -5,},
            {-5, 0, 0, 0, 0, 0, 0, -5,},
            {5, 10, 10, 10, 10, 10, 10, 5,},
            {0, 0, 0, 0, 0, 0, 0, 0,}
        };

        private static readonly float[,] QueenScoreTablePlayer = {
            {-20, -10, -10, -5, -5, -10, -10, -20},
            {-10, 0, 5, 0, 0, 0, 0, -10,},
            {-10, 5, 5, 5, 5, 5, 0, -10,},
            {0, 0, 5, 5, 5, 5, 0, -5,},
            {-5, 0, 5, 5, 5, 5, 0, -5,},
            {-10, 0, 5, 5, 5, 5, 0, -10,},
            {-10, 0, 0, 0, 0, 0, 0, -10,},
            {-20, -10, -10, -5, -5, -10, -10, -20,}
        };

        private static readonly float[,] KingMiddleGameScoreTablePlayer = {
            {20, 30, 10, 0, 0, 10, 30, 20},
            {20, 20, 0, 0, 0, 0, 20, 20,},
            {-10, -20, -20, -20, -20, -20, -20, -10,},
            {-20, -30, -30, -40, -40, -30, -30, -20,},
            {-30, -40, -40, -50, -50, -40, -40, -30,},
            {-30, -40, -40, -50, -50, -40, -40, -30,},
            {-30, -40, -40, -50, -50, -40, -40, -30,},
            {-30, -40, -40, -50, -50, -40, -40, -30,}
        };

        private static readonly float[,] KingEndGameScoreTableAI = {
            {-50, -30, -30, -30, -30, -30, -30, -50},
            {-30, -30, 0, 0, 0, 0, -30, -30,},
            {-30, -10, 20, 30, 30, 20, -10, -30,},
            {-30, -10, 30, 40, 40, 30, -10, -30,},
            {-30, -10, 30, 40, 40, 30, -10, -30,},
            {-30, -10, 20, 30, 30, 20, -10, -30,},
            {-30, -20, -10, 0, 0, -10, -20, -30,},
            {-50, -40, -30, -20, -20, -30, -40, -50,}
        };

        #endregion

        public MinMaxMoveCalc(int searchDepth)
        {
            _searchDepth = searchDepth;
        }

        /// <summary>
        /// Returns the best move for the current player by using the MinMax algorithm.
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public BoardPieceMove GetBestMove(Board board)
        {
            BoardPieceMoveScore bestMove = null;
            Maximise(board, 0, ref bestMove, 10000000, board.PlayerTurn);
            return bestMove.Move;
        }

        private float Maximise(Board board, int depth, ref BoardPieceMoveScore maxBestMove, float beta, Player currentPlayer)
        {
            // If the depth limit is reached, don't go any deeper and return the score of the board
            if (depth >= _searchDepth)
                return ScoreTheBoard(board, currentPlayer);

            HashSet<BoardPieceMove> validMoves = new HashSet<BoardPieceMove>();
            ValidMovesCalc.GetValidMovesForPlayer(board, board.PlayerTurn, validMoves);

            float alpha = -10000000; // Set alpha to low num
            BoardPieceMoveScore bestChildMove = null;

            // Iterate through every available move
            foreach (var move in validMoves)
            {
                // Create a copy of the board and then perform the current move
                Board newBoard = BoardHelpers.DuplicateBoard(board);
                newBoard.BoardPieces[move.From.x, move.From.y].HasMoved = true;
                newBoard.BoardPieces[move.To.x, move.To.y] = newBoard.BoardPieces[move.From.x, move.From.y];
                newBoard.BoardPieces[move.From.x, move.From.y] = new BoardPiece();
                newBoard.PlayerTurn = BoardHelpers.GetOpponentPlayer(newBoard.PlayerTurn);

                // Go down a layer and find the minimum score available for the enemy (PLAYER). Set it to alpha if it is greater than a previous alpha.
                alpha = Math.Max(alpha, Minimise(newBoard, depth + 1, alpha, currentPlayer));

                // If the new alpha is greater than the previous best score (or hasn't been set yet)
                if (bestChildMove == null || alpha > bestChildMove.Score)
                {
                    bestChildMove = new BoardPieceMoveScore(move, alpha);
                }

                // If a move is found to be worse than the best score currently stored, return out of the tree early.
                if (alpha > beta)
                {
                    return alpha;
                }
            }

            // If we're on the top layer, return the best move available
            if (depth == 0)
            {
                maxBestMove = bestChildMove;
            }

            return alpha;
        }

        private float Minimise(Board board, int depth, float alpha, Player currentPlayer)
        {
            if (depth >= _searchDepth)
                return ScoreTheBoard(board, currentPlayer);

            HashSet<BoardPieceMove> validMoves = new HashSet<BoardPieceMove>();
            ValidMovesCalc.GetValidMovesForPlayer(board, board.PlayerTurn, validMoves);

            float beta = 10000000;
            BoardPieceMoveScore bestChildMove = null;

            // Iterate through every available move
            foreach (var move in validMoves)
            {
                // Create a copy of the board and then perform the current move
                Board newBoard = BoardHelpers.DuplicateBoard(board);
                newBoard.BoardPieces[move.From.x, move.From.y].HasMoved = true;
                newBoard.BoardPieces[move.To.x, move.To.y] = newBoard.BoardPieces[move.From.x, move.From.y];
                newBoard.BoardPieces[move.From.x, move.From.y] = new BoardPiece();
                newBoard.PlayerTurn = BoardHelpers.GetOpponentPlayer(newBoard.PlayerTurn);
                var newMove = new BoardPieceMoveScore(move, beta);

                beta = Math.Min(beta, Maximise(newBoard, depth + 1, ref newMove, beta, currentPlayer));

                if (bestChildMove == null || beta < bestChildMove.Score)
                {
                    bestChildMove = new BoardPieceMoveScore(move, beta);
                }

                if (beta < alpha)
                {
                    return beta;
                }
            }

            return beta;
        }

        private static float ScoreTheBoard(Board boardToScore, Player currentPlayer)
        {
            float numQueensPlayer = 0,
                numQueensAI = 0,
                numRooksPlayer = 0,
                numRooksAI = 0,
                numBishopsPlayer = 0,
                numBishopsAI = 0,
                numKnightsPlayer = 0,
                numKnightsAI = 0,
                numPawnsPlayer = 0,
                numPawnsAI = 0;

            float score = 0;

            for (int x = 0; x < Board.BOARD_DIMENSIONS; x++)
            {
                for (int y = 0; y < Board.BOARD_DIMENSIONS; y++)
                {
                    //Check for pieces.
                    BoardPiece currentPiece = boardToScore.BoardPieces[x, y];

                    // AI
                    if (currentPiece.PieceOwner == currentPlayer && currentPiece.PieceType != PieceType.None)
                    {
                        switch (currentPiece.PieceType)
                        {
                            case PieceType.Pawn:
                                score += PawnScoreTableAI[x, y];
                                numPawnsAI++;
                                break;

                            case PieceType.Knight:
                                score += KnightScoreTableAI[x, y];
                                numKnightsAI++;
                                break;

                            case PieceType.Bishop:
                                score += BishopScoreTableAI[x, y];
                                numBishopsAI++;
                                break;

                            case PieceType.Castle:
                                score += CastleScoreTableAI[x, y];
                                numRooksAI++;
                                break;

                            case PieceType.Queen:
                                score += QueenScoreTableAI[x, y];
                                numQueensAI++;
                                break;

                            case PieceType.King:
                                score += KING_SCORE;
                                score += KingMiddleGameScoreTableAI[x, y];
                                break;
                        }
                    }
                    
                    // Player
                    if (currentPiece.PieceOwner != currentPlayer && currentPiece.PieceType != PieceType.None)
                    {
                        switch (currentPiece.PieceType)
                        {
                            case PieceType.Pawn:
                                score -= PawnScoreTablePlayer[x, y];
                                numPawnsPlayer++;
                                break;

                            case PieceType.Knight:
                                score -= KnightScoreTablePlayer[x, y];
                                numKnightsPlayer++;
                                break;

                            case PieceType.Bishop:
                                score -= BishopScoreTablePlayer[x, y];
                                numBishopsPlayer++;
                                break;

                            case PieceType.Castle:
                                score -= CastleScoreTablePlayer[x, y];
                                numRooksPlayer++;
                                break;

                            case PieceType.Queen:
                                score -= QueenScoreTablePlayer[x, y];
                                numQueensPlayer++;
                                break;

                            case PieceType.King:
                                score -= KING_SCORE;
                                score -= KingMiddleGameScoreTablePlayer[x, y];
                                break;
                        }
                    }
                }
            }
            
            score += (QUEEN_SCORE * (numQueensAI - numQueensPlayer)) +
                     (CASTLE_SCORE * (numRooksAI - numRooksPlayer)) +
                     (BISHOP_SCORE * (numBishopsAI - numBishopsPlayer)) +
                     (KNIGHT_SCORE * (numKnightsAI - numKnightsPlayer)) +
                     (PAWN_SCORE * (numPawnsAI - numPawnsPlayer));

            return score;
        }
    }
}