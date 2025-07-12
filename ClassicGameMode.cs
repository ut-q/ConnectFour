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
            GameModeExplanation = "Classic Connect Four Game with default rules",
            EvaluationConstant = 138, // sum of all the values in the evaluation table divided by two
            MaxBoardEvaluationValue = 1000, // this is sort of an arbitrary value, as long it's larger than 138 * 2, it's ok
            
            // represents how many potential game winning moves can be done at each cell
            // for example top left position can have 1 diagonal, 1 horizontal and 1 vertical so it's 3
            // taken from: https://softwareengineering.stackexchange.com/questions/263514/why-does-this-evaluation-function-work-in-a-connect-four-game-in-java
            EvaluationTable = new[,] 
            {
                { 3, 4, 5, 7, 5, 4, 3 },
                { 4, 6, 8, 10, 8, 6, 4 },
                { 5, 8, 11, 13, 11, 8, 5 },
                { 5, 8, 11, 13, 11, 8, 5 },
                { 4, 6, 8, 10, 8, 6, 4 },
                { 3, 4, 5, 7, 5, 4, 3 }
            }
        };

        public override GameModeTuning Tuning
        {
            get { return GameModeTuning; }
        }

        public override List<Move> GetAllPossibleMoves(Board board, PlayerType playerType, int[]? columnOrder = null)
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
            
            if (string.IsNullOrEmpty(input))
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