namespace ConnectFour;

public class PlayerFactory : IPlayerFactory
{
    public IPlayer CreateHumanPlayer(PlayerType playerType, Board board, string name)
    {
        return new HumanPlayer(playerType, board) { Name = name };
    }

    public IPlayer CreateAIPlayer(PlayerType playerType, Board board, AIDifficultyTuning difficulty, string name)
    {
        return new AIPlayer(playerType, board, difficulty) { Name = name };
    }

    public IPlayer CreatePlayer(PlayerType playerType, Board board, string playerTypeName, string name, string? difficulty = null)
    {
        return playerTypeName.ToLower() switch
        {
            "human" => CreateHumanPlayer(playerType, board, name),
            "ai" => CreateAIPlayer(playerType, board, GetDifficultyTuning(difficulty), name),
            _ => CreateHumanPlayer(playerType, board, name) // Default to human
        };
    }

    private static AIDifficultyTuning GetDifficultyTuning(string? difficulty)
    {
        return difficulty?.ToLower() switch
        {
            "easy" => AIDifficultyTuning.EasyTuning,
            "medium" => AIDifficultyTuning.MediumTuning,
            "hard" => AIDifficultyTuning.HardTuning,
            "pro" => AIDifficultyTuning.ProTuning,
            _ => AIDifficultyTuning.MediumTuning // Default to medium
        };
    }
} 