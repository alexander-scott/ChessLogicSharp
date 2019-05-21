namespace ChessLogicSharp.DataStructures
{
    public struct BoardPiece
    {
        public Player PieceOwner;
        public PieceType PieceType;
        public bool HasMoved;
        public bool CanEnPassant;
    }
    
    public enum PieceType
    {
        None,
        Pawn,
        Castle,
        Knight,
        Bishop,
        Queen,
        King
    }
    
    public static class EnumExtensions
    {
        public static string ToFriendlyString(this PieceType type)
        {
            switch (type)
            {
                default:
                    return "Pawn";
                case PieceType.Bishop:
                    return "Bishop";
                case PieceType.Knight:
                    return "Knight";
                case PieceType.Castle:
                    return "Castle";
                case PieceType.Queen:
                    return "Queen";
                case PieceType.King:
                    return "King";
            }
        }
        
        public static string ToFriendlyString(this Player type)
        {
            switch (type)
            {
                default:
                    return "None";
                case Player.PlayerOne:
                    return "Player One";
                case Player.PlayerTwo:
                    return "Player Two";
            }
        }
    }
}