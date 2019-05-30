using ChessLogicSharp;
using ChessLogicSharp.ChessPlayers;
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
            BasicPlayer player1 = new BasicPlayer(board, Player.PlayerOne);
            board.AddPlayer(player1);
            BasicPlayer player2 = new BasicPlayer(board, Player.PlayerTwo);
            board.AddPlayer(player2);

            Vector2I knightPos = new Vector2I(1, 0);
            Assert.IsTrue(board.BoardPieces[knightPos.X, knightPos.Y].PieceType == PieceType.Knight);

            Vector2I knightDest = new Vector2I(2, 2);
            Assert.IsTrue(board.BoardPieces[knightDest.X, knightDest.Y].PieceType == PieceType.None);

            BoardPieceMove move = new BoardPieceMove(knightPos, knightDest);
            player1.ApplyMove(move);

            Assert.IsTrue(board.BoardPieces[knightPos.X, knightPos.Y].PieceType == PieceType.None);
            Assert.IsTrue(board.BoardPieces[knightDest.X, knightDest.Y].PieceType == PieceType.Knight);
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
            BasicPlayer player1 = new BasicPlayer(board, Player.PlayerOne);
            board.AddPlayer(player1);
            BasicPlayer player2 = new BasicPlayer(board, Player.PlayerTwo);
            board.AddPlayer(player2);
            
            Vector2I knightPos = new Vector2I(0, 2);
            Assert.IsTrue(board.BoardPieces[knightPos.X, knightPos.Y].PieceType == PieceType.Knight);
            
            Vector2I knightDest = new Vector2I(2, 3);
            Assert.IsTrue(board.BoardPieces[knightDest.X, knightDest.Y].PieceType == PieceType.None);
            
            BoardPieceMove move = new BoardPieceMove(knightPos, knightDest);
            player1.ApplyMove(move);

            Assert.IsTrue(board.BoardPieces[knightPos.X, knightPos.Y].PieceType == PieceType.None);
            Assert.IsTrue(board.BoardPieces[knightDest.X, knightDest.Y].PieceType == PieceType.Knight);
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
            BasicPlayer player1 = new BasicPlayer(board, Player.PlayerOne);
            board.AddPlayer(player1);
            BasicPlayer player2 = new BasicPlayer(board, Player.PlayerTwo);
            board.AddPlayer(player2);
            
            Vector2I knightPos = new Vector2I(0, 2);
            Assert.IsTrue(board.BoardPieces[knightPos.X, knightPos.Y].PieceType == PieceType.Knight);
            
            Vector2I knightDest = new Vector2I(1, 2);
            Assert.IsTrue(board.BoardPieces[knightDest.X, knightDest.Y].PieceType == PieceType.None);
            
            BoardPieceMove move = new BoardPieceMove(knightPos, knightDest);
            Assert.IsFalse(player1.ApplyMove(move));

            Assert.IsTrue(board.BoardPieces[knightPos.X, knightPos.Y].PieceType == PieceType.Knight);
            Assert.IsTrue(board.BoardPieces[knightDest.X, knightDest.Y].PieceType == PieceType.None);
        }
    }
}