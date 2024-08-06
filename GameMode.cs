using System.Collections.Generic;

namespace ConnectFour
{
    public abstract class GameMode
    {
        public abstract GameModeTuning Tuning { get; }
        
        // TODO see if we should pass a delegate instead
        public abstract List<Move> GetAllPossibleMoves(Board board, PlayerType playerType, int[] columnOrder = null);

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
        
        public string GameModeName { get; set; }
        
        public string GameModeExplanation { get; set; }
    }
}