using System.Collections.Generic;
using ChessLogicSharp;
using ChessLogicSharp.DataStructures;
using NUnit.Framework;

namespace ChessLogicTests
{
    [TestFixture]
    public class BoardTests
    {
        [Test]
        public void CheckDefaultPositions()
        {
            Board board = BoardFactory.CreateBoard();
            for (int x = 0; x < Board.BOARD_DIMENSIONS; x++)
            {
                for (int y = 0; y < Board.BOARD_DIMENSIONS; y++)
                {
                    BoardPiece currentPiece = board.BoardPieces[x, y];
                    if (y == 0) // Bottom row - P1
                    {
                        Assert.IsTrue(PieceTypePositionCheck(x, Player.PlayerOne, currentPiece));
                    }
                    else if (y == 1) // Pawn row - P1
                    {
                        Assert.IsTrue(currentPiece.PieceType == PieceType.Pawn && currentPiece.PieceOwner == Player.PlayerOne);
                    }
                    else if (y == 6) // Pawn row - P2
                    {
                        Assert.IsTrue(currentPiece.PieceType == PieceType.Pawn && currentPiece.PieceOwner == Player.PlayerTwo);
                    }
                    else if (y == 7) // Top row - P2
                    {
                        Assert.IsTrue(PieceTypePositionCheck(x, Player.PlayerTwo, currentPiece));
                    }
                    else // Empty squares
                    {
                        Assert.IsTrue(currentPiece.PieceType == PieceType.None && currentPiece.PieceOwner == Player.None);
                    }
                }
            }
        }

        [Test]
        public void TurnSwapTest()
        {
            Board board = BoardFactory.CreateBoard();

            Assert.IsTrue(board.PlayerTurn == Player.PlayerOne);
            board.ApplyMove(3, 1, 3, 2);

            Assert.IsTrue(board.PlayerTurn == Player.PlayerTwo);
            board.ApplyMove(3, 6, 3, 5);

            Assert.IsTrue(board.PlayerTurn == Player.PlayerOne);
        }

        [Test]
        public void ActionsRecordedTest()
        {
            Board board = BoardFactory.CreateBoard();
            List<BoardAction> recordedActions = new List<BoardAction>();
            board.OnBoardChanged += delegate(List<BoardAction> actions)
            {
                for (int i = 0; i < actions.Count; i++)
                {
                    recordedActions.Add(actions[i]);
                }
            };
            board.ApplyMove(3, 1, 3, 2);
            board.ApplyMove(3, 6, 3, 5);

            Assert.AreEqual(2, recordedActions.Count);
            
            // First action
            Assert.AreEqual(Player.PlayerOne, recordedActions[0].Player);
            Assert.AreEqual(BoardActionType.MovePiece, recordedActions[0].Type);
            var action = (MovePieceAction) recordedActions[0];
            Assert.AreEqual(PieceType.Pawn, action.MovedPieceType);
            Assert.AreEqual(new Vector2I(3,1), action.Move.From);
            Assert.AreEqual(new Vector2I(3,2), action.Move.To);
            
            // Second action
            Assert.AreEqual(Player.PlayerTwo, recordedActions[1].Player);
            Assert.AreEqual(BoardActionType.MovePiece, recordedActions[1].Type);
            action = (MovePieceAction) recordedActions[1];
            Assert.AreEqual(PieceType.Pawn, action.MovedPieceType);
            Assert.AreEqual(new Vector2I(3,6), action.Move.From);
            Assert.AreEqual(new Vector2I(3,5), action.Move.To);
        }

