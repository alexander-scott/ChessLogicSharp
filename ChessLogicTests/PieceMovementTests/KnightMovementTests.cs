using ChessLogicSharp;
using ChessLogicSharp.DataStructures;
using NUnit.Framework;

namespace ChessLogicTests.PieceMovementTests
{
    [TestFixture]
    public class KnightMovementTests
    {
        [Test]
        public void ValidMoveVertical()
        {
            Board board = BoardFactory.CreateBoard();

            Vector2I knightPos = new Vector2I(1, 0);
            Assert.IsTrue(board.BoardPieces[knightPos.x, knightPos.y].PieceType == PieceType.Knight);

            Vector2I knightDest = new Vector2I(2, 2);
            Assert.IsTrue(board.BoardPieces[knightDest.x, knightDest.y].PieceType == PieceType.None);

            BoardPieceMove move = new BoardPieceMove(knightPos, knightDest);
            board.ApplyMove(move);

            Assert.IsTrue(board.BoardPieces[knightPos.x, knightPos.y].PieceType == PieceType.None);
            Assert.IsTrue(board.BoardPieces[knightDest.x, knightDest.y].PieceType == PieceType.Knight);
        }
        
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
                {'n', 'e', 'e', 'e', 'e', 'e', 'e', 'e'}, // Knight movement
                {'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p'},
                {'c', 'e', 'b', 'q', 'k', 'b', 'n', 'c'}
            };
            boardLayout = boardLayout.RotateArray();
            
            Board board = BoardFactory.CreateBoard(boardLayout);
            
            Vector2I knightPos = new Vector2I(0, 2);
            Assert.IsTrue(board.BoardPieces[knightPos.x, knightPos.y].PieceType == PieceType.Knight);
            
            Vector2I knightDest = new Vector2I(2, 3);
            Assert.IsTrue(board.BoardPieces[knightDest.x, knightDest.y].PieceType == PieceType.None);
            
            BoardPieceMove move = new BoardPieceMove(knightPos, knightDest);
            board.ApplyMove(move);

            Assert.IsTrue(board.BoardPieces[knightPos.x, knightPos.y].PieceType == PieceType.None);
            Assert.IsTrue(board.BoardPieces[knightDest.x, knightDest.y].PieceType == PieceType.Knight);
        }

        [Test]
        public void InvalidMoveTest()
        {
            char[,] boardLayout =
            {
                {'C', 'N', 'B', 'Q', 'K', 'B', 'N', 'C'},
                {'P', 'P', 'P', 'P', 'P', 'P', 'P', 'P'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'}, 
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'}, 
                {'n', 'e', 'e', 'e', 'e', 'e', 'e', 'e'}, // Knight movement
                {'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p'},
                {'c', 'e', 'b', 'q', 'k', 'b', 'n', 'c'}
            };
            boardLayout = boardLayout.RotateArray();
            
            Board board = BoardFactory.CreateBoard(boardLayout);
            
            Vector2I knightPos = new Vector2I(0, 2);
            Assert.IsTrue(board.BoardPieces[knightPos.x, knightPos.y].PieceType == PieceType.Knight);
            
            Vector2I knightDest = new Vector2I(1, 2);
            Assert.IsTrue(board.BoardPieces[knightDest.x, knightDest.y].PieceType == PieceType.None);
            
            BoardPieceMove move = new BoardPieceMove(knightPos, knightDest);
            board.ApplyMove(move);

            Assert.IsTrue(board.BoardPieces[knightPos.x, knightPos.y].PieceType == PieceType.Knight);
            Assert.IsTrue(board.BoardPieces[knightDest.x, knightDest.y].PieceType == PieceType.None);
        }
    }
}