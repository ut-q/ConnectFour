using System.Collections.Generic;

namespace ConnectFour
{
    public abstract class GameMode
    {
        public abstract GameModeTuning Tuning { get; }
        public abstract List<Move> GetAllPossibleMoves(Board board, PlayerType playerType, int[]? columnOrder = null);
        public virtual void InitializeBoard(Board board)
        {
            
        }

        public abstract Move GetMoveFromInput(string input, PlayerType playerType);

    }

    public class GameModeTuning
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public int NumberOfTilesToWin { get; set; }
        public string GameModeName { get; set; } = string.Empty;
        public string GameModeExplanation { get; set; } = string.Empty;
        public int[,] EvaluationTable { get; set; } = new int[0,0];
        public int EvaluationConstant { get; set; }
        public int MaxBoardEvaluationValue { get; set; }
    }
}