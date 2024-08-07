namespace ConnectFour
{
    public interface IBoardView
    {
        void DisplayBoard();
        void Init(IPlayer player1, IPlayer player2);
        void ShowMessage(string message);
    }
}