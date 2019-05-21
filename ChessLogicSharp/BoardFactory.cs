using System.Collections.Generic;
using ChessLogicSharp.DataStructures;

namespace ChessLogicSharp
{
     public static class BoardFactory
        {
            public static Board CreateDefaultBoard()
            {
                BoardPiece[,] boardPieces = new BoardPiece[Board.BOARD_DIMENSIONS, Board.BOARD_DIMENSIONS];
                CreateP1Set(boardPieces);
                CreateP2Set(boardPieces);
                return new Board {BoardPieces = boardPieces, PlayerTurn = Player.PlayerOne, Actions = new Stack<BoardAction>()};
            }
    
            private static void CreateP1Set(BoardPiece[,] boardPieces)
            {
                int yPos = 7;
    
                // Create castles
                var pawnData = new BoardPiece {PieceType = PieceType.Pawn, PieceOwner = Player.PlayerOne};
                var castleData = new BoardPiece {PieceType = PieceType.Castle, PieceOwner = Player.PlayerOne};
                var knightData = new BoardPiece {PieceType = PieceType.Knight, PieceOwner = Player.PlayerOne};
                var bishopData = new BoardPiece {PieceType = PieceType.Bishop, PieceOwner = Player.PlayerOne};
                var queenData = new BoardPiece {PieceType = PieceType.Queen, PieceOwner = Player.PlayerOne};
                var kingData = new BoardPiece {PieceType = PieceType.King, PieceOwner = Player.PlayerOne};
    
                boardPieces[0, yPos] = castleData;
                boardPieces[7, yPos] = castleData;
    
                boardPieces[1, yPos] = knightData;
                boardPieces[6, yPos] = knightData;
    
                boardPieces[2, yPos] = bishopData;
                boardPieces[5, yPos] = bishopData;
    
                boardPieces[3, yPos] = queenData;
                boardPieces[4, yPos] = kingData;
    
                yPos = 6;
                // Create Pawns
                for (int xPos = 0; xPos < Board.BOARD_DIMENSIONS; xPos++)
                {
                    boardPieces[xPos, yPos] = pawnData;
                }
            }
    
            private static void CreateP2Set(BoardPiece[,] boardPieces)
            {
                int yPos = 0;
    
                // Create castles
                var pawnData = new BoardPiece {PieceType = PieceType.Pawn, PieceOwner = Player.PlayerTwo};
                var castleData = new BoardPiece {PieceType = PieceType.Castle, PieceOwner = Player.PlayerTwo};
                var knightData = new BoardPiece {PieceType = PieceType.Knight, PieceOwner = Player.PlayerTwo};
                var bishopData = new BoardPiece {PieceType = PieceType.Bishop, PieceOwner = Player.PlayerTwo};
                var queenData = new BoardPiece {PieceType = PieceType.Queen, PieceOwner = Player.PlayerTwo};
                var kingData = new BoardPiece {PieceType = PieceType.King, PieceOwner = Player.PlayerTwo};
    
                boardPieces[0, yPos] = castleData;
                boardPieces[7, yPos] = castleData;
    
                boardPieces[1, yPos] = knightData;
                boardPieces[6, yPos] = knightData;
    
                boardPieces[2, yPos] = bishopData;
                boardPieces[5, yPos] = bishopData;
    
                boardPieces[3, yPos] = queenData;
                boardPieces[4, yPos] = kingData;
    
                yPos = 1;
                // Create Pawns
                for (int xPos = 0; xPos < Board.BOARD_DIMENSIONS; xPos++)
                {
                    boardPieces[xPos, yPos] = pawnData;
                }
            }
        }
}