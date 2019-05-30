namespace ChessLogicSharp.DataStructures
{
    public abstract class BoardChange
    {
        public BoardChangeType Type;
        public Player Player;
    }

    public class MovePieceChange : BoardChange
    {
        public BoardPieceMove Move;
        public PieceType MovedPieceType;
    }
    
    public class TakePieceChange : BoardChange
    {
        public BoardPieceMove Move;
        public PieceType TakingPieceType;
        public PieceType TakenPieceType;
    }
    
    public class EnPassantTakePieceChange : BoardChange
    {
        public BoardPieceMove Move;
        public PieceType TakingPieceType;
        public Vector2I TakenPawnPosition;
    }

    public class CastlingChange : BoardChange
    {
        public BoardPieceMove KingMove;
        public BoardPieceMove CastleMove;
    }

    public class PromotePawnChange : BoardChange
    {
        public Vector2I PawnPosition;
        public PieceType NewPieceType;
    }
    
    public enum BoardChangeType
    {
        None,
        MovePiece,
        TakePiece,
        EnPassantTakePiece,
        Castling,
        PromotePawn
    }
}