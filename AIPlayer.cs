namespace ConnectFour
{
    public class AIPlayer : IPlayer
    {
        public PlayerType PlayerType { get; }
        public string Name { get;  }

        private readonly int[] _columnOrder;
        private readonly Board _board;

        public AIPlayer(PlayerType playerType, Board board)
        {
            PlayerType = playerType;
            Name = playerType.ToString();
            _columnOrder = new int[board.Width];
            for (int i = 0; i < _columnOrder.Length; i++)
            {
                _columnOrder[i] = _columnOrder.Length / 2 + (1 - 2 * (i % 2)) * (i + 1) / 2; 
            }

            _board = board;
        }

        public Move MakeMove()
        {
            int bestScore = int.MinValue;
            int bestColumn = -1;
            for (int i = 0; i < _board.Width; ++i)
            {
                Move move = new Move()
                {
                    MoveColumn = i,
                    PlayerType = PlayerType
                };
                if (_board.CanMakeMove(move))
                {
                    Board newBoard = _board.GetCopyForAiIteration();
                    newBoard.MakeMove(move);
                    int score = NegaMax(newBoard, 3, -3,
                        PlayerType == PlayerType.Player1 ? PlayerType.Player2 : PlayerType.Player1);
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestColumn = i;
                    }
                    //TODO handle equality case with a random selection
                }
            }
            
            //TODO handle no column being available?> (draw
            return new Move()
            {
                MoveColumn = bestColumn,
                PlayerType = PlayerType
            };
        }

        /// <summary>
        /// Algorithm adapted from: http://blog.gamesolver.org/solving-connect-four/01-introduction/
        /// </summary>
        private int NegaMax(Board board, int alpha, int beta, PlayerType playerType)
        {
            if (board.IsDraw())
            {
                return 0;
            }

            for (int i = 0; i < board.Width; ++i)
            {
                Move move = new Move()
                {
                    MoveColumn = i,
                    PlayerType = playerType
                };
                if (board.CanMakeMove(move) && board.CheckIfMoveWinsPlayerTheGame(move))
                {
                    return (board.Width * board.Height + 1 - board.MoveCount) / 2;
                }
            }
            
            int max = (board.Width * board.Height - 1 - board.MoveCount) / 2;

            if (beta > max)
            {
                beta = max;
                if (alpha >= beta)
                {
                    return beta;
                }
            }

            for (int i = 0; i < board.Width; ++i)
            {
                Move move = new Move()
                {
                    MoveColumn = _columnOrder[i],
                    PlayerType = playerType
                };
                if (board.CanMakeMove(move))
                {
                    Board newBoard = _board.GetCopyForAiIteration();
                    newBoard.MakeMove(move);

                    int score = -NegaMax(newBoard, -beta, -alpha,
                        playerType == PlayerType.Player1 ? PlayerType.Player2 : PlayerType.Player1);

                    if (score >= beta)
                    {
                        return score;
                    }

                    if (score > alpha)
                    {
                        alpha = score;
                    }
                }
            }

            return alpha;
        }
    }
}