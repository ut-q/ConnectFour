using System;
using System.Collections.Generic;
using System.Diagnostics;

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

        private readonly int[] _columnOrder;
        private readonly Board _board;
        private readonly Random _random;

        private AIDifficultyTuning _difficultyTuning;

        private static int[][] _evaluationTable =
        {
            new int[] { 3, 4, 5, 7, 5, 4, 3 }, new int[] { 4, 6, 8, 10, 8, 6, 4 }, new int[] { 5, 8, 11, 13, 11, 8, 5 },
            new int[] { 5, 8, 11, 13, 11, 8, 5 }, new int[] { 4, 6, 8, 10, 8, 6, 4 }, new int[] { 3, 4, 5, 7, 5, 4, 3 }
        };

        private static int _evaluationConstant = 138;
        private static int _maxValue = 1000;

        public AIPlayer(PlayerType playerType, Board board, AIDifficultyTuning aiDifficultyTuning)
        {
            PlayerType = playerType;
            Name = playerType.ToString();
            _columnOrder = new int[board.Width];
            for (int i = 0; i < _columnOrder.Length; i++)
            {
                _columnOrder[i] = _columnOrder.Length / 2 + (1 - 2 * (i % 2)) * (i + 1) / 2; 
            }

            _board = board;
            _random = new Random();
            _difficultyTuning = aiDifficultyTuning;
        }

        public Move MakeMove()
        {
            int bestScore = int.MinValue;
            List<int> bestColumns = new List<int>(); 
            
            //DEBUG

            List<(int, int)> columnScores = new List<(int, int)>();
            
            //end DEBUG
            
            for (int i = 0; i < _board.Width; ++i)
            {
                Move move = new Move()
                {
                    MoveColumn = _columnOrder[i],
                    PlayerType = PlayerType
                };
                if (_board.CanMakeMove(move))
                {
                    int score;
                    if (_board.CheckIfMoveWinsPlayerTheGame(move))
                    {
                        score = _maxValue;
                    }
                    else
                    {
                        Board newBoard = _board.GetCopyForAiIteration(true);
                        newBoard.MakeMove(move);
                        score = -NegaMax(newBoard, -_maxValue, _maxValue,
                            PlayerType == PlayerType.Player1 ? PlayerType.Player2 : PlayerType.Player1, 0, _difficultyTuning.MaxDepth);
                    }

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestColumns.Clear();
                        bestColumns.Add(move.MoveColumn);
                    }
                    else if (score == bestScore)
                    {
                        bestColumns.Add(move.MoveColumn);
                        //TODO add weighted random here
                    }
                    
                    columnScores.Add((move.MoveColumn,score));
                }
            }
            
            Console.WriteLine($"DEBUG: AI Score {bestScore}");
            foreach ((int, int) columnScore in columnScores)
            {
                Console.Write($"(C:{columnScore.Item1 + 1} - S:{columnScore.Item2}) - ");
            }
            Console.WriteLine("");

            if (bestColumns.Count == 0)
            {
                return new Move()
                {
                    Result = Move.MoveResult.Fail,
                    Message = "Can't find next move, is it a draw?"
                };
            }
            int column = _random.Next(0, bestColumns.Count);
            
            //TODO handle no column being available?> (draw
            return new Move()
            {
                MoveColumn = bestColumns[column],
                PlayerType = PlayerType
            };
        }

        public void UpdateDifficultyTuning(AIDifficultyTuning tuning)
        {
            _difficultyTuning = tuning;
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

            for (int i = 0; i < board.Width; ++i)
            {
                Move move = new Move()
                {
                    MoveColumn = i,
                    PlayerType = playerType
                };
                if (board.CanMakeMove(move) && board.CheckIfMoveWinsPlayerTheGame(move))
                {
                    return _maxValue - board.MoveCount;
                }
            }
            
            int max = _maxValue - board.MoveCount;

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
                return EvaluateBoardScore(board, playerType);
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
            }

            return alpha;
        }

        private int EvaluateBoardScore(Board board, PlayerType currentPlayer)
        {
            int sum = 0;
            Board.SpaceState spaceState = Board.ConvertPlayerToSpaceState(currentPlayer);
            for (int row = 0; row < board.Height; row++)
            {
                for (int column = 0; column < board.Width; column++)
                {
                    Board.SpaceState cellSpaceState = board.GetSpace(row, column);
                    if (cellSpaceState == spaceState)
                    {
                        sum += _evaluationTable[row][column];
                    }
                    else if (cellSpaceState != Board.SpaceState.Empty)
                    {
                        sum -= _evaluationTable[row][column];
                    }
                }
            }

            return _evaluationConstant + sum;
        }
    }
}