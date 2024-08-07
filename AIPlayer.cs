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

        // this is an optimization. This stores the most likely to be used column in order, this way we start going down the minimax tree
        // branches with the highest possibility, potentially saving time.
        private int[] _columnOrder;
        
        private readonly Board _board;
        private readonly Random _random;

        private AIDifficultyTuning _difficultyTuning;

        private static bool ShowDebugText
        {
            get { return ConnectFourController.ShowDebugText; }
        }

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

        /// <summary>
        /// This is the entry point for an AI player move
        /// </summary>
        public Move MakeMove()
        {
            Stopwatch stopwatch = null;
            if (ShowDebugText) // this is also used as a profiling flag here but well... it is a timed test :)
            {
                stopwatch = new Stopwatch();
                stopwatch.Start();
            }
            
            int bestScore = int.MinValue;
            List<Move> bestMoves = new List<Move>();
            List<(Move, int)> moveScores = new List<(Move, int)>();

            List<Move> moves = _board.CurrentGameMode.GetAllPossibleMoves(_board, PlayerType, _columnOrder);
            foreach (Move move in moves)
            {
                int score;
                if (_board.CheckIfMoveWinsPlayerTheGame(move))
                {
                    score = _board.MaxBoardEvaluationValue;
                }
                else
                {
                    // we use a copy of the board to process the move in order to preserve state
                    Board newBoard = _board.GetCopyForAiIteration(true);
                    newBoard.MakeMove(move);
                    // this is the entry point for the minimax tree search algorithm
                    score = -NegaMax(newBoard, -_board.MaxBoardEvaluationValue, _board.MaxBoardEvaluationValue,
                        PlayerType == PlayerType.Player1 ? PlayerType.Player2 : PlayerType.Player1, 0,
                        _difficultyTuning.MaxDepth);
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
                }

                if (ShowDebugText)
                {
                    moveScores.Add((move, score));
                }
            }

            if (ShowDebugText)
            {
                Console.WriteLine($"DEBUG: AI Score {bestScore}");
                foreach ((Move, int) moveScore in moveScores)
                {
                    string actionString = moveScore.Item1.MoveType == Move.Type.PushTop ? "pushed" : "popped";
                    Console.Write($"(C:{moveScore.Item1.MoveColumn + 1} {actionString} S:{moveScore.Item2}) || ");
                }

                Console.WriteLine("");
            }

            if (bestMoves.Count == 0)
            {
                return new Move()
                {
                    MoveResult = Move.Result.Fail,
                    Message = "Can't find next move, is it a draw?"
                };
            }

            // this would ideally be a weighted random so there would be some  more realistic variation to the choices on ties
            int column = _random.Next(0, bestMoves.Count);
            
            if (ShowDebugText && stopwatch != null)
            {
                stopwatch.Stop();
                Console.WriteLine($"Move took {stopwatch.Elapsed.Milliseconds} milliseconds");    
            }

            return bestMoves[column];
        }

        /// <summary>
        /// Algorithm adapted from: http://blog.gamesolver.org/solving-connect-four/01-introduction/
        ///
        /// A minimax tree search algorithm with alpha-beta pruning
        /// </summary>
        private int NegaMax(Board board, int alpha, int beta, PlayerType playerType, int depth, int maxDepth)
        {
            if (alpha >= beta)
            {
                throw new InvalidOperationException(
                    "Alpha is larger than beta during alpha-beta pruning, this can't happen");
            }

            if (board.IsDraw())
            {
                return 0;
            }

            List<Move> moves = board.CurrentGameMode.GetAllPossibleMoves(board, playerType, _columnOrder);
            foreach (Move move in moves)
            {
                if (board.CheckIfMoveWinsPlayerTheGame(move))
                {
                    return board.MaxBoardEvaluationValue - board.MoveCount;
                }
            }

            int max = board.MaxBoardEvaluationValue - board.MoveCount;

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