using ChessLogicSharp;
using ChessLogicSharp.DataStructures;
using NUnit.Framework;

namespace ChessLogicTests.PieceMovementTests
{
    [TestFixture]
    public class PawnMovementTests
    {
        /// <summary>
        /// Test the pawn can move 1 space from the start position
        /// </summary>
        [Test]
        public void PawnMoveUpSinglePieceTest()
        {
            Board board = BoardFactory.CreateBoard();

            Vector2I pawnPos = new Vector2I(3, 1);
            Assert.IsTrue(board.BoardPieces[pawnPos.x, pawnPos.y].PieceType == PieceType.Pawn);

            Vector2I pawnDest = new Vector2I(3, 2);
            Assert.IsTrue(board.BoardPieces[pawnDest.x, pawnDest.y].PieceType == PieceType.None);

            BoardPieceMove move = new BoardPieceMove(pawnPos, pawnDest);
            board.ApplyMove(move);

            Assert.IsTrue(board.BoardPieces[pawnPos.x, pawnPos.y].PieceType == PieceType.None);
            Assert.IsTrue(board.BoardPieces[pawnDest.x, pawnDest.y].PieceType == PieceType.Pawn);
        }

        /// <summary>
        /// Test the pawn can move 2 spaces from the start position
        /// </summary>
        [Test]
        public void PawnMoveUpDoublePieceTest()
        {
            Board board = BoardFactory.CreateBoard();

            Vector2I pawnPos = new Vector2I(3, 1);
            Assert.IsTrue(board.BoardPieces[pawnPos.x, pawnPos.y].PieceType == PieceType.Pawn);

            Vector2I pawnDest = new Vector2I(3, 3);
            Assert.IsTrue(board.BoardPieces[pawnDest.x, pawnDest.y].PieceType == PieceType.None);

            BoardPieceMove move = new BoardPieceMove(pawnPos, pawnDest);
            board.ApplyMove(move);

            Assert.IsTrue(board.BoardPieces[pawnPos.x, pawnPos.y].PieceType == PieceType.None);
            Assert.IsTrue(board.BoardPieces[pawnDest.x, pawnDest.y].PieceType == PieceType.Pawn);
        }

        /// <summary>
        /// Test the pawn can't move 3 spaces 
        /// </summary>
        [Test]
        public void PawnInvalidMovePieceTest()
        {
            Board board = BoardFactory.CreateBoard();

            Vector2I pawnPos = new Vector2I(3, 1);
            Assert.IsTrue(board.BoardPieces[pawnPos.x, pawnPos.y].PieceType == PieceType.Pawn);

            Vector2I pawnDest = new Vector2I(3, 4);
            Assert.IsTrue(board.BoardPieces[pawnDest.x, pawnDest.y].PieceType == PieceType.None);

            BoardPieceMove move = new BoardPieceMove(pawnPos, pawnDest);
            board.ApplyMove(move);

            Assert.IsTrue(board.BoardPieces[pawnPos.x, pawnPos.y].PieceType == PieceType.Pawn);
            Assert.IsTrue(board.BoardPieces[pawnDest.x, pawnDest.y].PieceType == PieceType.None);
        }

