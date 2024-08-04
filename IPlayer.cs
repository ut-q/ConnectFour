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
        PlayerType PlayerType { get; }
        string PlayerInfoString { get; }
        Move MakeMove();
    }
}