namespace ConnectFour;

public interface IPlayerFactory
{
    IPlayer CreateHumanPlayer(PlayerType playerType, Board board, string name);
    IPlayer CreateAIPlayer(PlayerType playerType, Board board, AIDifficultyTuning difficulty, string name);
    IPlayer CreatePlayer(PlayerType playerType, Board board, string playerTypeName, string name, string? difficulty = null);
} 