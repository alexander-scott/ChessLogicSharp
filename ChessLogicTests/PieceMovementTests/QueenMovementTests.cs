using ChessLogicSharp;
using ChessLogicSharp.ChessPlayers;
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
            BasicPlayer player1 = new BasicPlayer(board, Player.PlayerOne);
            board.AddPlayer(player1);
            BasicPlayer player2 = new BasicPlayer(board, Player.PlayerTwo);
            board.AddPlayer(player2);
            
            Vector2I queenPos = new Vector2I(3, 2);
            Assert.IsTrue(board.BoardPieces[queenPos.X, queenPos.Y].PieceType == PieceType.Queen);
            
            Vector2I queenDest = new Vector2I(6, 2);
            Assert.IsTrue(board.BoardPieces[queenDest.X, queenDest.Y].PieceType == PieceType.None);
            
            BoardPieceMove move = new BoardPieceMove(queenPos, queenDest);
            player1.ApplyMove(move);

            Assert.IsTrue(board.BoardPieces[queenPos.X, queenPos.Y].PieceType == PieceType.None);
            Assert.IsTrue(board.BoardPieces[queenDest.X, queenDest.Y].PieceType == PieceType.Queen);
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
            BasicPlayer player1 = new BasicPlayer(board, Player.PlayerOne);
            board.AddPlayer(player1);
            BasicPlayer player2 = new BasicPlayer(board, Player.PlayerTwo);
            board.AddPlayer(player2);
            
            Vector2I queenPos = new Vector2I(3, 0);
            Assert.IsTrue(board.BoardPieces[queenPos.X, queenPos.Y].PieceType == PieceType.Queen);
            
            Vector2I queenDest = new Vector2I(3, 5);
            Assert.IsTrue(board.BoardPieces[queenDest.X, queenDest.Y].PieceType == PieceType.None);
            
            BoardPieceMove move = new BoardPieceMove(queenPos, queenDest);
            player1.ApplyMove(move);

            Assert.IsTrue(board.BoardPieces[queenPos.X, queenPos.Y].PieceType == PieceType.None);
            Assert.IsTrue(board.BoardPieces[queenDest.X, queenDest.Y].PieceType == PieceType.Queen);
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
            BasicPlayer player1 = new BasicPlayer(board, Player.PlayerOne);
            board.AddPlayer(player1);
            BasicPlayer player2 = new BasicPlayer(board, Player.PlayerTwo);
            board.AddPlayer(player2);
            
            Vector2I castlePos = new Vector2I(3, 0);
            Assert.IsTrue(board.BoardPieces[castlePos.X, castlePos.Y].PieceType == PieceType.Queen);
            
            Vector2I castleDest = new Vector2I(6, 3);
            Assert.IsTrue(board.BoardPieces[castleDest.X, castleDest.Y].PieceType == PieceType.None);
            
            BoardPieceMove move = new BoardPieceMove(castlePos, castleDest);
            player1.ApplyMove(move);

            Assert.IsTrue(board.BoardPieces[castlePos.X, castlePos.Y].PieceType == PieceType.None);
            Assert.IsTrue(board.BoardPieces[castleDest.X, castleDest.Y].PieceType == PieceType.Queen);
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
            BasicPlayer player1 = new BasicPlayer(board, Player.PlayerOne);
            board.AddPlayer(player1);
            BasicPlayer player2 = new BasicPlayer(board, Player.PlayerTwo);
            board.AddPlayer(player2);
            
            Vector2I castlePos = new Vector2I(3, 0);
            Assert.IsTrue(board.BoardPieces[castlePos.X, castlePos.Y].PieceType == PieceType.Queen);
            
            Vector2I castleDest = new Vector2I(2, 2);
            Assert.IsTrue(board.BoardPieces[castleDest.X, castleDest.Y].PieceType == PieceType.None);
            
            BoardPieceMove move = new BoardPieceMove(castlePos, castleDest);
            Assert.IsFalse(player1.ApplyMove(move));

            Assert.IsTrue(board.BoardPieces[castlePos.X, castlePos.Y].PieceType == PieceType.Queen);
            Assert.IsTrue(board.BoardPieces[castleDest.X, castleDest.Y].PieceType == PieceType.None);
        }
    }
}