using ChessLogicSharp;
using ChessLogicSharp.DataStructures;
using NUnit.Framework;

namespace ChessLogicTests.PieceMovementTests
{
    [TestFixture]
    public class QueenMovementTests
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
                {'e', 'e', 'e', 'p', 'e', 'e', 'e', 'e'}, 
                {'e', 'e', 'e', 'q', 'e', 'e', 'e', 'e'}, 
                {'p', 'p', 'p', 'e', 'p', 'p', 'p', 'p'},
                {'c', 'n', 'b', 'e', 'k', 'b', 'n', 'c'}
            };
            boardLayout = boardLayout.RotateArray();
            
            Board board = BoardFactory.CreateBoard(boardLayout);
            
            Vector2I queenPos = new Vector2I(3, 2);
            Assert.IsTrue(board.BoardPieces[queenPos.x, queenPos.y].PieceType == PieceType.Queen);
            
            Vector2I queenDest = new Vector2I(6, 2);
            Assert.IsTrue(board.BoardPieces[queenDest.x, queenDest.y].PieceType == PieceType.None);
            
            BoardPieceMove move = new BoardPieceMove(queenPos, queenDest);
            board.ApplyMove(move);

            Assert.IsTrue(board.BoardPieces[queenPos.x, queenPos.y].PieceType == PieceType.None);
            Assert.IsTrue(board.BoardPieces[queenDest.x, queenDest.y].PieceType == PieceType.Queen);
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
                {'p', 'p', 'p', 'e', 'p', 'p', 'p', 'p'},
                {'c', 'n', 'b', 'q', 'k', 'b', 'n', 'c'}
            };
            boardLayout = boardLayout.RotateArray();
            
            Board board = BoardFactory.CreateBoard(boardLayout);
            
            Vector2I queenPos = new Vector2I(3, 0);
            Assert.IsTrue(board.BoardPieces[queenPos.x, queenPos.y].PieceType == PieceType.Queen);
            
            Vector2I queenDest = new Vector2I(3, 5);
            Assert.IsTrue(board.BoardPieces[queenDest.x, queenDest.y].PieceType == PieceType.None);
            
            BoardPieceMove move = new BoardPieceMove(queenPos, queenDest);
            board.ApplyMove(move);

            Assert.IsTrue(board.BoardPieces[queenPos.x, queenPos.y].PieceType == PieceType.None);
            Assert.IsTrue(board.BoardPieces[queenDest.x, queenDest.y].PieceType == PieceType.Queen);
        }
        
        [Test]
        public void ValidMoveDiagonal()
        {
            char[,] boardLayout =
            {
                {'C', 'N', 'B', 'Q', 'K', 'B', 'N', 'C'},
                {'P', 'P', 'P', 'P', 'P', 'P', 'P', 'P'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'}, 
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'}, 
                {'e', 'e', 'e', 'e', 'p', 'e', 'e', 'e'}, 
                {'p', 'p', 'p', 'p', 'e', 'p', 'p', 'p'},
                {'c', 'n', 'b', 'q', 'k', 'b', 'n', 'c'}
            };
            boardLayout = boardLayout.RotateArray();
            
            Board board = BoardFactory.CreateBoard(boardLayout);
            
            Vector2I castlePos = new Vector2I(3, 0);
            Assert.IsTrue(board.BoardPieces[castlePos.x, castlePos.y].PieceType == PieceType.Queen);
            
            Vector2I castleDest = new Vector2I(6, 3);
            Assert.IsTrue(board.BoardPieces[castleDest.x, castleDest.y].PieceType == PieceType.None);
            
            BoardPieceMove move = new BoardPieceMove(castlePos, castleDest);
            board.ApplyMove(move);

            Assert.IsTrue(board.BoardPieces[castlePos.x, castlePos.y].PieceType == PieceType.None);
            Assert.IsTrue(board.BoardPieces[castleDest.x, castleDest.y].PieceType == PieceType.Queen);
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
                {'e', 'e', 'e', 'e', 'p', 'e', 'e', 'e'}, 
                {'p', 'p', 'p', 'p', 'e', 'p', 'p', 'p'},
                {'c', 'n', 'b', 'q', 'k', 'b', 'n', 'c'}
            };
            boardLayout = boardLayout.RotateArray();
            
            Board board = BoardFactory.CreateBoard(boardLayout);
            
            Vector2I castlePos = new Vector2I(3, 0);
            Assert.IsTrue(board.BoardPieces[castlePos.x, castlePos.y].PieceType == PieceType.Queen);
            
            Vector2I castleDest = new Vector2I(2, 2);
            Assert.IsTrue(board.BoardPieces[castleDest.x, castleDest.y].PieceType == PieceType.None);
            
            BoardPieceMove move = new BoardPieceMove(castlePos, castleDest);
            board.ApplyMove(move);

            Assert.IsTrue(board.BoardPieces[castlePos.x, castlePos.y].PieceType == PieceType.Queen);
            Assert.IsTrue(board.BoardPieces[castleDest.x, castleDest.y].PieceType == PieceType.None);
        }
    }
}