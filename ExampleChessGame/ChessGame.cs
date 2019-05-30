using System;
using System.Collections.Generic;
using ChessLogicSharp;
using ChessLogicSharp.ChessPlayers;
using ChessLogicSharp.DataStructures;
using ChessLogicTests;

namespace ExampleChessGame
{
    internal class ChessGame
    {
        public static void Main(string[] args)
        {
            // Create the board
            var board = BoardFactory.CreateBoard();
            
            BasicPlayer player1 = new BasicPlayer(board, Player.PlayerOne);
            board.AddPlayer(player1);
            BasicPlayer player2 = new BasicPlayer(board, Player.PlayerTwo);
            board.AddPlayer(player2);

            // Loop until the game is over
            while (board.GameState != GameState.Ended)
            {
                // Render the board in the console
                PrintBoardToConsole(board);
                
                // Fetch the current players turn
                Console.Write(board.PlayerTurn.ToFriendlyString() + " enter your move: ");
                var moveString = Console.ReadLine();

                // Ensure the input is in the correct format (xFrom,yFrom,xTo,yTo) - no commas and must be in the correct chess notation e.g. a1a3
                if (!BoardHelpers.ValidMoveRepresentation(moveString))
                {
                    Console.WriteLine("Invalid string, please enter it again.");
                    continue;
                }

                // Fetch the available moves for the player
                var validMoves = new HashSet<BoardPieceMove>();
                ValidMovesCalc.GetValidMovesForPlayer(board, board.PlayerTurn, validMoves);
                
                // Create an instance of the move
                var from = BoardHelpers.ConvertStringRepIntoPos(moveString.Substring(0, 2));
                var to = BoardHelpers.ConvertStringRepIntoPos(moveString.Substring(2, 2));
                var move = new BoardPieceMove(from, to);

                // Make sure the move is legal
                if (!validMoves.Contains(move))
                {
                    Console.WriteLine("Invalid move, please enter it again.");
                    continue;
                }

                // Apply the move
                if (board.PlayerTurn == Player.PlayerOne)
                {
                    player1.ApplyMove(move);
                }
                else
                {
                    player2.ApplyMove(move);
                }
            }
            
            PrintBoardToConsole(board);
            Console.WriteLine(board.PlayerTurn.ToFriendlyString() + " wins!");

            Console.ReadLine();
        }

        private static void PrintBoardToConsole(Board board)
        {
            // Rotate the array to make it look better in the console as it currently goes horizontally rather than vertically
            var newBoard = RotateArray(board.BoardPieces);
            
            var xLength = newBoard.GetLength(0);
            var yLength = newBoard.GetLength(1);

            for (var i = 0; i < xLength; i++)
            {
                for (var j = 0; j < yLength; j++)
                {
                    Console.Write($"{GetStringRepForPiece(newBoard[i, j])} ");
                }

                Console.Write(Environment.NewLine + Environment.NewLine);
            }
        }

        private static string GetStringRepForPiece(BoardPiece piece)
        {
            switch (piece.PieceType)
            {
                case PieceType.Castle:
                    return piece.PieceOwner == Player.PlayerOne ? "c" : "C";
                case PieceType.Knight:
                    return piece.PieceOwner == Player.PlayerOne ? "n" : "N";
                case PieceType.Bishop:
                    return piece.PieceOwner == Player.PlayerOne ? "b" : "B";
                case PieceType.Queen:
                    return piece.PieceOwner == Player.PlayerOne ? "q" : "Q";
                case PieceType.King:
                    return piece.PieceOwner == Player.PlayerOne ? "k" : "K";
                case PieceType.Pawn:
                    return piece.PieceOwner == Player.PlayerOne ? "p" : "P";
                default:
                    return "e";
            }
        }

        private static BoardPiece[,] RotateArray(BoardPiece[,] board)
        {
            var ret = new BoardPiece[8, 8];

            for (var i = 0; i < 8; ++i)
            {
                for (var j = 0; j < 8; ++j)
                {
                    ret[i, j] = board[j, 8 - i - 1];
                }
            }

            return ret;
        }
    }
}