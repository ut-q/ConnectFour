using System.Collections.Generic;

namespace ConnectFour
{
    public class ConnectFourController
    {
        private IPlayer? _currentPlayer = null;

        private readonly IBoardView _boardView;
        private readonly List<Move> _moves;

        public static bool ShowDebugText = false; // toggled in the main menu

        public GameMode CurrentGameMode { get; private set; }
        public IPlayer Player1 { get; private set; }
        public IPlayer Player2 { get; private set; }
        public Board Board { get; }

            public ConnectFourController()
    {
        CurrentGameMode = new ClassicGameMode();
        Board = new Board();
        _boardView = new BoardConsoleView(Board);
        _moves = new List<Move>();
        Player1 = new HumanPlayer(PlayerType.Player1, Board);
        Player2 = new AIPlayer(PlayerType.Player2, Board, AIDifficultyTuning.MediumTuning);
    }

    public ConnectFourController(
        IGameModeFactory gameModeFactory,
        IBoardFactory boardFactory,
        IBoardViewFactory boardViewFactory,
        IPlayerFactory playerFactory)
    {
        CurrentGameMode = gameModeFactory.CreateClassicGameMode();
        Board = boardFactory.CreateBoard(CurrentGameMode);
        _boardView = boardViewFactory.CreateConsoleBoardView(Board);
        _moves = new List<Move>();
        Player1 = playerFactory.CreateHumanPlayer(PlayerType.Player1, Board, "Player1");
        Player2 = playerFactory.CreateAIPlayer(PlayerType.Player2, Board, AIDifficultyTuning.MediumTuning, "Player2");
    }

        /// <summary>
        /// Main Game Loop
        /// </summary>
        public void StartGame()
        {
            Board.Init(CurrentGameMode);
            Player1.Init();
            Player2.Init();
            _boardView.Init(Player1, Player2);
            
            bool currentPlayerWon = false;
            _currentPlayer = Player1;
            _boardView.DisplayBoard();

            while (!Board.IsDraw())
            {
                _boardView.ShowMessage($"Current Move: {_currentPlayer.Name}");
                Move move = _currentPlayer.MakeMove();
                if (move.MoveResult == Move.Result.Fail)
                {
                    _boardView.ShowMessage(move.Message);
                    continue;
                }

                if (!Board.CanMakeMove(move))
                {
                    _boardView.ShowMessage(move.Message);
                    continue;
                }

                if (Board.CheckIfMoveWinsPlayerTheGame(move))
                {
                    currentPlayerWon = true;
                }
                
                Board.MakeMove(move);
                _moves.Add(move);

                string actionString = move.MoveType == Move.Type.PushTop ? "pushed" : "popped";
                _boardView.ShowMessage($"{_currentPlayer.Name} {actionString} column {move.MoveColumn + 1}");
                _boardView.DisplayBoard();

                if (currentPlayerWon)
                {
                    break;
                }
             
                _currentPlayer = GetNextPlayer();
            }

            if (currentPlayerWon)
            {
                _boardView.ShowMessage($"{_currentPlayer.Name} won the game in {_moves.Count} total moves");
            }
            else
            {
                _boardView.ShowMessage($"Game is a draw after {_moves.Count} total moves");
            }
        }

        private IPlayer GetNextPlayer()
        {
            return _currentPlayer == Player2 ? Player1 : Player2;
        }

        public void SetPlayer(IPlayer player, PlayerType playerType)
        {
            if (playerType == PlayerType.Player1)
            {
                Player1 = player;
            }
            else // player 2
            {
                Player2 = player;
            }
        }
        
        public void SetGameMode(GameMode gameMode)
        {
            CurrentGameMode = gameMode;
        }
        
    }
}