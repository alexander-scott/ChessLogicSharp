using ChessLogicSharp;
using ChessLogicSharp.ChessPlayers;
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
            BasicPlayer player1 = new BasicPlayer(board, Player.PlayerOne);
            board.AddPlayer(player1);
            BasicPlayer player2 = new BasicPlayer(board, Player.PlayerTwo);
            board.AddPlayer(player2);
            
            Vector2I castlePos = new Vector2I(0, 2);
            Assert.IsTrue(board.BoardPieces[castlePos.X, castlePos.Y].PieceType == PieceType.Castle);
            
            Vector2I castleDest = new Vector2I(7, 2);
            Assert.IsTrue(board.BoardPieces[castleDest.X, castleDest.Y].PieceType == PieceType.None);
            
            BoardPieceMove move = new BoardPieceMove(castlePos, castleDest);
            player1.ApplyMove(move);

            Assert.IsTrue(board.BoardPieces[castlePos.X, castlePos.Y].PieceType == PieceType.None);
            Assert.IsTrue(board.BoardPieces[castleDest.X, castleDest.Y].PieceType == PieceType.Castle);
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
            BasicPlayer player1 = new BasicPlayer(board, Player.PlayerOne);
            board.AddPlayer(player1);
            BasicPlayer player2 = new BasicPlayer(board, Player.PlayerTwo);
            board.AddPlayer(player2);
            
            Vector2I castlePos = new Vector2I(0, 0);
            Assert.IsTrue(board.BoardPieces[castlePos.X, castlePos.Y].PieceType == PieceType.Castle);
            
            Vector2I castleDest = new Vector2I(0, 4);
            Assert.IsTrue(board.BoardPieces[castleDest.X, castleDest.Y].PieceType == PieceType.None);
            
            BoardPieceMove move = new BoardPieceMove(castlePos, castleDest);
            player1.ApplyMove(move);

            Assert.IsTrue(board.BoardPieces[castlePos.X, castlePos.Y].PieceType == PieceType.None);
            Assert.IsTrue(board.BoardPieces[castleDest.X, castleDest.Y].PieceType == PieceType.Castle);
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
            BasicPlayer player1 = new BasicPlayer(board, Player.PlayerOne);
            board.AddPlayer(player1);
            BasicPlayer player2 = new BasicPlayer(board, Player.PlayerTwo);
            board.AddPlayer(player2);
            
            Vector2I castlePos = new Vector2I(0, 2);
            Assert.IsTrue(board.BoardPieces[castlePos.X, castlePos.Y].PieceType == PieceType.Castle);
            
            Vector2I castleDest = new Vector2I(2, 4);
            Assert.IsTrue(board.BoardPieces[castleDest.X, castleDest.Y].PieceType == PieceType.None);
            
            BoardPieceMove move = new BoardPieceMove(castlePos, castleDest);
            Assert.IsFalse(player1.ApplyMove(move));

            Assert.IsTrue(board.BoardPieces[castlePos.X, castlePos.Y].PieceType == PieceType.Castle);
            Assert.IsTrue(board.BoardPieces[castleDest.X, castleDest.Y].PieceType == PieceType.None);
        }
    }
}