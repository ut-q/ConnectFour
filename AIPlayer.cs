using System;
using System.Collections.Generic;

namespace ConnectFour
{
    public class AIPlayer : IPlayer
    {
        public PlayerType PlayerType { get; }
        public string Name { get; set; }
        
        public string PlayerInfoString
        {
            get { return $"Type: Ai - Name: {Name} - Difficulty: {_difficultyTuning.Name}"; }
        }

        private int[] _columnOrder;
        private readonly Board _board;
        private readonly Random _random;

        private AIDifficultyTuning _difficultyTuning;

        public AIPlayer(PlayerType playerType, Board board, AIDifficultyTuning aiDifficultyTuning)
        {
            PlayerType = playerType;
            Name = playerType.ToString();
            

            _board = board;
            _random = new Random();
            _difficultyTuning = aiDifficultyTuning;
        }

        public void Init()
        {
            _columnOrder = new int[_board.Width];
            for (int i = 0; i < _columnOrder.Length; i++)
            {
                _columnOrder[i] = _columnOrder.Length / 2 + (1 - 2 * (i % 2)) * (i + 1) / 2; 
            }
        }

        public Move MakeMove()
        {
            int bestScore = int.MinValue;
            List<Move> bestMoves = new List<Move>(); 
            
            //DEBUG

            List<(string, int)> columnScores = new List<(string, int)>();
            
            //end DEBUG

            List<Move> moves = _board.CurrentGameMode.GetAllPossibleMoves(_board, PlayerType, _columnOrder);

            foreach (Move move in moves)
            {
                int score;
                if (_board.CheckIfMoveWinsPlayerTheGame(move))
                {
                    score = Board.MaxBoardEvaluationValue;
                }
                else
                {
                    Board newBoard = _board.GetCopyForAiIteration(true);
                    newBoard.MakeMove(move);
                    score = -NegaMax(newBoard, -Board.MaxBoardEvaluationValue, Board.MaxBoardEvaluationValue,
                        PlayerType == PlayerType.Player1 ? PlayerType.Player2 : PlayerType.Player1, 0, _difficultyTuning.MaxDepth);
                }

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMoves.Clear();
                    bestMoves.Add(move);
                }
                else if (score == bestScore)
                {
                    bestMoves.Add(move);
                    //TODO add weighted random here
                }
                    
                columnScores.Add((move.MoveColumn + " " + move.MoveType.ToString(),score));
            }
            
            Console.WriteLine($"DEBUG: AI Score {bestScore}");
            foreach ((string, int) columnScore in columnScores)
            {
                Console.Write($"(C:{columnScore.Item1 + 1} - S:{columnScore.Item2}) - ");
            }
            Console.WriteLine("");

            if (bestMoves.Count == 0)
            {
                return new Move()
                {
                    MoveResult = Move.Result.Fail,
                    Message = "Can't find next move, is it a draw?"
                };
            }
            int column = _random.Next(0, bestMoves.Count);
            
            //TODO handle no column being available?> (draw
            return bestMoves[column];
        }

        /// <summary>
        /// Algorithm adapted from: http://blog.gamesolver.org/solving-connect-four/01-introduction/
        /// </summary>
        private int NegaMax(Board board, int alpha, int beta, PlayerType playerType, int depth, int maxDepth)
        {
            if (alpha >= beta)
            {
                //TODO clean this up obviously
                Console.WriteLine("Alpha is larger than beta");
                return 0;
            }
            
            if (board.IsDraw())
            {
                return 0;
            }
            
            List<Move> moves = board.CurrentGameMode.GetAllPossibleMoves(board, playerType, _columnOrder);
            foreach (Move move in moves)
            {
                if(board.CheckIfMoveWinsPlayerTheGame(move))
                {
                    return Board.MaxBoardEvaluationValue - board.MoveCount;
                }
            }
            
            int max = Board.MaxBoardEvaluationValue - board.MoveCount;

            if (beta > max)
            {
                beta = max;
                if (alpha >= beta)
                {
                    return beta;
                }
            }

            if (depth >= maxDepth)
            {
                return board.GetBoardEvaluationScore(playerType);
            }
            
            moves = board.CurrentGameMode.GetAllPossibleMoves(board, playerType, _columnOrder);

            foreach (Move move in moves)
            {
                Board newBoard = board.GetCopyForAiIteration();
                newBoard.MakeMove(move);

                int score = -NegaMax(newBoard, -beta, -alpha,
                    playerType == PlayerType.Player1 ? PlayerType.Player2 : PlayerType.Player1, depth + 1, maxDepth);

                if (score >= beta)
                {
                    return score;
                }

                if (score > alpha)
                {
                    alpha = score;
                }
            }

            return alpha;
        }
    }
}