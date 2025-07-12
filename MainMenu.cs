using System;

namespace ConnectFour
{
    public class MainMenu
    {
        private readonly ConnectFourController _connectFourController;

        public MainMenu(ConnectFourController controller)
        {
            _connectFourController = controller;
        }

        public void ShowMainMenu()
        {
            Console.WriteLine(GetCurrentPlayerInfoText());
            Console.WriteLine(GetCurrentGameModeText());
            HandleMainMenu();
        }

        private string GetCurrentPlayerInfoText()
        {
            return
                $"Current Player Info: \n Player1: {_connectFourController.Player1.PlayerInfoString}\n Player2: {_connectFourController.Player2.PlayerInfoString}";
        }

        private string GetCurrentGameModeText()
        {
            return
                $"Current Game Mode Info: \n {_connectFourController.CurrentGameMode.Tuning.GameModeName}\n     {_connectFourController.CurrentGameMode.Tuning.GameModeExplanation}";
        }

        private string GetMainMenuText()
        {
            return
                $"Options: \n 1 - Start the game\n 2 - Change player settings\n 3 - Game Mode settings \n 4 - Toggle Debug Text";
        }

        private void HandleMainMenu()
        {
            Console.WriteLine(GetMainMenuText());

            string? selection = Console.ReadLine();
            int val = 1;
            if (string.IsNullOrEmpty(selection) || !int.TryParse(selection, out val) || val < 1 || val > 4)
            {
                Console.WriteLine("Invalid main menu selection, try again");
                return;
            }

            switch (val)
            {
                case 1:
                    _connectFourController.StartGame();
                    break;
                case 2:
                    HandlePlayerMenu();
                    ShowMainMenu();
                    break;
                case 3:
                    HandleGameModeMenu();
                    ShowMainMenu();
                    break;
                case 4:
                    ConnectFourController.ShowDebugText = !ConnectFourController.ShowDebugText;
                    ShowMainMenu();
                    break;
            }
        }

        private string GetPlayerSettingsMenuText()
        {
            return $"Options: \n 1 - Change Player 1 Info \n 2 - Change Player 2 Info";
        }

        private void HandlePlayerMenu()
        {
            Console.WriteLine(GetPlayerSettingsMenuText());

            string? selection = Console.ReadLine();
            int val = 1;
            if (string.IsNullOrEmpty(selection) || !int.TryParse(selection, out val) || val < 1 || val > 2)
            {
                Console.WriteLine("Invalid main menu selection, try again");
                return;
            }

            switch (val)
            {
                case 1:
                    HandlePlayerTypeMenu(PlayerType.Player1);
                    break;
                case 2:
                    HandlePlayerTypeMenu(PlayerType.Player2);
                    break;
            }
        }

        private void HandlePlayerTypeMenu(PlayerType playerType)
        {
            IPlayer player;

            Console.WriteLine("Set Player Type: \n 1 - Human\n 2 - Ai");
            string? selection = Console.ReadLine();
            if (string.IsNullOrEmpty(selection) || !int.TryParse(selection, out int type) || type < 1 || type > 3)
            {
                Console.WriteLine("Invalid menu selection, try again");
                return;
            }

            bool isHuman = type == 1;

            Console.WriteLine(@"Enter Player Name:");
            string? playerName = Console.ReadLine();
            if (string.IsNullOrEmpty(playerName))
            {
                Console.WriteLine("Invalid menu selection, try again");
                return;
            }

            if (isHuman)
            {
                player = new HumanPlayer(playerType, _connectFourController.Board)
                {
                    Name = playerName
                };
            }
            else
            {
                Console.WriteLine("Set Ai Difficulty: \n 1 - Easy\n 2 - Medium\n 3 - Hard\n 4 - Pro");
                string? difficultySelection = Console.ReadLine();
                if (string.IsNullOrEmpty(difficultySelection) ||
                    (!int.TryParse(difficultySelection, out int difficulty) && (difficulty < 1 || difficulty > 4)))
                {
                    Console.WriteLine("Invalid menu selection, try again");
                    return;
                }

                AIDifficultyTuning tuning = AIDifficultyTuning.MediumTuning;
                switch (difficulty)
                {
                    case 1:
                        tuning = AIDifficultyTuning.EasyTuning;
                        break;
                    case 2:
                        tuning = AIDifficultyTuning.MediumTuning;
                        break;
                    case 3:
                        tuning = AIDifficultyTuning.HardTuning;
                        break;
                    case 4:
                        tuning = AIDifficultyTuning.ProTuning;
                        break;
                }

                player = new AIPlayer(playerType, _connectFourController.Board, tuning)
                {
                    Name = playerName
                };
            }

            _connectFourController.SetPlayer(player, playerType);
        }

        private string GetGameModeSettingsMenuText()
        {
            return $"Options: \n 1 - {ClassicGameMode.GameModeTuning.GameModeName}\n" +
                   $"   {ClassicGameMode.GameModeTuning.GameModeExplanation}\n" +
                   $" 2 - {PopOutGameMode.GameModeTuning.GameModeName}\n" +
                   $"   {PopOutGameMode.GameModeTuning.GameModeExplanation}\n";
        }

        private void HandleGameModeMenu()
        {
            Console.WriteLine(GetGameModeSettingsMenuText());

            string? selection = Console.ReadLine();
            int val = 1;
            if (string.IsNullOrEmpty(selection) || !int.TryParse(selection, out val) || val < 1 || val > 2)
            {
                Console.WriteLine("Invalid main menu selection, try again");
                return;
            }

            GameMode gameMode;
            switch (val)
            {
                case 1:
                    gameMode = new ClassicGameMode();
                    break;
                case 2:
                    gameMode = new PopOutGameMode();
                    break;
                default:
                    gameMode = new ClassicGameMode();
                    break;
            }

            _connectFourController.SetGameMode(gameMode);
        }
    }
}