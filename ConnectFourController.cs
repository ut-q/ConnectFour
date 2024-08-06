using System;
using System.Collections.Generic;

namespace ConnectFour
{
    public class ConnectFourController
    {
        private IPlayer _player1;
        private IPlayer _player2;
        private IPlayer _currentPlayer = null;

        private readonly Board _board;
        private readonly IBoardView _boardView;
        private readonly List<Move> _moves;
        private GameMode _currentGameMode;

        public ConnectFourController()
        {
            _currentGameMode = new ClassicGameMode();
            _board = new Board();
            _boardView = new BoardConsoleView(_board);
            _moves = new List<Move>();
            _player1 = new HumanPlayer(PlayerType.Player1, _board);
            _player2 = new AIPlayer(PlayerType.Player2, _board, AIDifficultyTuning.MediumTuning);
        }

        public void ShowMainMenu()
        {
            Console.WriteLine(GetCurrentPlayerInfoText());
            Console.WriteLine(GetCurrentGameModeText());
            HandleMainMenu();
        }

        /// <summary>
        /// Main Game Loop
        /// </summary>
        private void StartGame()
        {
            _board.Init(_currentGameMode);
            _player1.Init();
            _player2.Init();
            
            bool currentPlayerWon = false;
            _currentPlayer = _player1;
            _boardView.DisplayBoard();

            while (!_board.IsDraw())
            {
                Console.WriteLine($"Current Move: {_currentPlayer.Name}");
                Move move = _currentPlayer.MakeMove();
                if (move.MoveResult == Move.Result.Fail)
                {
                    // TODO evaluate calling print here
                    Console.WriteLine(move.Message);
                    continue;
                }

                if (!_board.CanMakeMove(move))
                {
                    Console.WriteLine(move.Message);
                    continue;
                }

                if (_board.CheckIfMoveWinsPlayerTheGame(move))
                {
                    currentPlayerWon = true;
                }
                
                _board.MakeMove(move);
                _moves.Add(move);

                string actionString = move.MoveType == Move.Type.PushTop ? "pushed" : "popped";
                Console.WriteLine($"{_currentPlayer.Name} {actionString} column {move.MoveColumn + 1}");
                _boardView.DisplayBoard();

                if (currentPlayerWon)
                {
                    break;
                }
             
                _currentPlayer = GetNextPlayer();
            }

            if (currentPlayerWon)
            {
                Console.WriteLine($"{_currentPlayer.Name} won the game in {_moves.Count} total moves");
            }
            else
            {
                Console.WriteLine($"Game is a draw after {_moves.Count} total moves");
            }
        }

        private IPlayer GetNextPlayer()
        {
            return _currentPlayer == _player2 ? _player1 : _player2;
        }

        private string GetCurrentPlayerInfoText()
        {
            return
                $"Current Player Info: \n Player1: {_player1.PlayerInfoString}\n Player2: {_player2.PlayerInfoString}";
        }
        
        private string GetCurrentGameModeText()
        {
            return
                $"Current Game Mode Info: \n {_currentGameMode.Tuning.GameModeName}\n {_currentGameMode.Tuning.GameModeExplanation}";
        }

        private string GetMainMenuText()
        {
            return $"Options: \n 1 - Start the game\n 2 - Change player settings\n 3 - Game Mode settings";
        }

        private void HandleMainMenu()
        {
            Console.WriteLine(GetMainMenuText());
            
            string selection = Console.ReadLine();
            int val = 1;
            if (selection == null || (!int.TryParse(selection, out val) && (val < 1 || val > 3)))
            {
                Console.WriteLine("Invalid main menu selection, try again");
            }

            if (val == 1)
            {
                StartGame();
            }
            else if (val == 2)
            {
                HandlePlayerMenu();
                ShowMainMenu();
            }
            else if (val == 3)
            {
                HandleGameModeMenu();
                ShowMainMenu();
            }
        }

        private string GetPlayerSettingsMenuText()
        {
            return $"Options: \n 1 - Change Player 1 Info \n 2 - Change Player 2 Info";
        }
        
        private void HandlePlayerMenu()
        {
            Console.WriteLine(GetPlayerSettingsMenuText());
            
            string selection = Console.ReadLine();
            int val = 1;
            if (selection == null || (!int.TryParse(selection, out val) && (val < 1 || val > 2)))
            {
                Console.WriteLine("Invalid main menu selection, try again");
            }

            switch (val)
            {
                case 1:
                    HandlePlayerTypeMenu(out _player1, PlayerType.Player1);
                    break;
                case 2:
                    HandlePlayerTypeMenu(out _player2, PlayerType.Player2);
                    break;
            }
        }
        
        private void HandlePlayerTypeMenu(out IPlayer player, PlayerType playerType)
        {
            Console.WriteLine("Set Player Type: \n 1 - Human\n 2 - Ai");
            string selection = Console.ReadLine();
            if (selection == null || (!int.TryParse(selection, out int type) && (type < 1 || type > 3)))
            {
                Console.WriteLine("Invalid menu selection, try again");
                player = null;
                return;
            }
            bool isHuman = type == 1;
            
            Console.WriteLine(@"Enter Player Name:");
            string playerName = Console.ReadLine();
            if (playerName == null)
            {
                Console.WriteLine("Invalid menu selection, try again");
                player = null;
                return;
            }

            if (isHuman)
            {
                player = new HumanPlayer(playerType, _board)
                {
                    Name = playerName
                };
            }
            else
            {
                Console.WriteLine("Set Ai Difficulty: \n 1 - Easy\n 2 - Medium\n 3 - Hard\n 4 - Pro");
                selection = Console.ReadLine();
                if (selection == null || (!int.TryParse(selection, out int difficulty) && (difficulty < 1 || difficulty > 4)))
                {
                    Console.WriteLine("Invalid menu selection, try again");
                    player = null;
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

                player = new AIPlayer(playerType, _board, tuning)
                {
                    Name = playerName
                };
            }
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
            
            string selection = Console.ReadLine();
            int val = 1;
            if (selection == null || (!int.TryParse(selection, out val) && (val < 1 || val > 2)))
            {
                Console.WriteLine("Invalid main menu selection, try again");
            }

            switch (val)
            {
                case 1:
                    _currentGameMode = new ClassicGameMode();
                    break;
                case 2:
                    _currentGameMode = new PopOutGameMode();
                    break;
            }
        }
    }
}