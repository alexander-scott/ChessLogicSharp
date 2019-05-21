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
            Board board = BoardFactory.CreateDefaultBoard();
            for (int x = 0; x < Board.BOARD_DIMENSIONS; x++)
            {
                for (int y = 0; y < Board.BOARD_DIMENSIONS; y++)
                {
                    BoardPiece currentPiece = board.BoardPieces[x, y];
                    if (y == 0) // Bottom row
                    {
                        
                    }
                    else if (y == 1)
                    {
                        
                    }
                    else if (y == 6)
                    {
                        
                    }
                    else if (y == 7)
                    {
                        
                    }
                    else
                    {
                        
                    }
                }
            }
        }

        [Test]
        public void ResetBoard()
        {
            
        }
    }
}