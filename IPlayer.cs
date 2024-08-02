namespace ConnectFour
{
    public enum PlayerType
    {
        Player1,
        Player2,
    }
    
    public interface IPlayer
    {
        string Name { get; }
        Move MakeMove();
    }
}