using System;
using System.Diagnostics.Eventing.Reader;
using ChessLogicSharp;
using ChessLogicSharp.DataStructures;
using NUnit.Framework;

namespace ChessLogicTests
{
    [TestFixture]
    public class CreationTests
    {
        [Test]
        public void CheckDefaultPositions()
        {
            Board board = BoardFactory.CreateBoard();
            for (int x = 0; x < Board.BOARD_DIMENSIONS; x++)
            {
                for (int y = 0; y < Board.BOARD_DIMENSIONS; y++)
                {
                    BoardPiece currentPiece = board.BoardPieces[x, y];
                    if (y == 0) // Bottom row - P1
                    {
                        Assert.IsTrue(PieceTypePositionCheck(x, Player.PlayerOne, currentPiece));
                    }
                    else if (y == 1) // Pawn row - P1
                    {
                        Assert.IsTrue(currentPiece.PieceType == PieceType.Pawn && currentPiece.PieceOwner == Player.PlayerOne);
                    }
                    else if (y == 6) // Pawn row - P2
                    {
                        Assert.IsTrue(currentPiece.PieceType == PieceType.Pawn && currentPiece.PieceOwner == Player.PlayerTwo);
                    }
                    else if (y == 7) // Top row - P2
                    {
                        Assert.IsTrue(PieceTypePositionCheck(x, Player.PlayerTwo, currentPiece));
                    }
                    else // Empty squares
                    {
                        Assert.IsTrue(currentPiece.PieceType == PieceType.None && currentPiece.PieceOwner == Player.None);
                    }
                }
            }
        }

        private bool PieceTypePositionCheck(int xPos, Player player, BoardPiece piece)
        {
            switch (xPos)
            {
                case 0: // Left castle
                    if (piece.PieceType == PieceType.Castle && piece.PieceOwner == player)
                        return true;
                    break;
                case 1: // Left knight
                    if (piece.PieceType == PieceType.Knight && piece.PieceOwner == player)
                        return true;
                    break;
                case 2: // Left bishop
                    if (piece.PieceType == PieceType.Bishop && piece.PieceOwner == player)
                        return true;
                    break;
                case 3: // Queen
                    if (piece.PieceType == PieceType.Queen && piece.PieceOwner == player)
                        return true;
                    break;
                case 4: // Knight
                    if (piece.PieceType == PieceType.King && piece.PieceOwner == player)
                        return true;
                    break;
                case 5: // Right bishop
                    if (piece.PieceType == PieceType.Bishop && piece.PieceOwner == player)
                        return true;
                    break;
                case 6: // Right knight
                    if (piece.PieceType == PieceType.Knight && piece.PieceOwner == player)
                        return true;
                    break;
                case 7: // Right castle
                    if (piece.PieceType == PieceType.Castle && piece.PieceOwner == player)
                        return true;
                    break;
            }

            return false;
        }
    }
}