        /// <summary>
        /// Test the pawn can't move 2 spaces after already moving
        /// </summary>
        [Test]
        public void PawnInvalidMovePieceTest2()
        {
            char[,] boardLayout =
            {
                {'C', 'N', 'B', 'Q', 'K', 'B', 'N', 'C'},
                {'P', 'P', 'P', 'P', 'P', 'P', 'P', 'P'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'},
                {'e', 'e', 'e', 'p', 'e', 'e', 'e', 'e'}, // Pawn has already moved
                {'p', 'p', 'p', 'e', 'p', 'p', 'p', 'p'},
                {'c', 'n', 'b', 'q', 'k', 'b', 'n', 'c'}
            };
            boardLayout = boardLayout.RotateArray();

            Board board = BoardFactory.CreateBoard(boardLayout);
            var pawnPos = new Vector2I(3, 2);
            Assert.IsTrue(board.BoardPieces[pawnPos.x, pawnPos.y].PieceType == PieceType.Pawn);

            var pawnDest = new Vector2I(3, 4);
            Assert.IsTrue(board.BoardPieces[pawnDest.x, pawnDest.y].PieceType == PieceType.None);

            var move = new BoardPieceMove(pawnPos, pawnDest);
            board.ApplyMove(move);

            Assert.IsTrue(board.BoardPieces[pawnPos.x, pawnPos.y].PieceType == PieceType.Pawn);
            Assert.IsTrue(board.BoardPieces[pawnDest.x, pawnDest.y].PieceType == PieceType.None);
        }

        [Test]
        public void PieceTakenTest()
        {
            char[,] boardLayout =
            {
                {'C', 'N', 'B', 'Q', 'K', 'B', 'N', 'C'},
                {'P', 'P', 'P', 'P', 'e', 'P', 'P', 'P'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'},
                {'e', 'e', 'e', 'e', 'P', 'e', 'e', 'e'},
                {'e', 'e', 'e', 'p', 'e', 'e', 'e', 'e'}, // Pawn take pawn
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'},
                {'p', 'p', 'p', 'e', 'p', 'p', 'p', 'p'},
                {'c', 'n', 'b', 'q', 'k', 'b', 'n', 'c'}
            };
            boardLayout = boardLayout.RotateArray();

            Board board = BoardFactory.CreateBoard(boardLayout);
            var pawnPos = new Vector2I(3, 3);
            Assert.IsTrue(board.BoardPieces[pawnPos.x, pawnPos.y].PieceType == PieceType.Pawn && board.BoardPieces[pawnPos.x, pawnPos.y].PieceOwner == Player.PlayerOne);

            var pawnDest = new Vector2I(4, 4);
            Assert.IsTrue(board.BoardPieces[pawnDest.x, pawnDest.y].PieceType == PieceType.Pawn && board.BoardPieces[pawnDest.x, pawnDest.y].PieceOwner == Player.PlayerTwo);

            var move = new BoardPieceMove(pawnPos, pawnDest);
            board.ApplyMove(move);

            Assert.IsTrue(board.BoardPieces[pawnPos.x, pawnPos.y].PieceType == PieceType.None);
            Assert.IsTrue(board.BoardPieces[pawnDest.x, pawnDest.y].PieceType == PieceType.Pawn && board.BoardPieces[pawnDest.x, pawnDest.y].PieceOwner == Player.PlayerOne);
        }

        [Test]
        public void PieceTakenEnPassantTest()
        {
            char[,] boardLayout =
            {
                {'C', 'N', 'B', 'Q', 'K', 'B', 'N', 'C'},
                {'P', 'P', 'P', 'P', 'P', 'P', 'P', 'P'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'},
                {'e', 'e', 'e', 'p', 'e', 'e', 'e', 'e'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'}, // Pawn take pawn
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'},
                {'p', 'p', 'p', 'e', 'p', 'p', 'p', 'p'},
                {'c', 'n', 'b', 'q', 'k', 'b', 'n', 'c'}
            };
            boardLayout = boardLayout.RotateArray();

            Board board = BoardFactory.CreateBoard(boardLayout);
            board.PlayerTurn = Player.PlayerTwo;
            
            var pawnPos = new Vector2I(4, 6); // Move enemy pawn
            var pawnDest = new Vector2I(4, 4);
            var move = new BoardPieceMove(pawnPos, pawnDest);
            board.ApplyMove(move);
            
            pawnPos = new Vector2I(3, 4); // En passant take
            pawnDest = new Vector2I(4, 5);
            move = new BoardPieceMove(pawnPos, pawnDest);
            board.ApplyMove(move);

            Assert.IsTrue(board.BoardPieces[pawnPos.x, pawnPos.y].PieceType == PieceType.None);
            Assert.IsTrue(board.BoardPieces[pawnDest.x, pawnDest.y].PieceType == PieceType.Pawn && board.BoardPieces[pawnDest.x, pawnDest.y].PieceOwner == Player.PlayerOne);
            Assert.IsTrue(board.BoardPieces[4, 4].PieceType == PieceType.None);
        }
    }
}