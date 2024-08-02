using System;

namespace ConnectFour
{
    public class HumanPlayer : IPlayer
    {
        public PlayerType PlayerType { get; }
        public string Name { get;  }
        
        public HumanPlayer(PlayerType playerType)
        {
            PlayerType = playerType;
            Name = playerType.ToString();
        }
        
        public Move MakeMove()
        {
            Move move = new Move
            {
                PlayerType = PlayerType
            };
            string requestedColumn = Console.ReadLine();
            if (requestedColumn == null)
            {
                move.Result = Move.MoveResult.Fail;
                move.Message = "Can't read input - column value needs to be a positive integer";
                move.MoveColumn = Int32.MaxValue;
            }
            else if (!int.TryParse(requestedColumn, out int column))
            {
                move.Result = Move.MoveResult.Fail;
                move.Message = "Can't read input - column value needs to be a positive integer";
                move.MoveColumn = Int32.MaxValue;
            }
            else
            {
                move.Result = Move.MoveResult.Success;
                move.MoveColumn = column - 1;
            }

            return move;
        }
    }
}