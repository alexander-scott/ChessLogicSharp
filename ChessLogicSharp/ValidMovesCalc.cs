using System;
using System.Collections.Generic;
using ChessLogicSharp.DataStructures;

namespace ChessLogicSharp
{
    public static class ValidMovesCalc
    {
        /// <summary>
        /// Calculates all the valid moves for a the current board for the specified player.
        /// </summary>
        /// <param name="board">The current board</param>
        /// <param name="player">The player to fetch moves for</param>
        /// <param name="moves">The hashset containing the valid moves</param>
        public static void GetValidMovesForPlayer(Board board, Player player, HashSet<BoardPieceMove> moves)
        {
            moves.Clear();

            // Go through the board and get the moves for all pieces for our player.
            for (int x = 0; x < Board.BOARD_DIMENSIONS; x++)
            {
                for (int z = 0; z < Board.BOARD_DIMENSIONS; z++)
                {
                    BoardPiece currentPiece = board.BoardPieces[x, z];

                    // We only want pieces that belong to our player.
                    if (currentPiece.PieceOwner == player && currentPiece.PieceType != PieceType.None)
                    {
                        Vector2I piecePosition = new Vector2I(x, z);
                        switch (currentPiece.PieceType)
                        {
                            case PieceType.Pawn:
                                GetPawnMoveOptions(piecePosition, currentPiece, board.BoardPieces, moves);
                                break;

                            case PieceType.Knight:
                                GetKnightMoveOptions(piecePosition, currentPiece, board.BoardPieces, moves);
                                break;

                            case PieceType.Bishop:
                                GetDiagonalMoveOptions(piecePosition, currentPiece, board.BoardPieces, moves);
                                break;

                            case PieceType.Castle:
                                GetUpDownMoveOptions(piecePosition, currentPiece, board.BoardPieces, moves);
                                break;

                            case PieceType.Queen:
                                GetUpDownMoveOptions(piecePosition, currentPiece, board.BoardPieces, moves);
                                GetDiagonalMoveOptions(piecePosition, currentPiece, board.BoardPieces, moves);
                                break;

                            case PieceType.King:
                                GetKingMoveOptions(piecePosition, currentPiece, board, moves);
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns a bool which represents whether the specified player is in check or not.
        /// </summary>
        /// <param name="board">The current board</param>
        /// <param name="player">The player to perform the check query on</param>
        /// <returns>True if the player is in check</returns>
        /// <exception cref="Exception">Thrown if the king is not found on the board</exception>
        public static bool IsPlayerInCheck(BoardPiece[,] board, Player player)
        {
            Vector2I kingPosition = new Vector2I();
            bool kingFound = false;

            // Go through the board and find our KING's position.
            for (int xAxis = 0; xAxis < Board.BOARD_DIMENSIONS; xAxis++)
            {
                for (int yAxis = 0; yAxis < Board.BOARD_DIMENSIONS; yAxis++)
                {
                    BoardPiece currentPiece = board[xAxis, yAxis];
                    if (currentPiece.PieceOwner == player && currentPiece.PieceType == PieceType.King)
                    {
                        kingPosition = new Vector2I(xAxis, yAxis);

                        // Force double loop exit.
                        xAxis = Board.BOARD_DIMENSIONS;
                        yAxis = Board.BOARD_DIMENSIONS;
                        kingFound = true;
                    }
                }
            }

            if (!kingFound)
            {
                throw new Exception("King was not found on the board when checking if player is in check.");
            }

            // Now we have our kings position lets check if it is under attack from anywhere.
            // Horizontal - Right
            int x = kingPosition.X;
            while (++x < Board.BOARD_DIMENSIONS)
            {
                BoardPiece currentPiece = board[x, kingPosition.Y];
                if (currentPiece.PieceType != PieceType.None)
                {
                    if (currentPiece.PieceOwner == player)
                        break;
                    if (currentPiece.PieceType == PieceType.Queen ||
                        currentPiece.PieceType == PieceType.Castle)
                        return true;
                    break;
                }
            }

            // Horizontal - Left
            x = kingPosition.X;
            while (--x >= 0)
            {
                BoardPiece currentPiece = board[x, kingPosition.Y];
                if (currentPiece.PieceType != PieceType.None)
                {
                    if (currentPiece.PieceOwner == player)
                        break;
                    if (currentPiece.PieceType == PieceType.Queen ||
                        currentPiece.PieceType == PieceType.Castle)
                        return true;
                    break;
                }
            }

            // Vertical - Up
            int y = kingPosition.Y;
            while (--y >= 0)
            {
                BoardPiece currentPiece = board[kingPosition.X, y];
                if (currentPiece.PieceType != PieceType.None)
                {
                    if (currentPiece.PieceOwner == player)
                        break;
                    if (currentPiece.PieceType == PieceType.Queen ||
                        currentPiece.PieceType == PieceType.Castle)
                        return true;
                    break;
                }
            }

            // Vertical - Down
            y = kingPosition.Y;
            while (++y < Board.BOARD_DIMENSIONS)
            {
                BoardPiece currentPiece = board[kingPosition.X, y];
                if (currentPiece.PieceType != PieceType.None)
                {
                    if (currentPiece.PieceOwner == player)
                        break;
                    if (currentPiece.PieceType == PieceType.Queen ||
                        currentPiece.PieceType == PieceType.Castle)
                        return true;
                    break;
                }
            }

            // Diagonal - Right Down
            x = kingPosition.X;
            y = kingPosition.Y;
            while (++y < Board.BOARD_DIMENSIONS && ++x < Board.BOARD_DIMENSIONS)
            {
                BoardPiece currentPiece = board[x, y];
                if (currentPiece.PieceType != PieceType.None)
                {
                    if (currentPiece.PieceOwner == player)
                        break;
                    if (currentPiece.PieceType == PieceType.Queen ||
                        currentPiece.PieceType == PieceType.Bishop)
                        return true;
                    break;
                }
            }

            // Diagonal - Right Up
            x = kingPosition.X;
            y = kingPosition.Y;
            while (--y >= 0 && ++x < Board.BOARD_DIMENSIONS)
            {
                BoardPiece currentPiece = board[x, y];
                if (currentPiece.PieceType != PieceType.None)
                {
                    if (currentPiece.PieceOwner == player)
                        break;
                    if (currentPiece.PieceType == PieceType.Queen ||
                        currentPiece.PieceType == PieceType.Bishop)
                        return true;
                    break;
                }
            }

            //Diagonal - Left Down
            x = kingPosition.X;
            y = kingPosition.Y;
            while (++y < Board.BOARD_DIMENSIONS && --x >= 0)
            {
                BoardPiece currentPiece = board[x, y];
                if (currentPiece.PieceType != PieceType.None)
                {
                    if (currentPiece.PieceOwner == player)
                        break;
                    if (currentPiece.PieceType == PieceType.Queen ||
                        currentPiece.PieceType == PieceType.Bishop)
                        return true;
                    break;
                }
            }

            // Diagonal - Left Up
            x = kingPosition.X;
            y = kingPosition.Y;
            while (--y >= 0 && --x >= 0)
            {
                BoardPiece currentPiece = board[x, y];
                if (currentPiece.PieceType != PieceType.None)
                {
                    if (currentPiece.PieceOwner == player)
                        break;
                    if (currentPiece.PieceType == PieceType.Queen ||
                        currentPiece.PieceType == PieceType.Bishop)
                        return true;
                    break;
                }
            }

            // Awkward Knight moves
            x = kingPosition.X + 2;
            y = kingPosition.Y + 1;
            if (x < Board.BOARD_DIMENSIONS && y < Board.BOARD_DIMENSIONS)
            {
                BoardPiece currentPiece = board[x, y];
                if (currentPiece.PieceOwner != player &&
                    currentPiece.PieceType == PieceType.Knight)
                    return true;
            }

            x = kingPosition.X + 2;
            y = kingPosition.Y - 1;
            if (x < Board.BOARD_DIMENSIONS && y >= 0)
            {
                BoardPiece currentPiece = board[x, y];
                if (currentPiece.PieceOwner != player &&
                    currentPiece.PieceType == PieceType.Knight)
                    return true;
            }

            x = kingPosition.X + 1;
            y = kingPosition.Y + 2;
            if (x < Board.BOARD_DIMENSIONS && y < Board.BOARD_DIMENSIONS)
            {
                BoardPiece currentPiece = board[x, y];
                if (currentPiece.PieceOwner != player &&
                    currentPiece.PieceType == PieceType.Knight)
                    return true;
            }

            x = kingPosition.X - 1;
            y = kingPosition.Y + 2;
            if (x >= 0 && y < Board.BOARD_DIMENSIONS)
            {
                BoardPiece currentPiece = board[x, y];
                if (currentPiece.PieceOwner != player &&
                    currentPiece.PieceType == PieceType.Knight)
                    return true;
            }

            x = kingPosition.X - 2;
            y = kingPosition.Y + 1;
            if (x >= 0 && y < Board.BOARD_DIMENSIONS)
            {
                BoardPiece currentPiece = board[x, y];
                if (currentPiece.PieceOwner != player &&
                    currentPiece.PieceType == PieceType.Knight)
                    return true;
            }

            x = kingPosition.X - 2;
            y = kingPosition.Y - 1;
            if (x >= 0 && y >= 0)
            {
                BoardPiece currentPiece = board[x, y];
                if (currentPiece.PieceOwner != player &&
                    currentPiece.PieceType == PieceType.Knight)
                    return true;
            }

            x = kingPosition.X - 1;
            y = kingPosition.Y - 2;
            if (x >= 0 && y >= 0)
            {
                BoardPiece currentPiece = board[x, y];
                if (currentPiece.PieceOwner != player &&
                    currentPiece.PieceType == PieceType.Knight)
                    return true;
            }

            x = kingPosition.X + 1;
            y = kingPosition.Y - 2;
            if (x < Board.BOARD_DIMENSIONS && y >= 0)
            {
                BoardPiece currentPiece = board[x, y];
                if (currentPiece.PieceOwner != player &&
                    currentPiece.PieceType == PieceType.Knight)
                    return true;
            }

            // Opponent King positions
            for (int yPos = kingPosition.Y - 1; yPos < kingPosition.Y + 2; yPos++)
            {
                for (int xPos = kingPosition.X - 1; xPos < kingPosition.X + 2; xPos++)
                {
                    if (xPos >= 0 && xPos < Board.BOARD_DIMENSIONS && yPos >= 0 && yPos < Board.BOARD_DIMENSIONS)
                    {
                        BoardPiece currentPiece = board[xPos, yPos];

                        // Must be the opponents king, as w will pass over our own in this embedded loop.
                        if (currentPiece.PieceOwner != player &&
                            currentPiece.PieceType == PieceType.King)
                            return true;
                    }
                }
            }

            // Opponent Pawns
            int opponentPawnDirection = BoardHelpers.GetPlayerDirection(player);
            x = kingPosition.X + 1;
            y = kingPosition.Y + opponentPawnDirection;
            if (x < Board.BOARD_DIMENSIONS && y >= 0 && y < Board.BOARD_DIMENSIONS)
            {
                BoardPiece currentPiece = board[x, y];
                if (currentPiece.PieceOwner != player &&
                    currentPiece.PieceType == PieceType.Pawn)
                    return true;
            }

            x = kingPosition.X - 1;
            y = kingPosition.Y + opponentPawnDirection;
            if (x >= 0 && y >= 0 && y < Board.BOARD_DIMENSIONS)
            {
                BoardPiece currentPiece = board[x, y];
                if (currentPiece.PieceOwner != player &&
                    currentPiece.PieceType == PieceType.Pawn)
                    return true;
            }

            return false;
        }

        public static bool PlayerCanMove(Board board, Player player)
        {
            HashSet<BoardPieceMove> currentValidMoves = new HashSet<BoardPieceMove>();
            GetValidMovesForPlayer(board, player, currentValidMoves);
            if (currentValidMoves.Count == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check the current move is valid and if it is, add it to the hashset of moves
        /// </summary>
        /// <param name="moveToCheck">The move to check the validity of</param>
        /// <param name="player">The player that is performing the move</param>
        /// <param name="board">The board to check the move on</param>
        /// <param name="moves">The hashset containing all the moves</param>
        /// <returns>True if the move is valid</returns>
        private static bool CheckMoveValidityAndStoreMove(BoardPieceMove moveToCheck, Player player,
            BoardPiece[,] board, HashSet<BoardPieceMove> moves)
        {
            BoardPiece[,] tempBoard = BoardHelpers.DuplicateBoard(board);

            if (moveToCheck.To.X >= 0 && moveToCheck.To.X < Board.BOARD_DIMENSIONS &&
                moveToCheck.To.Y >= 0 && moveToCheck.To.Y < Board.BOARD_DIMENSIONS)
            {
                // We check with colour passed in to enable the same functions to construct attacked spaces
                // as well as constructing the positions we can move to.
                if (tempBoard[moveToCheck.To.X, moveToCheck.To.Y].PieceType == PieceType.None)
                {
                    // Will this leave us in check?
                    tempBoard[moveToCheck.To.X, moveToCheck.To.Y] = tempBoard[moveToCheck.From.X, moveToCheck.From.Y];
                    tempBoard[moveToCheck.From.X, moveToCheck.From.Y] = new BoardPiece();

                    if (!IsPlayerInCheck(tempBoard, player))
                    {
                        moves.Add(moveToCheck);
                    }
                }
                else
                {
                    // A piece so no more moves after this, but can we take it?
                    if (tempBoard[moveToCheck.To.X, moveToCheck.To.Y].PieceOwner != player)
                    {
                        // Will this leave us in check?
                        tempBoard[moveToCheck.To.X, moveToCheck.To.Y] = tempBoard[moveToCheck.From.X, moveToCheck.From.Y];
                        tempBoard[moveToCheck.From.X, moveToCheck.From.Y] = new BoardPiece();

                        if (!IsPlayerInCheck(tempBoard, player))
                        {
                            moves.Add(moveToCheck);
                        }
                    }

                    // Hit a piece, so no more moves in this direction.
                    return false;
                }

                return true;
            }

            return false;
        }
        
        public static bool IsMoveValid(BoardPieceMove moveToCheck, Player player, Board board)
        {
            var moves = new HashSet<BoardPieceMove>();
            GetValidMovesForPlayer(board, player, moves);

            if (!moves.Contains(moveToCheck))
            {
                return false;
            }
            
            BoardPiece[,] tempBoard = BoardHelpers.DuplicateBoard(board.BoardPieces);

            if (moveToCheck.To.X >= 0 && moveToCheck.To.X < Board.BOARD_DIMENSIONS &&
                moveToCheck.To.Y >= 0 && moveToCheck.To.Y < Board.BOARD_DIMENSIONS)
            {
                // We check with colour passed in to enable the same functions to construct attacked spaces
                // as well as constructing the positions we can move to.
                if (tempBoard[moveToCheck.To.X, moveToCheck.To.Y].PieceType == PieceType.None)
                {
                    // Will this leave us in check?
                    tempBoard[moveToCheck.To.X, moveToCheck.To.Y] = tempBoard[moveToCheck.From.X, moveToCheck.From.Y];
                    tempBoard[moveToCheck.From.X, moveToCheck.From.Y] = new BoardPiece();

                    if (!IsPlayerInCheck(tempBoard, player))
                    {
                        return true;
                    }
                }
                else
                {
                    // A piece so no more moves after this, but can we take it?
                    if (tempBoard[moveToCheck.To.X, moveToCheck.To.Y].PieceOwner != player)
                    {
                        // Will this leave us in check?
                        tempBoard[moveToCheck.To.X, moveToCheck.To.Y] = tempBoard[moveToCheck.From.X, moveToCheck.From.Y];
                        tempBoard[moveToCheck.From.X, moveToCheck.From.Y] = new BoardPiece();

                        if (!IsPlayerInCheck(tempBoard, player))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Calculates all the possible moves for a pawn piece.
        /// </summary>
        /// <param name="piecePosition">The current position of the piece.</param>
        /// <param name="boardPiece">The piece data.</param>
        /// <param name="board">The board to calculate the moves on</param>
        /// <param name="moves">The hashset containing all the moves</param>
        private static void GetPawnMoveOptions(Vector2I piecePosition, BoardPiece boardPiece,
            BoardPiece[,] board, HashSet<BoardPieceMove> moves)
        {
            BoardPiece[,] tempBoard = BoardHelpers.DuplicateBoard(board);
            int pawnDirection = BoardHelpers.GetPlayerDirection(boardPiece.PieceOwner);

            // Single step FORWARD.
            int xPos = piecePosition.X;
            int yPos = piecePosition.Y + pawnDirection;
            if (yPos >= 0 && yPos < Board.BOARD_DIMENSIONS && tempBoard[xPos, yPos].PieceType == PieceType.None)
            {
                // Will this leave us in check? Only interested in check if its one of our moves.
                tempBoard[xPos, yPos] = tempBoard[piecePosition.X, piecePosition.Y];
                tempBoard[piecePosition.X, piecePosition.Y] = new BoardPiece();
                if (!IsPlayerInCheck(tempBoard, boardPiece.PieceOwner))
                {
                    moves.Add(new BoardPieceMove(piecePosition.X, piecePosition.Y, piecePosition.X, piecePosition.Y + pawnDirection));
                }
            }

            // Double step FORWARD.
            if ((boardPiece.PieceOwner == Player.PlayerOne && piecePosition.Y == 1) ||
                (boardPiece.PieceOwner == Player.PlayerTwo && piecePosition.Y == 6))
            {
                //Reset the board.
                tempBoard = BoardHelpers.DuplicateBoard(board);

                int yPos2 = piecePosition.Y + pawnDirection * 2;
                if (tempBoard[xPos, yPos].PieceType == PieceType.None &&
                    tempBoard[xPos, yPos2].PieceType == PieceType.None)
                {
                    //Will this leave us in check? Only interested in check if its one of our moves.
                    tempBoard[xPos, yPos2] = tempBoard[piecePosition.X, piecePosition.Y];
                    tempBoard[piecePosition.X, piecePosition.Y] = new BoardPiece();

                    if (!IsPlayerInCheck(tempBoard, boardPiece.PieceOwner))
                    {
                        moves.Add(new BoardPieceMove(piecePosition.X, piecePosition.Y, piecePosition.X, yPos2));
                    }
                }
            }

            // En'Passant move.
            if (piecePosition.Y == 4 && boardPiece.PieceOwner == Player.PlayerOne ||
                piecePosition.Y == 3 && boardPiece.PieceOwner == Player.PlayerTwo)
            {
                // Pawn beside us, can we en'passant.
                xPos = piecePosition.X - 1;
                yPos = piecePosition.Y;
                if (xPos >= 0)
                {
                    // Reset the board.
                    tempBoard = BoardHelpers.DuplicateBoard(board);
                    
                    BoardPiece leftPiece = tempBoard[xPos, yPos];
                    if (leftPiece.PieceType == PieceType.Pawn && leftPiece.CanEnPassant)
                    {
                        //Will this leave us in check? Only interested in check if its one of our moves.
                        tempBoard[xPos, yPos] = tempBoard[piecePosition.X, piecePosition.Y];
                        tempBoard[piecePosition.X, piecePosition.Y] = new BoardPiece();

                        if (!IsPlayerInCheck(tempBoard, boardPiece.PieceOwner))
                        {
                            moves.Add(new BoardPieceMove(piecePosition.X, piecePosition.Y,piecePosition.X - 1, piecePosition.Y + pawnDirection));
                        }
                    }
                }

                xPos = piecePosition.X + 1;
                if (xPos < Board.BOARD_DIMENSIONS)
                {
                    // Reset the board.
                    tempBoard = BoardHelpers.DuplicateBoard(board);
                    
                    BoardPiece rightPiece = tempBoard[xPos, yPos];
                    if (rightPiece.PieceType == PieceType.Pawn && rightPiece.CanEnPassant)
                    {
                        //Will this leave us in check? Only interested in check if its one of our moves.
                        tempBoard[xPos, yPos] = tempBoard[piecePosition.X, piecePosition.Y];
                        tempBoard[piecePosition.X, piecePosition.Y] = new BoardPiece();

                        if (!IsPlayerInCheck(tempBoard, boardPiece.PieceOwner))
                        {
                            moves.Add(new BoardPieceMove(piecePosition.X, piecePosition.Y,
                                piecePosition.X + 1, piecePosition.Y + pawnDirection));
                        }
                    }
                }
            }

            // Take a piece move.
            if (piecePosition.Y > 0 && piecePosition.Y < Board.BOARD_DIMENSIONS - 1)
            {
                // Ahead of selected pawn to the LEFT.
                if (piecePosition.X > 0)
                {
                    // Reset the board.
                    tempBoard = BoardHelpers.DuplicateBoard(board);

                    xPos = piecePosition.X - 1;
                    yPos = piecePosition.Y + pawnDirection;
                    BoardPiece aheadLeftPiece = tempBoard[xPos, yPos];
                    if (aheadLeftPiece.PieceType != PieceType.None &&
                        aheadLeftPiece.PieceOwner != boardPiece.PieceOwner)
                    {
                        // Will this leave us in check? Only interested in check if its one of our moves.
                        tempBoard[xPos, yPos] = tempBoard[piecePosition.X, piecePosition.Y];
                        tempBoard[piecePosition.X, piecePosition.Y] = new BoardPiece();

                        if (!IsPlayerInCheck(tempBoard, boardPiece.PieceOwner))
                        {
                            moves.Add(new BoardPieceMove(piecePosition.X, piecePosition.Y, xPos, piecePosition.Y + pawnDirection));
                        }
                    }
                }

                // Ahead of selected pawn to the RIGHT.
                if (piecePosition.X < Board.BOARD_DIMENSIONS - 1)
                {
                    //Rest the board.
                    tempBoard = BoardHelpers.DuplicateBoard(board);

                    xPos = piecePosition.X + 1;
                    yPos = piecePosition.Y + pawnDirection;
                    BoardPiece aheadRightPiece = tempBoard[xPos, yPos];
                    if (aheadRightPiece.PieceType != PieceType.None &&
                        aheadRightPiece.PieceOwner != boardPiece.PieceOwner)
                    {
                        // Will this leave us in check? Only interested in check if its one of our moves.
                        tempBoard[xPos, yPos] = tempBoard[piecePosition.X, piecePosition.Y];
                        tempBoard[piecePosition.X, piecePosition.Y] = new BoardPiece();

                        if (!IsPlayerInCheck(tempBoard, boardPiece.PieceOwner))
                        {
                            moves.Add(new BoardPieceMove(piecePosition.X, piecePosition.Y,
                                xPos, piecePosition.Y + pawnDirection));
                        }
                    }
                }
            }
        }

        private static void GetKnightMoveOptions(Vector2I piecePosition, BoardPiece currentPiece,
            BoardPiece[,] boardBoardPieces, HashSet<BoardPieceMove> moves)
        {
            //Moves to the RIGHT.
            var move = new BoardPieceMove(piecePosition.X, piecePosition.Y, piecePosition.X + 2,piecePosition.Y + 1);
            CheckMoveValidityAndStoreMove(move, currentPiece.PieceOwner, boardBoardPieces, moves);

            move.To.Y = piecePosition.Y - 1;
            CheckMoveValidityAndStoreMove(move, currentPiece.PieceOwner, boardBoardPieces, moves);

            //Moves to the LEFT.
            move.To.X = piecePosition.X - 2;
            move.To.Y = piecePosition.Y + 1;
            CheckMoveValidityAndStoreMove(move, currentPiece.PieceOwner, boardBoardPieces, moves);

            move.To.Y = piecePosition.Y - 1;
            CheckMoveValidityAndStoreMove(move, currentPiece.PieceOwner, boardBoardPieces, moves);

            //Moves ABOVE.
            move.To.X = piecePosition.X + 1;
            move.To.Y = piecePosition.Y - 2;
            CheckMoveValidityAndStoreMove(move, currentPiece.PieceOwner, boardBoardPieces, moves);

            move.To.X = piecePosition.X - 1;
            CheckMoveValidityAndStoreMove(move, currentPiece.PieceOwner, boardBoardPieces, moves);

            //Moves BELOW.
            move.To.X = piecePosition.X + 1;
            move.To.Y = piecePosition.Y + 2;
            CheckMoveValidityAndStoreMove(move, currentPiece.PieceOwner, boardBoardPieces, moves);

            move.To.X = piecePosition.X - 1;
            CheckMoveValidityAndStoreMove(move, currentPiece.PieceOwner, boardBoardPieces, moves);
        }


        private static void GetUpDownMoveOptions(Vector2I piecePosition, BoardPiece currentPiece,
            BoardPiece[,] boardPieces, HashSet<BoardPieceMove> moves)
        {
            BoardPieceMove move;

            //Vertical DOWN the board.
            for (int yPos = piecePosition.Y + 1; yPos < Board.BOARD_DIMENSIONS; yPos++)
            {
                //Keep checking moves until one is invalid.
                move = new BoardPieceMove(piecePosition.X, piecePosition.Y, piecePosition.X, yPos);
                if (CheckMoveValidityAndStoreMove(move, currentPiece.PieceOwner, boardPieces, moves) == false)
                    break;
            }

            //Vertical UP the board.
            for (int yPos = piecePosition.Y - 1; yPos >= 0; yPos--)
            {
                //Keep checking moves until one is invalid.
                move = new BoardPieceMove(piecePosition.X, piecePosition.Y, piecePosition.X, yPos);
                if (CheckMoveValidityAndStoreMove(move, currentPiece.PieceOwner, boardPieces, moves) == false)
                    break;
            }

            //Horizontal LEFT of the board.
            for (int xPos = piecePosition.X - 1; xPos >= 0; xPos--)
            {
                //Keep checking moves until one is invalid.
                move = new BoardPieceMove(piecePosition.X, piecePosition.Y, xPos, piecePosition.Y);
                if (CheckMoveValidityAndStoreMove(move, currentPiece.PieceOwner, boardPieces, moves) == false)
                    break;
            }

            //Horizontal RIGHT of the board.
            for (int xPos = piecePosition.X + 1; xPos < Board.BOARD_DIMENSIONS; xPos++)
            {
                //Keep checking moves until one is invalid.
                move = new BoardPieceMove(piecePosition.X, piecePosition.Y, xPos, piecePosition.Y);
                if (CheckMoveValidityAndStoreMove(move, currentPiece.PieceOwner, boardPieces, moves) == false)
                    break;
            }
        }

        private static void GetDiagonalMoveOptions(Vector2I piecePosition, BoardPiece currentPiece,
            BoardPiece[,] boardPieces, HashSet<BoardPieceMove> moves)
        {
            BoardPieceMove move;

            //ABOVE & LEFT
            for (int yPos = piecePosition.Y - 1, xPos = piecePosition.X - 1;
                yPos >= 0 && xPos >= 0;
                yPos--, xPos--)
            {
                //Keep checking moves until one is invalid.
                move = new BoardPieceMove(piecePosition.X, piecePosition.Y, xPos, yPos);
                if (CheckMoveValidityAndStoreMove(move, currentPiece.PieceOwner, boardPieces, moves) == false)
                    break;
            }

            //ABOVE & RIGHT
            for (int yPos = piecePosition.Y - 1, xPos = piecePosition.X + 1;
                yPos >= 0 && xPos < Board.BOARD_DIMENSIONS;
                yPos--, xPos++)
            {
                //Keep checking moves until one is invalid.
                move = new BoardPieceMove(piecePosition.X, piecePosition.Y, xPos, yPos);
                if (CheckMoveValidityAndStoreMove(move, currentPiece.PieceOwner, boardPieces, moves) == false)
                    break;
            }

            //BELOW & LEFT
            for (int yPos = piecePosition.Y + 1, xPos = piecePosition.X - 1;
                yPos < Board.BOARD_DIMENSIONS && xPos >= 0;
                yPos++, xPos--)
            {
                //Keep checking moves until one is invalid.
                move = new BoardPieceMove(piecePosition.X, piecePosition.Y, xPos, yPos);
                if (CheckMoveValidityAndStoreMove(move, currentPiece.PieceOwner, boardPieces, moves) == false)
                    break;
            }

            //BELOW & RIGHT
            for (int yPos = piecePosition.Y + 1, xPos = piecePosition.X + 1;
                yPos < Board.BOARD_DIMENSIONS && xPos < Board.BOARD_DIMENSIONS;
                yPos++, xPos++)
            {
                //Keep checking moves until one is invalid.
                move = new BoardPieceMove(piecePosition.X, piecePosition.Y, xPos, yPos);
                if (CheckMoveValidityAndStoreMove(move, currentPiece.PieceOwner, boardPieces, moves) == false)
                    break;
            }
        }

        private static void GetKingMoveOptions(Vector2I piecePosition, BoardPiece kingPiece,
            Board board, HashSet<BoardPieceMove> moves)
        {
            BoardPieceMove move;

            //Start at position top left of king and move across and down.
            for (int yPos = piecePosition.Y - 1; yPos <= piecePosition.Y + 1; yPos++)
            {
                for (int xPos = piecePosition.X - 1; xPos <= piecePosition.X + 1; xPos++)
                {
                    if (yPos >= 0 && yPos < Board.BOARD_DIMENSIONS && xPos >= 0 && xPos < Board.BOARD_DIMENSIONS)
                    {
                        //Check if move is valid and store it. We dont care about the return value as we are only
                        // checking one move in each direction.
                        move = new BoardPieceMove(piecePosition.X, piecePosition.Y, xPos, yPos);
                        CheckMoveValidityAndStoreMove(move, kingPiece.PieceOwner, board.BoardPieces, moves);
                    }
                }
            }

            // Fixes the recursive issue. Only check moves and fetch opponent moves for the current player turn on the board
            if (board.PlayerTurn == kingPiece.PieceOwner)
            {
                Player opponent = kingPiece.PieceOwner == Player.PlayerOne ? Player.PlayerTwo : Player.PlayerOne;

                //Compile all the moves available to our opponent. Duplicate the board BUT keep the player turn the same.
                var tempBoard = BoardHelpers.DuplicateBoard(board);
                HashSet<BoardPieceMove> allOpponentMoves = new HashSet<BoardPieceMove>();
                GetValidMovesForPlayer(tempBoard, opponent, allOpponentMoves);

                //Can CASTLE if not in CHECK.
                if (!IsPlayerInCheck(tempBoard.BoardPieces, kingPiece.PieceOwner))
                {
                    //CASTLE to the right.
                    BoardPiece king = tempBoard.BoardPieces[piecePosition.X, piecePosition.Y];

                    if (!king.HasMoved)
                    {
                        BoardPiece rightRook = tempBoard.BoardPieces[piecePosition.X + 3, piecePosition.Y];
                        if (rightRook.PieceType == PieceType.Castle && !rightRook.HasMoved)
                        {
                            if (tempBoard.BoardPieces[piecePosition.X + 1, piecePosition.Y].PieceType == PieceType.None &&
                                tempBoard.BoardPieces[piecePosition.X + 2, piecePosition.Y].PieceType == PieceType.None)
                            {
                                //Cannot CASTLE through a CHECK position.
                                bool canCastle = true;
                                foreach (var oppMove in allOpponentMoves)
                                {
                                    if (oppMove.To.X == piecePosition.X + 2 &&
                                        oppMove.To.Y == piecePosition.Y ||
                                        oppMove.To.X == piecePosition.X + 1 &&
                                        oppMove.To.Y == piecePosition.Y)
                                    {
                                        canCastle = false;
                                    }
                                }

                                //Check if the final position is valid.
                                if (canCastle)
                                {
                                    move = new BoardPieceMove(piecePosition.X, piecePosition.Y, piecePosition.X + 2, piecePosition.Y);
                                    CheckMoveValidityAndStoreMove(move, kingPiece.PieceOwner, tempBoard.BoardPieces, moves);
                                }
                            }
                        }

                        //CASTLE to the left.
                        tempBoard = BoardHelpers.DuplicateBoard(board);
                        BoardPiece leftRook = tempBoard.BoardPieces[piecePosition.X - 4, piecePosition.Y];

                        if (leftRook.PieceType == PieceType.Castle && !leftRook.HasMoved)
                        {
                            if (tempBoard.BoardPieces[piecePosition.X - 1, piecePosition.Y].PieceType == PieceType.None &&
                                tempBoard.BoardPieces[piecePosition.X - 2, piecePosition.Y].PieceType == PieceType.None &&
                                tempBoard.BoardPieces[piecePosition.X - 3, piecePosition.Y].PieceType == PieceType.None)
                            {
                                //Cannot CASTLE through a CHECK position.
                                bool canCastle = true;
                                foreach (var oppMove in allOpponentMoves)
                                {
                                    if (oppMove.To.X == piecePosition.X - 1 &&
                                        oppMove.To.Y == piecePosition.Y ||
                                        oppMove.To.X == piecePosition.X - 2 &&
                                        oppMove.To.Y == piecePosition.Y ||
                                        oppMove.To.X == piecePosition.X - 3 &&
                                        oppMove.To.Y == piecePosition.Y)
                                    {
                                        canCastle = false;
                                    }
                                }

                                //Check if the final position is valid.
                                if (canCastle)
                                {
                                    move = new BoardPieceMove(piecePosition.X, piecePosition.Y, piecePosition.X - 2, piecePosition.Y);
                                    CheckMoveValidityAndStoreMove(move, kingPiece.PieceOwner, tempBoard.BoardPieces, moves);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}