        [Test]
        public void CheckRegisteredTest()
        {
            char[,] boardLayout =
            {
                {'C', 'N', 'B', 'Q', 'K', 'B', 'N', 'C'},
                {'P', 'P', 'P', 'P', 'P', 'e', 'P', 'P'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'},
                {'e', 'e', 'e', 'e', 'e', 'P', 'e', 'e'}, 
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'}, 
                {'e', 'e', 'e', 'b', 'p', 'e', 'e', 'e'}, 
                {'p', 'p', 'p', 'p', 'e', 'p', 'p', 'p'},
                {'c', 'n', 'b', 'q', 'k', 'e', 'n', 'c'} 
            };
            boardLayout = boardLayout.RotateArray();
            
            Board board = BoardFactory.CreateBoard(boardLayout);
            bool inCheck = false;
            Player playerInCheck = Player.None;
            board.OnPlayerInCheck += delegate(Player player)
            {
                inCheck = true;
                playerInCheck = player;
            };
            
            Vector2I queenPos = new Vector2I(3, 0);
            Assert.IsTrue(board.BoardPieces[queenPos.x, queenPos.y].PieceType == PieceType.Queen);
            
            Vector2I queenDest = new Vector2I(7, 4);
            Assert.IsTrue(board.BoardPieces[queenDest.x, queenDest.y].PieceType == PieceType.None);
            
            BoardPieceMove move = new BoardPieceMove(queenPos, queenDest);
            board.ApplyMove(move);
            
            Assert.IsTrue(inCheck);
            Assert.AreEqual(playerInCheck, Player.PlayerTwo);
        }
        
        [Test]
        public void CheckMateRegisteredTest()
        {
            char[,] boardLayout =
            {
                {'e', 'e', 'e', 'e', 'K', 'e', 'e', 'e'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'c'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'}, 
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'},
                {'e', 'e', 'e', 'e', 'k', 'e', 'c', 'e'}
            };
            boardLayout = boardLayout.RotateArray();
            
            Board board = BoardFactory.CreateBoard(boardLayout);
            bool checkMate = false;
            Player winningPlayer = Player.None;
            board.OnGameStateChanged += delegate(GameState state)
            {
                if (state != GameState.WonByCheckmate)
                    return;
                
                checkMate = true;
                winningPlayer = board.PlayerTurn;
            };
            
            Vector2I castlePos = new Vector2I(6, 0);
            Assert.IsTrue(board.BoardPieces[castlePos.x, castlePos.y].PieceType == PieceType.Castle);
            
            Vector2I castleDest = new Vector2I(6, 7);
            Assert.IsTrue(board.BoardPieces[castleDest.x, castleDest.y].PieceType == PieceType.None);
            
            BoardPieceMove move = new BoardPieceMove(castlePos, castleDest);
            board.ApplyMove(move);
            
            Assert.IsTrue(checkMate);
            Assert.AreEqual(GameState.WonByCheckmate, board.GameState);
            Assert.AreEqual(winningPlayer, Player.PlayerOne);
        }

        [Test]
        public void ResetBoardTest()
        {
            char[,] boardLayout =
            {
                {'e', 'e', 'e', 'e', 'K', 'e', 'e', 'e'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'c'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'}, 
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'},
                {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'},
                {'e', 'e', 'e', 'e', 'k', 'e', 'c', 'e'}
            };
            boardLayout = boardLayout.RotateArray();
            
            Board board = BoardFactory.CreateBoard(boardLayout);
            board.PlayerTurn = Player.PlayerTwo;
            
            BoardFactory.ResetBoard(board);
            Assert.AreEqual(Player.PlayerOne, board.PlayerTurn);
            
            for (int x = 0; x < Board.BOARD_DIMENSIONS; x++)
            {
                for (int y = 0; y < Board.BOARD_DIMENSIONS; y++)
                {
                    BoardPiece currentPiece = board.BoardPieces[x, y];
                    if (y == 0) // Bottom row - P1
                    {
                        Assert.IsTrue(PieceTypePositionCheck(x, Player.PlayerOne, currentPiece));
                    }
                    else if (y == 1) // Pawn row - P1
                    {
                        Assert.IsTrue(currentPiece.PieceType == PieceType.Pawn && currentPiece.PieceOwner == Player.PlayerOne);
                    }
                    else if (y == 6) // Pawn row - P2
                    {
                        Assert.IsTrue(currentPiece.PieceType == PieceType.Pawn && currentPiece.PieceOwner == Player.PlayerTwo);
                    }
                    else if (y == 7) // Top row - P2
                    {
                        Assert.IsTrue(PieceTypePositionCheck(x, Player.PlayerTwo, currentPiece));
                    }
                    else // Empty squares
                    {
                        Assert.IsTrue(currentPiece.PieceType == PieceType.None && currentPiece.PieceOwner == Player.None);
                    }
                }
            }
        }

        private bool PieceTypePositionCheck(int xPos, Player player, BoardPiece piece)
        {
            switch (xPos)
            {
                case 0: // Left castle
                    if (piece.PieceType == PieceType.Castle && piece.PieceOwner == player)
                        return true;
                    break;
                case 1: // Left knight
                    if (piece.PieceType == PieceType.Knight && piece.PieceOwner == player)
                        return true;
                    break;
                case 2: // Left bishop
                    if (piece.PieceType == PieceType.Bishop && piece.PieceOwner == player)
                        return true;
                    break;
                case 3: // Queen
                    if (piece.PieceType == PieceType.Queen && piece.PieceOwner == player)
                        return true;
                    break;
                case 4: // Knight
                    if (piece.PieceType == PieceType.King && piece.PieceOwner == player)
                        return true;
                    break;
                case 5: // Right bishop
                    if (piece.PieceType == PieceType.Bishop && piece.PieceOwner == player)
                        return true;
                    break;
                case 6: // Right knight
                    if (piece.PieceType == PieceType.Knight && piece.PieceOwner == player)
                        return true;
                    break;
                case 7: // Right castle
                    if (piece.PieceType == PieceType.Castle && piece.PieceOwner == player)
                        return true;
                    break;
            }

            return false;
        }
    }
}