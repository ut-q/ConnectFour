namespace ConnectFour;

public interface IBoardViewFactory
{
    IBoardView CreateBoardView(Board board);
    IBoardView CreateConsoleBoardView(Board board);
} 