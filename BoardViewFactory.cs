namespace ConnectFour;

public class BoardViewFactory : IBoardViewFactory
{
    public IBoardView CreateBoardView(Board board)
    {
        return CreateConsoleBoardView(board);
    }

    public IBoardView CreateConsoleBoardView(Board board)
    {
        return new BoardConsoleView(board);
    }
} 