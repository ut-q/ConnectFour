using System;

namespace ConnectFour
{
    public class BoardConsoleView : IBoardView
    {
        private readonly Board _boardReference;
        private IPlayer? _player1;
        private IPlayer? _player2;
        
        public BoardConsoleView(Board board)
        {
            _boardReference = board;
        }

        public void Init(IPlayer player1, IPlayer player2)
        {
            _player1 = player1;
            _player2 = player2;
        }

        public void DisplayBoard()
        {
            for (int i = 0; i < _boardReference.Width; i++)
            {
                Console.Write($"| {i + 1} |");
            }
            Console.WriteLine();

            for (int i = 0; i < _boardReference.Height; i++)
            {
                for (int j = 0; j < _boardReference.Width; j++)
                {
                    Console.Write($"| {GetSpaceStateCharacter(_boardReference.GetSpace(_boardReference.Height - i - 1, j))} |");
                }
                Console.WriteLine();
            }
            Console.WriteLine($"{_player1?.Name ?? "Player1"}:{GetSpaceStateCharacter(Board.ConvertPlayerToSpaceState(PlayerType.Player1))}\n" +
                              $"{_player2?.Name ?? "Player2"}:{GetSpaceStateCharacter(Board.ConvertPlayerToSpaceState(PlayerType.Player2))}");
        }

        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }
        
        private static string GetSpaceStateCharacter(Board.SpaceState s)
        {
            switch(s)
            {
                case Board.SpaceState.Empty:
                default:
                    return " ";
                case Board.SpaceState.Player1:
                    return "X";
                case Board.SpaceState.Player2:
                    return "O";
            }
        }
    }
}