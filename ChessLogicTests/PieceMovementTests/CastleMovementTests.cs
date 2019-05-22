using ChessLogicSharp;
using ChessLogicSharp.DataStructures;
using NUnit.Framework;

namespace ChessLogicTests.PieceMovementTests
{
    [TestFixture]
    public class CastleMovementTests
    {
        [Test]
        public void ValidMoveHorizontal()
        {
            char[,] boardLayout =
            {
                {'C', 'N', 'B', 'Q', 'K', 'B', 'N', 'C'},
                {'P', 'P', 'P', 'P', 'P', 'P', 'P', 'P'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'}, 
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'}, 
                {'c', 'e', 'e', 'e', 'e', 'e', 'e', 'e'},
                {'e', 'p', 'p', 'p', 'p', 'p', 'p', 'p'},
                {'e', 'n', 'b', 'q', 'k', 'b', 'n', 'c'}
            };
            boardLayout = boardLayout.RotateArray();
            
            Board board = BoardFactory.CreateBoard(boardLayout);
            
            Vector2I castlePos = new Vector2I(0, 2);
            Assert.IsTrue(board.BoardPieces[castlePos.x, castlePos.y].PieceType == PieceType.Castle);
            
            Vector2I castleDest = new Vector2I(7, 2);
            Assert.IsTrue(board.BoardPieces[castleDest.x, castleDest.y].PieceType == PieceType.None);
            
            BoardPieceMove move = new BoardPieceMove(castlePos, castleDest);
            board.ApplyMove(move);

            Assert.IsTrue(board.BoardPieces[castlePos.x, castlePos.y].PieceType == PieceType.None);
            Assert.IsTrue(board.BoardPieces[castleDest.x, castleDest.y].PieceType == PieceType.Castle);
        }
        
        [Test]
        public void ValidMoveVertical()
        {
            char[,] boardLayout =
            {
                {'C', 'N', 'B', 'Q', 'K', 'B', 'N', 'C'},
                {'P', 'P', 'P', 'P', 'P', 'P', 'P', 'P'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'}, 
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'}, 
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'}, 
                {'e', 'p', 'p', 'p', 'p', 'p', 'p', 'p'},
                {'c', 'n', 'b', 'q', 'k', 'b', 'n', 'c'}
            };
            boardLayout = boardLayout.RotateArray();
            
            Board board = BoardFactory.CreateBoard(boardLayout);
            
            Vector2I castlePos = new Vector2I(0, 0);
            Assert.IsTrue(board.BoardPieces[castlePos.x, castlePos.y].PieceType == PieceType.Castle);
            
            Vector2I castleDest = new Vector2I(0, 4);
            Assert.IsTrue(board.BoardPieces[castleDest.x, castleDest.y].PieceType == PieceType.None);
            
            BoardPieceMove move = new BoardPieceMove(castlePos, castleDest);
            board.ApplyMove(move);

            Assert.IsTrue(board.BoardPieces[castlePos.x, castlePos.y].PieceType == PieceType.None);
            Assert.IsTrue(board.BoardPieces[castleDest.x, castleDest.y].PieceType == PieceType.Castle);
        }
        
        [Test]
        public void InvalidMove()
        {
            char[,] boardLayout =
            {
                {'C', 'N', 'B', 'Q', 'K', 'B', 'N', 'C'},
                {'P', 'P', 'P', 'P', 'P', 'P', 'P', 'P'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'}, 
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'}, 
                {'c', 'e', 'e', 'e', 'e', 'e', 'e', 'e'},
                {'e', 'p', 'p', 'p', 'p', 'p', 'p', 'p'},
                {'e', 'n', 'b', 'q', 'k', 'b', 'n', 'c'}
            };
            boardLayout = boardLayout.RotateArray();
            
            Board board = BoardFactory.CreateBoard(boardLayout);
            
            Vector2I castlePos = new Vector2I(0, 2);
            Assert.IsTrue(board.BoardPieces[castlePos.x, castlePos.y].PieceType == PieceType.Castle);
            
            Vector2I castleDest = new Vector2I(2, 4);
            Assert.IsTrue(board.BoardPieces[castleDest.x, castleDest.y].PieceType == PieceType.None);
            
            BoardPieceMove move = new BoardPieceMove(castlePos, castleDest);
            board.ApplyMove(move);

            Assert.IsTrue(board.BoardPieces[castlePos.x, castlePos.y].PieceType == PieceType.Castle);
            Assert.IsTrue(board.BoardPieces[castleDest.x, castleDest.y].PieceType == PieceType.None);
        }
    }
}