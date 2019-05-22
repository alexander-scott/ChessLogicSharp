using ChessLogicSharp;
using ChessLogicSharp.DataStructures;
using NUnit.Framework;

namespace ChessLogicTests.PieceMovementTests
{
    [TestFixture]
    public class BishopMovementTests
    {
        [Test]
        public void ValidMovementRightDiagonal()
        {
            char[,] boardLayout =
            {
                {'C', 'N', 'B', 'Q', 'K', 'B', 'N', 'C'},
                {'P', 'P', 'P', 'P', 'P', 'P', 'P', 'P'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'}, 
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'}, 
                {'e', 'e', 'e', 'p', 'e', 'e', 'e', 'e'}, 
                {'p', 'p', 'p', 'e', 'p', 'p', 'p', 'p'},
                {'c', 'n', 'b', 'q', 'k', 'b', 'n', 'c'} // Bishop movement
            };
            boardLayout = boardLayout.RotateArray();
            
            Board board = BoardFactory.CreateBoard(boardLayout);
            
            Vector2I bishopPos = new Vector2I(2, 0);
            Assert.IsTrue(board.BoardPieces[bishopPos.x, bishopPos.y].PieceType == PieceType.Bishop);
            
            Vector2I bishopDest = new Vector2I(4, 2);
            Assert.IsTrue(board.BoardPieces[bishopDest.x, bishopDest.y].PieceType == PieceType.None);
            
            BoardPieceMove move = new BoardPieceMove(bishopPos, bishopDest);
            board.ApplyMove(move);

            Assert.IsTrue(board.BoardPieces[bishopPos.x, bishopPos.y].PieceType == PieceType.None);
            Assert.IsTrue(board.BoardPieces[bishopDest.x, bishopDest.y].PieceType == PieceType.Bishop);
        }
        
        [Test]
        public void ValidMovementLeftDiagonal()
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
                {'c', 'n', 'b', 'q', 'k', 'b', 'n', 'c'} // Bishop movement
            };
            boardLayout = boardLayout.RotateArray();
            
            Board board = BoardFactory.CreateBoard(boardLayout);
            
            Vector2I bishopPos = new Vector2I(5, 0);
            Assert.IsTrue(board.BoardPieces[bishopPos.x, bishopPos.y].PieceType == PieceType.Bishop);
            
            Vector2I bishopDest = new Vector2I(3, 2);
            Assert.IsTrue(board.BoardPieces[bishopDest.x, bishopDest.y].PieceType == PieceType.None);
            
            BoardPieceMove move = new BoardPieceMove(bishopPos, bishopDest);
            board.ApplyMove(move);

            Assert.IsTrue(board.BoardPieces[bishopPos.x, bishopPos.y].PieceType == PieceType.None);
            Assert.IsTrue(board.BoardPieces[bishopDest.x, bishopDest.y].PieceType == PieceType.Bishop);
        }
        
        [Test]
        public void ValidMovementLeftDiagonalBackwards()
        {
            char[,] boardLayout =
            {
                {'C', 'N', 'B', 'Q', 'K', 'B', 'N', 'C'},
                {'P', 'P', 'P', 'P', 'P', 'P', 'P', 'P'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'}, 
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'}, 
                {'e', 'e', 'e', 'b', 'p', 'e', 'e', 'e'}, 
                {'p', 'p', 'p', 'p', 'e', 'p', 'p', 'p'},
                {'c', 'n', 'b', 'q', 'k', 'e', 'n', 'c'} // Bishop movement
            };
            boardLayout = boardLayout.RotateArray();
            
            Board board = BoardFactory.CreateBoard(boardLayout);
            
            Vector2I bishopPos = new Vector2I(3, 2);
            Assert.IsTrue(board.BoardPieces[bishopPos.x, bishopPos.y].PieceType == PieceType.Bishop);
            
            Vector2I bishopDest = new Vector2I(5, 0);
            Assert.IsTrue(board.BoardPieces[bishopDest.x, bishopDest.y].PieceType == PieceType.None);
            
            BoardPieceMove move = new BoardPieceMove(bishopPos, bishopDest);
            board.ApplyMove(move);

            Assert.IsTrue(board.BoardPieces[bishopPos.x, bishopPos.y].PieceType == PieceType.None);
            Assert.IsTrue(board.BoardPieces[bishopDest.x, bishopDest.y].PieceType == PieceType.Bishop);
        }
        
        [Test]
        public void InvalidMoveVertical()
        {
            char[,] boardLayout =
            {
                {'C', 'N', 'B', 'Q', 'K', 'B', 'N', 'C'},
                {'P', 'P', 'P', 'P', 'P', 'P', 'P', 'P'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'}, 
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'}, 
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'}, 
                {'p', 'p', 'e', 'p', 'p', 'p', 'p', 'p'},
                {'c', 'n', 'b', 'q', 'k', 'b', 'n', 'c'} // Bishop movement
            };
            boardLayout = boardLayout.RotateArray();
            
            Board board = BoardFactory.CreateBoard(boardLayout);
            
            Vector2I bishopPos = new Vector2I(2, 0);
            Assert.IsTrue(board.BoardPieces[bishopPos.x, bishopPos.y].PieceType == PieceType.Bishop);
            
            Vector2I bishopDest = new Vector2I(2, 3);
            Assert.IsTrue(board.BoardPieces[bishopDest.x, bishopDest.y].PieceType == PieceType.None);
            
            BoardPieceMove move = new BoardPieceMove(bishopPos, bishopDest);
            board.ApplyMove(move);

            Assert.IsTrue(board.BoardPieces[bishopPos.x, bishopPos.y].PieceType == PieceType.Bishop);
            Assert.IsTrue(board.BoardPieces[bishopDest.x, bishopDest.y].PieceType == PieceType.None);
        }
    }
}