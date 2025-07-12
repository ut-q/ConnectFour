namespace ConnectFour;

public interface IBoardFactory
{
    Board CreateBoard();
    Board CreateBoard(GameMode gameMode);
} 