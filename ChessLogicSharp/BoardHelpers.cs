using System;
using ChessLogicSharp.DataStructures;

namespace ChessLogicSharp
{
    public static class BoardHelpers
    {
        public static BoardPiece[,] DuplicateBoard(BoardPiece[,] oldBoard)
        {
            BoardPiece[,] newBoard = new BoardPiece[Board.BOARD_DIMENSIONS, Board.BOARD_DIMENSIONS];
            for (int x = 0; x < Board.BOARD_DIMENSIONS; x++)
            {
                for (int z = 0; z < Board.BOARD_DIMENSIONS; z++)
                {
                    newBoard[x, z] = oldBoard[x, z];
                }
            }

            return newBoard;
        }

        public static Board DuplicateBoard(Board oldBoard)
        {
            return new Board
            {
                PlayerTurn = oldBoard.PlayerTurn, BoardPieces = DuplicateBoard(oldBoard.BoardPieces)
            };
        }

        public static int GetPlayerDirection(Player playerNum)
        {
            if (playerNum == Player.PlayerOne)
            {
                return 1;
            }
            
            if (playerNum == Player.PlayerTwo)
            {
                return -1;
            }

            throw new Exception();
        }

        public static Player GetOpponentPlayer(Player currentTurn)
        {
            return currentTurn == Player.PlayerOne ? Player.PlayerTwo : Player.PlayerOne;
        }

        public static string ConvertPositionIntoStringRep(Vector2I pos)
        {
            return "(" + XPosToLetter(pos.x) + YPosToLetter(pos.y) + ")";
        }

        public static Vector2I ConvertStringRepIntoPos(string pos)
        {
            if (pos.Length != 2)
            {
                throw new Exception();
            }
            
            return new Vector2I(LetterToXPos(pos[0]), LetterToYPos(pos[1]));
        }

        private static string XPosToLetter(int pos)
        {
            switch (pos)
            {
                case 0:
                    return "a";
                case 1:
                    return "b";
                case 2:
                    return "c";
                case 3:
                    return "d";
                case 4:
                    return "e";
                case 5:
                    return "f";
                case 6:
                    return "g";
                case 7:
                    return "h";
            }

            throw new Exception();
        }

        private static string YPosToLetter(int pos)
        {
            return (1 + pos).ToString();
        }
        
        private static int LetterToXPos(char letter)
        {
            switch (letter)
            {
                case 'a':
                    return 0;
                case 'b':
                    return 1;
                case 'c':
                    return 2;
                case 'd':
                    return 3;
                case 'e':
                    return 4;
                case 'f':
                    return 5;
                case 'g':
                    return 6;
                case 'h':
                    return 7;
            }

            throw new Exception();
        }

        private static int LetterToYPos(char pos)
        {
            return pos - 1;
        }

        public static bool ValidMoveRepresentation(string move)
        {
            if (move.Length != 4)
            {
                return false;
            }
            
            char charToTest = move[0];
            if (!ValidXPos(charToTest))
            {
                return false;
            }

            charToTest = move[1];
            if (!ValidYPos(charToTest))
            {
                return false;
            }
            
            charToTest = move[2];
            if (!ValidXPos(charToTest))
            {
                return false;
            }
            
            charToTest = move[3];
            if (!ValidYPos(charToTest))
            {
                return false;
            }
            
            return true;
        }

        private static bool ValidXPos(char pos)
        {
            switch (pos)
            {
                case 'a':
                    return true;
                case 'b':
                    return true;
                case 'c':
                    return true;
                case 'd':
                    return true;
                case 'e':
                    return true;
                case 'f':
                    return true;
                case 'g':
                    return true;
                case 'h':
                    return true;
            }
            
            return false;
        }

        private static bool ValidYPos(char pos)
        {
            if (!int.TryParse(pos.ToString(), out var intRep))
            {
                return false;
            }
            return intRep > 0 && intRep < 9;
        }
    }

    public struct Vector2I
    {
        public int x;
        public int y;

        public Vector2I(int x1, int y1)
        {
            x = x1;
            y = y1;
        }
    }
}