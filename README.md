# ChessLogicSharp
A lightweight, fully-featured Chess solution for .NET Applications that exhibits correct piece movement with castling, en passant, pawn promotion and a minmax AI player.

## Usage

### Instantiation

Create an instance of a Board by using the static BoardFactory.CreateBoard function, creating a board with pieces in the [default positions](https://www.chess.com/article/view/how-to-set-up-a-chessboard). The board follows an X,Y grid system with (0,0) being the bottom left grid square and (7,7) being the top right grid square. The Player One pieces (white) are created on the two bottom rows and the Player Two pieces (black) are created on the top two rows. The Board also stores the current turn, which is Player One when the board is initially created and swaps when moves are made.

```csharp
Board board = BoardFactory.CreateBoard();
```

You must then create instances of ChessPlayers and add them to the Board. The ChessPlayer class itself is abstract so you must create a derived class or simply use the existing BasicPlayer. When instantiating a ChessPlayer you must also specific which team they are on, Player One or Player Two, and then adding them to the Board. ChessPlayer instances can then make moves by calling the base MakeMove function.

```csharp
public abstract class ChessPlayer
{
    public virtual void Update(float deltaTime) { }   

    protected virtual void OnGameStateChanged(GameState state) { }

    protected virtual void OnTurnSwapped(Player player) { }

    protected bool MovePiece(BoardPieceMove boardPieceMove) { }
}
```
```csharp
_board = BoardFactory.CreateBoard();   
_board.AddPlayer(new UnityChessPlayer(_board, Player.PlayerOne));
_board.AddPlayer(new AIChessPlayer(_board, Player.PlayerTwo)); 
```

With your custom ChessPlayers you can have specific player functionality, such as an AI moving for a player, or a 3rd party input manager applying moves for one of the players. 

```csharp
public class AIChessPlayer : ChessPlayer
{
    public AIChessPlayer(Board board, Player player) : base(board, player) { }
    
    protected override void BoardOnOnTurnSwapped(Player player)
    {
        if (player == _player)
        {
            ThreadPool.QueueUserWorkItem((state) => CalculateAndMove());
        }
    }

    private void CalculateAndMove()
    {
         MovePiece(MinMaxMoveCalc.GetBestMove(Board));
    }
}
```


### Making Moves

A move is defined by a From Position and a To Position. ChessPlayers can make moves by calling the MovePiece function if the have been added to the Board.

```csharp
var pawnPos = new Vector2I(4, 1);
var pawnDest = new Vector2I(4, 3);
var move = new BoardPieceMove(pawnPos, pawnDest);
_player.MovePiece(move);
```

The ApplyMove function will return false if the move you made was invalid in any way and will return true if the move was successfully applied. A move can be invalid for the following reasons:
- Attempting to move a piece which doesn't belong to the player whose turn it currently is.
- Attempting to make a move that puts the current player's king in check.
- Attempting to make a move with a piece that the piece is unable to do, as defined by the [piece movement rules](https://www.chessusa.com/chess-rules.html).
- Attempting to make a move when the board state is Game Over, which is set when a player has obtained checkmate.

### Callbacks

You are able to subscribe to various callbacks that are called throughout a chess game, making it easy for your application to react to events in the chess game. The OnBoardChanged callback is special because it gives you a list of every action applied to the board the previous turn, such as piece moved, piece taken, pawn promotion etc. and includes the move itself providing you with the To and From piece positions of the move. This can allow you to animate movement or just render the board.

```csharp
// Called when a player makes their move and its parameter is the current players go. 
public event PlayerDelegate OnTurnSwapped;

/// Called when a player is in checkmate and its parameter is the player in check.
public event PlayerDelegate OnPlayerInCheck;

/// Called when a something on the board has changed and its parameter is a list of changes.
public event BoardChangesDelegate OnBoardChanged;

/// Called when the state of the game changes, such as when a game is paused, resumed or ended.
public event BoardGameStateDelegate OnGameStateChanged;
```

### Current Limitations
- The pawn promotion is currently hard-coded to Queen.
