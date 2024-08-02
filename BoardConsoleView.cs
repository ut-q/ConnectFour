using System;

namespace ConnectFour
{
    public class BoardConsoleView : IBoardView
    {
        private readonly Board _boardReference;

        public BoardConsoleView(Board board)
        {
            _boardReference = board;
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