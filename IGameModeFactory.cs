namespace ConnectFour;

public interface IGameModeFactory
{
    GameMode CreateClassicGameMode();
    GameMode CreatePopOutGameMode();
    GameMode CreateGameMode(string gameModeType);
} 