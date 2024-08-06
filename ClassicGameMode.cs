using System.Collections.Generic;

namespace ConnectFour
{
    public class ClassicGameMode : GameMode
    {
        public static readonly GameModeTuning GameModeTuning = new GameModeTuning()
        {
            Height = 6,
            Width = 7,
            NumberOfTilesToWin = 4,
            GameModeName = "Classic",
            GameModeExplanation = "Classic Connect Four Game with default rules"
        };

        public override GameModeTuning Tuning
        {
            get { return GameModeTuning; }
        }

        public override List<Move> GetAllPossibleMoves(Board board, PlayerType playerType, int[] columnOrder = null)
        {
            List<Move> moves = new List<Move>();
            for (int i = 0; i < board.Width; ++i)
            {
                Move move = new Move()
                {
                    MoveColumn = columnOrder != null ? columnOrder[i] : i,
                    PlayerType = playerType
                };
                if (board.CanMakeMove(move))
                {
                    moves.Add(move);
                }
            }

            return moves;
        }

        public override Move GetMoveFromInput(string input, PlayerType playerType)
        {
            Move move = new Move
            {
                PlayerType = playerType
            };
            
            if (input == null)
            {
                move.MoveResult = Move.Result.Fail;
                move.Message = "Can't read input - column value needs to be a positive integer";
                move.MoveColumn = int.MaxValue;
            }
            else if (!int.TryParse(input, out int column))
            {
                move.MoveResult = Move.Result.Fail;
                move.Message = "Can't read input - column value needs to be a positive integer";
                move.MoveColumn = int.MaxValue;
            }
            else
            {
                move.MoveResult = Move.Result.Success;
                move.MoveColumn = column - 1;
            }

            return move;
        }
    }
}