using System;
using System.Collections.Generic;

namespace ConnectFour
{
    public class ConnectFourController
    {
        private readonly IPlayer _player1;
        private readonly IPlayer _player2;
        private IPlayer _currentPlayer = null;

        private readonly Board _board;
        private readonly IBoardView _boardView;
        private readonly List<Move> _moves;

        public ConnectFourController()
        {

            _board = new Board();
            _boardView = new BoardConsoleView(_board);
            _moves = new List<Move>();
            
            _player1 = new HumanPlayer(PlayerType.Player1);
            _player2 = new AIPlayer(PlayerType.Player2, _board);
        }

        /// <summary>
        /// Main Game Loop
        /// </summary>
        public void RunGame()
        {
            bool currentPlayerWon = false;
            _currentPlayer = _player1;
            _boardView.DisplayBoard();

            while (!_board.IsDraw())
            {
                Console.WriteLine($"Current Move: {_currentPlayer.Name}");
                Move move = _currentPlayer.MakeMove();
                if (move.Result == Move.MoveResult.Fail)
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
                
                Console.WriteLine($"{_currentPlayer.Name} played column {move.MoveColumn}");
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
        
    }
}