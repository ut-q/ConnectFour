using System;

namespace ConnectFour
{
    public class HumanPlayer : IPlayer
    {
        public PlayerType PlayerType { get; }
        public string Name { get; set; }
        private readonly Board _board;
        
        public string PlayerInfoString
        {
            get { return $"Type: Human - Name: {Name}"; }
        }
        
        public HumanPlayer(PlayerType playerType, Board board)
        {
            PlayerType = playerType;
            Name = playerType.ToString();
            _board = board;
        }
        
        public void Init(){}
        
        public Move MakeMove()
        {
            string? input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                return new Move
                {
                    MoveResult = Move.Result.Fail,
                    Message = "Invalid input: Please enter a valid column number"
                };
            }
            return _board.CurrentGameMode!.GetMoveFromInput(input, PlayerType);
        }
    }
}