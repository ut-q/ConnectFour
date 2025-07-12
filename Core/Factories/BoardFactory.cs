namespace ConnectFour;

public class BoardFactory : IBoardFactory
{
    public Board CreateBoard()
    {
        return new Board();
    }

    public Board CreateBoard(GameMode gameMode)
    {
        var board = new Board();
        board.Init(gameMode);
        return board;
    }
} 