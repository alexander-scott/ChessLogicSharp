using ChessLogicSharp;
using ChessLogicSharp.ChessPlayers;
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
            BasicPlayer player1 = new BasicPlayer(board, Player.PlayerOne);
            board.AddPlayer(player1);
            BasicPlayer player2 = new BasicPlayer(board, Player.PlayerTwo);
            board.AddPlayer(player2);
            
            Vector2I bishopPos = new Vector2I(2, 0);
            Assert.IsTrue(board.BoardPieces[bishopPos.X, bishopPos.Y].PieceType == PieceType.Bishop);
            
            Vector2I bishopDest = new Vector2I(4, 2);
            Assert.IsTrue(board.BoardPieces[bishopDest.X, bishopDest.Y].PieceType == PieceType.None);
            
            BoardPieceMove move = new BoardPieceMove(bishopPos, bishopDest);
            player1.ApplyMove(move);

            Assert.IsTrue(board.BoardPieces[bishopPos.X, bishopPos.Y].PieceType == PieceType.None);
            Assert.IsTrue(board.BoardPieces[bishopDest.X, bishopDest.Y].PieceType == PieceType.Bishop);
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
            BasicPlayer player1 = new BasicPlayer(board, Player.PlayerOne);
            board.AddPlayer(player1);
            BasicPlayer player2 = new BasicPlayer(board, Player.PlayerTwo);
            board.AddPlayer(player2);
            
            Vector2I bishopPos = new Vector2I(5, 0);
            Assert.IsTrue(board.BoardPieces[bishopPos.X, bishopPos.Y].PieceType == PieceType.Bishop);
            
            Vector2I bishopDest = new Vector2I(3, 2);
            Assert.IsTrue(board.BoardPieces[bishopDest.X, bishopDest.Y].PieceType == PieceType.None);
            
            BoardPieceMove move = new BoardPieceMove(bishopPos, bishopDest);
            player1.ApplyMove(move);

            Assert.IsTrue(board.BoardPieces[bishopPos.X, bishopPos.Y].PieceType == PieceType.None);
            Assert.IsTrue(board.BoardPieces[bishopDest.X, bishopDest.Y].PieceType == PieceType.Bishop);
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
            BasicPlayer player1 = new BasicPlayer(board, Player.PlayerOne);
            board.AddPlayer(player1);
            BasicPlayer player2 = new BasicPlayer(board, Player.PlayerTwo);
            board.AddPlayer(player2);
            
            Vector2I bishopPos = new Vector2I(3, 2);
            Assert.IsTrue(board.BoardPieces[bishopPos.X, bishopPos.Y].PieceType == PieceType.Bishop);
            
            Vector2I bishopDest = new Vector2I(5, 0);
            Assert.IsTrue(board.BoardPieces[bishopDest.X, bishopDest.Y].PieceType == PieceType.None);
            
            BoardPieceMove move = new BoardPieceMove(bishopPos, bishopDest);
            player1.ApplyMove(move);

            Assert.IsTrue(board.BoardPieces[bishopPos.X, bishopPos.Y].PieceType == PieceType.None);
            Assert.IsTrue(board.BoardPieces[bishopDest.X, bishopDest.Y].PieceType == PieceType.Bishop);
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
            BasicPlayer player1 = new BasicPlayer(board, Player.PlayerOne);
            board.AddPlayer(player1);
            BasicPlayer player2 = new BasicPlayer(board, Player.PlayerTwo);
            board.AddPlayer(player2);
            
            Vector2I bishopPos = new Vector2I(2, 0);
            Assert.IsTrue(board.BoardPieces[bishopPos.X, bishopPos.Y].PieceType == PieceType.Bishop);
            
            Vector2I bishopDest = new Vector2I(2, 3);
            Assert.IsTrue(board.BoardPieces[bishopDest.X, bishopDest.Y].PieceType == PieceType.None);
            
            BoardPieceMove move = new BoardPieceMove(bishopPos, bishopDest);
            Assert.IsFalse(player1.ApplyMove(move));

            Assert.IsTrue(board.BoardPieces[bishopPos.X, bishopPos.Y].PieceType == PieceType.Bishop);
            Assert.IsTrue(board.BoardPieces[bishopDest.X, bishopDest.Y].PieceType == PieceType.None);
        }
    }
}