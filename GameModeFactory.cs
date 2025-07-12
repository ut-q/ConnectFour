namespace ConnectFour;

public class GameModeFactory : IGameModeFactory
{
    public GameMode CreateClassicGameMode()
    {
        return new ClassicGameMode();
    }

    public GameMode CreatePopOutGameMode()
    {
        return new PopOutGameMode();
    }

    public GameMode CreateGameMode(string gameModeType)
    {
        return gameModeType.ToLower() switch
        {
            "classic" => CreateClassicGameMode(),
            "popout" => CreatePopOutGameMode(),
            _ => CreateClassicGameMode() // Default to classic
        };
    }
} 