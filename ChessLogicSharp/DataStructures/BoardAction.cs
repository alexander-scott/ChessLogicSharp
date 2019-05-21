namespace ChessLogicSharp.DataStructures
{
    public abstract class BoardAction
    {
        public BoardActionType Type;
        public Player Player;
    }

    public class MovePieceAction : BoardAction
    {
        public BoardPieceMove Move;
        public PieceType MovedPieceType;
    }
    
    public class TakePieceAction : BoardAction
    {
        public BoardPieceMove Move;
        public PieceType TakingPieceType;
        public PieceType TakenPieceType;
    }
    
    public class EnPassantTakePieceAction : BoardAction
    {
        public BoardPieceMove Move;
        public PieceType TakingPieceType;
        public Vector2I TakenPawnPosition;
    }

    public class CastlingAction : BoardAction
    {
        public BoardPieceMove KingMove;
        public BoardPieceMove CastleMove;
    }

    public class PromotePawnAction : BoardAction
    {
        public Vector2I PawnPosition;
        public PieceType NewPieceType;
    }
    
    public enum BoardActionType
    {
        None,
        MovePiece,
        TakePiece,
        EnPassantTakePiece,
        Castling,
        PromotePawn
    }
}