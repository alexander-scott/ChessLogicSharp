using ChessLogicSharp;
using ChessLogicSharp.DataStructures;
using NUnit.Framework;

namespace ChessLogicTests.PieceMovementTests
{
    [TestFixture]
    public class KingMovementTests
    {
        [Test]
        public void KingMovementTest()
        {
            char[,] boardLayout =
            {
                {'C', 'N', 'B', 'Q', 'K', 'B', 'N', 'C'},
                {'P', 'P', 'P', 'P', 'P', 'P', 'P', 'P'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'}, 
                {'e', 'e', 'e', 'e', 'k', 'e', 'e', 'e'}, 
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'}, 
                {'p', 'p', 'p', 'p', 'e', 'p', 'p', 'p'},
                {'c', 'n', 'b', 'q', 'e', 'b', 'n', 'c'}
            };
            boardLayout = boardLayout.RotateArray();
            
            Board board = BoardFactory.CreateBoard(boardLayout);
            
            // Move up
            Vector2I kingPos = new Vector2I(4, 3);
            Assert.IsTrue(board.BoardPieces[kingPos.x, kingPos.y].PieceType == PieceType.King);
            
            Vector2I kingDest = new Vector2I(4, 4);
            Assert.IsTrue(board.BoardPieces[kingDest.x, kingDest.y].PieceType == PieceType.None);
            
            BoardPieceMove move = new BoardPieceMove(kingPos, kingDest);
            board.ApplyMove(move);
            
            Assert.IsTrue(board.BoardPieces[kingPos.x, kingPos.y].PieceType == PieceType.None);
            Assert.IsTrue(board.BoardPieces[kingDest.x, kingDest.y].PieceType == PieceType.King);
            
            // Move left
            board.PlayerTurn = Player.PlayerOne;
            kingPos = kingDest;
            Assert.IsTrue(board.BoardPieces[kingPos.x, kingPos.y].PieceType == PieceType.King);
            
            kingDest = new Vector2I(3, 4);
            Assert.IsTrue(board.BoardPieces[kingDest.x, kingDest.y].PieceType == PieceType.None);
            
            move = new BoardPieceMove(kingPos, kingDest);
            board.ApplyMove(move);
            
            Assert.IsTrue(board.BoardPieces[kingPos.x, kingPos.y].PieceType == PieceType.None);
            Assert.IsTrue(board.BoardPieces[kingDest.x, kingDest.y].PieceType == PieceType.King);
            
            // Move down
            board.PlayerTurn = Player.PlayerOne;
            kingPos = kingDest;
            Assert.IsTrue(board.BoardPieces[kingPos.x, kingPos.y].PieceType == PieceType.King);
            
            kingDest = new Vector2I(3, 3);
            Assert.IsTrue(board.BoardPieces[kingDest.x, kingDest.y].PieceType == PieceType.None);
            
            move = new BoardPieceMove(kingPos, kingDest);
            board.ApplyMove(move);
            
            Assert.IsTrue(board.BoardPieces[kingPos.x, kingPos.y].PieceType == PieceType.None);
            Assert.IsTrue(board.BoardPieces[kingDest.x, kingDest.y].PieceType == PieceType.King);
            
            // Move right
            board.PlayerTurn = Player.PlayerOne;
            kingPos = kingDest;
            Assert.IsTrue(board.BoardPieces[kingPos.x, kingPos.y].PieceType == PieceType.King);
            
            kingDest = new Vector2I(4, 3);
            Assert.IsTrue(board.BoardPieces[kingDest.x, kingDest.y].PieceType == PieceType.None);
            
            move = new BoardPieceMove(kingPos, kingDest);
            board.ApplyMove(move);
            
            Assert.IsTrue(board.BoardPieces[kingPos.x, kingPos.y].PieceType == PieceType.None);
            Assert.IsTrue(board.BoardPieces[kingDest.x, kingDest.y].PieceType == PieceType.King);
        }
        
        [Test]
        public void KingDiagonalMovementTest()
        {
            char[,] boardLayout =
            {
                {'C', 'N', 'B', 'Q', 'K', 'B', 'N', 'C'},
                {'P', 'P', 'P', 'P', 'P', 'P', 'P', 'P'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'}, 
                {'e', 'e', 'e', 'e', 'k', 'e', 'e', 'e'}, 
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'}, 
                {'p', 'p', 'p', 'p', 'e', 'p', 'p', 'p'},
                {'c', 'n', 'b', 'q', 'e', 'b', 'n', 'c'}
            };
            boardLayout = boardLayout.RotateArray();
            
            Board board = BoardFactory.CreateBoard(boardLayout);
            
            // Move up-right
            Vector2I kingPos = new Vector2I(4, 3);
            Assert.IsTrue(board.BoardPieces[kingPos.x, kingPos.y].PieceType == PieceType.King);
            
            Vector2I kingDest = new Vector2I(5, 4);
            Assert.IsTrue(board.BoardPieces[kingDest.x, kingDest.y].PieceType == PieceType.None);
            
            BoardPieceMove move = new BoardPieceMove(kingPos, kingDest);
            board.ApplyMove(move);
            
            Assert.IsTrue(board.BoardPieces[kingPos.x, kingPos.y].PieceType == PieceType.None);
            Assert.IsTrue(board.BoardPieces[kingDest.x, kingDest.y].PieceType == PieceType.King);
            
            // Move down-right
            board.PlayerTurn = Player.PlayerOne;
            kingPos = kingDest;
            Assert.IsTrue(board.BoardPieces[kingPos.x, kingPos.y].PieceType == PieceType.King);
            
            kingDest = new Vector2I(6, 3);
            Assert.IsTrue(board.BoardPieces[kingDest.x, kingDest.y].PieceType == PieceType.None);
            
            move = new BoardPieceMove(kingPos, kingDest);
            board.ApplyMove(move);
            
            Assert.IsTrue(board.BoardPieces[kingPos.x, kingPos.y].PieceType == PieceType.None);
            Assert.IsTrue(board.BoardPieces[kingDest.x, kingDest.y].PieceType == PieceType.King);
            
            // Move down-left
            board.PlayerTurn = Player.PlayerOne;
            kingPos = kingDest;
            Assert.IsTrue(board.BoardPieces[kingPos.x, kingPos.y].PieceType == PieceType.King);
            
            kingDest = new Vector2I(5, 2);
            Assert.IsTrue(board.BoardPieces[kingDest.x, kingDest.y].PieceType == PieceType.None);
            
            move = new BoardPieceMove(kingPos, kingDest);
            board.ApplyMove(move);
            
            Assert.IsTrue(board.BoardPieces[kingPos.x, kingPos.y].PieceType == PieceType.None);
            Assert.IsTrue(board.BoardPieces[kingDest.x, kingDest.y].PieceType == PieceType.King);
            
            // Move up-left
            board.PlayerTurn = Player.PlayerOne;
            kingPos = kingDest;
            Assert.IsTrue(board.BoardPieces[kingPos.x, kingPos.y].PieceType == PieceType.King);
            
            kingDest = new Vector2I(4, 3);
            Assert.IsTrue(board.BoardPieces[kingDest.x, kingDest.y].PieceType == PieceType.None);
            
            move = new BoardPieceMove(kingPos, kingDest);
            board.ApplyMove(move);
            
            Assert.IsTrue(board.BoardPieces[kingPos.x, kingPos.y].PieceType == PieceType.None);
            Assert.IsTrue(board.BoardPieces[kingDest.x, kingDest.y].PieceType == PieceType.King);
        }

        [Test]
        public void InvalidKingMovementTest()
        {
            char[,] boardLayout =
            {
                {'C', 'N', 'B', 'Q', 'K', 'B', 'N', 'C'},
                {'P', 'P', 'P', 'P', 'P', 'P', 'P', 'P'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'}, 
                {'e', 'e', 'e', 'e', 'k', 'e', 'e', 'e'}, 
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'}, 
                {'p', 'p', 'p', 'p', 'e', 'p', 'p', 'p'},
                {'c', 'n', 'b', 'q', 'e', 'b', 'n', 'c'}
            };
            boardLayout = boardLayout.RotateArray();
            
            Board board = BoardFactory.CreateBoard(boardLayout);
            
            // Move right twice
            Vector2I kingPos = new Vector2I(4, 3);
            Assert.IsTrue(board.BoardPieces[kingPos.x, kingPos.y].PieceType == PieceType.King);
            
            Vector2I kingDest = new Vector2I(6, 3);
            Assert.IsTrue(board.BoardPieces[kingDest.x, kingDest.y].PieceType == PieceType.None);
            
            BoardPieceMove move = new BoardPieceMove(kingPos, kingDest);
            board.ApplyMove(move);
            
            Assert.IsTrue(board.BoardPieces[kingPos.x, kingPos.y].PieceType == PieceType.King);
            Assert.IsTrue(board.BoardPieces[kingDest.x, kingDest.y].PieceType == PieceType.None);
        }

        [Test]
        public void CastlingTest()
        {
            char[,] boardLayout =
            {
                {'C', 'N', 'B', 'Q', 'K', 'B', 'N', 'C'},
                {'P', 'P', 'P', 'P', 'P', 'P', 'P', 'P'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'}, 
                {'e', 'e', 'e', 'e', 'e', 'p', 'p', 'e'}, 
                {'e', 'e', 'e', 'e', 'e', 'n', 'e', 'b'}, 
                {'p', 'p', 'p', 'p', 'p', 'e', 'e', 'p'},
                {'c', 'n', 'b', 'q', 'k', 'e', 'e', 'c'}
            };
            boardLayout = boardLayout.RotateArray();
            
            Board board = BoardFactory.CreateBoard(boardLayout);
            
            // Move right twice
            Vector2I kingPos = new Vector2I(4, 0);
            Assert.IsTrue(board.BoardPieces[kingPos.x, kingPos.y].PieceType == PieceType.King);
            
            Vector2I kingDest = new Vector2I(6, 0);
            Assert.IsTrue(board.BoardPieces[kingDest.x, kingDest.y].PieceType == PieceType.None);
            
            BoardPieceMove move = new BoardPieceMove(kingPos, kingDest);
            board.ApplyMove(move);
            
            Assert.IsTrue(board.BoardPieces[kingPos.x, kingPos.y].PieceType == PieceType.None);
            Assert.IsTrue(board.BoardPieces[kingDest.x, kingDest.y].PieceType == PieceType.King);
            Assert.IsTrue(board.BoardPieces[kingDest.x - 1, kingDest.y].PieceType == PieceType.Castle);
        }

        [Test]
        public void InvalidCastlingTest()
        {
            char[,] boardLayout =
            {
                {'C', 'N', 'B', 'Q', 'K', 'B', 'N', 'C'},
                {'P', 'P', 'P', 'P', 'P', 'P', 'P', 'P'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'}, 
                {'e', 'e', 'e', 'e', 'e', 'p', 'p', 'e'}, 
                {'e', 'e', 'e', 'e', 'e', 'n', 'e', 'b'}, 
                {'p', 'p', 'p', 'p', 'p', 'e', 'e', 'c'},
                {'c', 'n', 'b', 'q', 'k', 'e', 'e', 'e'}
            };
            boardLayout = boardLayout.RotateArray();
            
            Board board = BoardFactory.CreateBoard(boardLayout);
            
            // Castle moves, can no longer perform castling
            Vector2I castlePos = new Vector2I(7, 1);
            Assert.IsTrue(board.BoardPieces[castlePos.x, castlePos.y].PieceType == PieceType.Castle);
            
            Vector2I castleDest = new Vector2I(7, 0);
            Assert.IsTrue(board.BoardPieces[castleDest.x, castleDest.y].PieceType == PieceType.None);
            
            BoardPieceMove move = new BoardPieceMove(castlePos, castleDest);
            board.ApplyMove(move);
            
            Assert.IsTrue(board.BoardPieces[castlePos.x, castlePos.y].PieceType == PieceType.None);
            Assert.IsTrue(board.BoardPieces[castleDest.x, castleDest.y].PieceType == PieceType.Castle);

            board.PlayerTurn = Player.PlayerOne;
            
            Vector2I kingPos = new Vector2I(4, 0);
            Assert.IsTrue(board.BoardPieces[kingPos.x, kingPos.y].PieceType == PieceType.King);
            
            Vector2I kingDest = new Vector2I(6, 0);
            Assert.IsTrue(board.BoardPieces[kingDest.x, kingDest.y].PieceType == PieceType.None);
            
            move = new BoardPieceMove(kingPos, kingDest);
            board.ApplyMove(move);
            
            Assert.IsTrue(board.BoardPieces[kingPos.x, kingPos.y].PieceType == PieceType.King);
            Assert.IsTrue(board.BoardPieces[kingDest.x, kingDest.y].PieceType == PieceType.None);
            Assert.IsTrue(board.BoardPieces[7, 0].PieceType == PieceType.Castle);
        }
    }
}