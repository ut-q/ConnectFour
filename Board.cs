using System;
using System.Collections.Generic;

namespace ConnectFour
{
    public class Board
    {
        public enum SpaceState
        {
            Empty,
            Player1,
            Player2,
        }
        
        public int MoveCount { get; private set; }

        public int Width
        {
            get { return CurrentGameMode!.Tuning.Width; }
        }
        
        public int Height 
        {
            get { return CurrentGameMode!.Tuning.Height; }
        }

        private int NumberOfTilesToWin
        {
            get { return CurrentGameMode!.Tuning.NumberOfTilesToWin; }
        }
                
        public int MaxBoardEvaluationValue
        {
            get { return CurrentGameMode!.Tuning.MaxBoardEvaluationValue; }
        }

        public GameMode? CurrentGameMode { get; private set; }

        /// <summary>
        /// This matrix is the data representation of the board
        /// </summary>
        private SpaceState[,]? _board;
        /// <summary>
        /// this stores the height of each column for easy calculation
        /// </summary>
        private int[]? _heights;

        private static readonly List<((int, int), (int, int))> Directions = new List<((int, int), (int, int))>
        {
            ((-1,0),(1,0)),
            ((0,-1),(0,1)),
            ((-1,-1),(1,1)),
            ((-1,1),(1,-1))
        };

        public void Init(GameMode gameMode)
        {
            CurrentGameMode = gameMode;
            _board = new SpaceState[Height, Width];
            for (int i = 0; i < Height; ++i)
            {
                for (int j = 0; j < Width; ++j)
                {
                    _board[i, j] = SpaceState.Empty;
                }
            }
            
            CurrentGameMode.InitializeBoard(this);

            _heights = new int[Width];
            MoveCount = 0;
        }

        /// <summary>
        /// Returns a clone of the board with identical state
        /// </summary>
        private Board Clone()
        {
            Board clone = new Board();
            clone.Init(CurrentGameMode!);
            
            for (int i = 0; i < Height; ++i)
            {
                for (int j = 0; j < Width; ++j)
                {
                    SpaceState spaceState = GetSpace(i, j);
                    if (spaceState != SpaceState.Empty)
                    {
                        clone.InsertToSpace(i,j,spaceState);
                    }
                }
            }

            return clone;
        }

        public SpaceState GetSpace(int row, int column)
        {
            if (row >= Height || row < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(row), "Value is out of bounds, we shouldn't be here, something is wrong");
            }

            if (column >= Width || column < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(column), "Value is out of bounds, we shouldn't be here, something is wrong");
            }

            return _board![row, column];
        }

        private void InsertToSpace(int row, int column, SpaceState newState)
        {
            if (row >= Height || row < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(row), "Value is out of bounds, we shouldn't be here, something is wrong");
            }

            if (column >= Width || column < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(column), "Value is out of bounds, we shouldn't be here, something is wrong");
            }

            if (newState == SpaceState.Empty)
            {
                throw new ArgumentException("Can't insert \"Empty Space\" into a space");
            }

            _board![row, column] = newState;
            _heights![column]++;
            MoveCount++;
        }

        private void RemoveFromSpace(int column)
        {
            if (column >= Width || column < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(column), "Value is out of bounds, we shouldn't be here, something is wrong");
            }

            if (_board![0, column] == SpaceState.Empty)
            {
                throw new InvalidOperationException("Trying to pop from empty column, something is wrong");

            }

            for (int i = 0; i < Height - 1; ++i)
            {
                _board![i, column] = _board![i + 1, column];
            }

            _board![Height - 1, column] = SpaceState.Empty;
            _heights![column]--;
            MoveCount++;
        }

        private int GetColumnHeight(int column)
        {
            if (column >= _heights!.Length || column < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(column), "Value is out of bounds, we shouldn't be here, something is wrong");
            }

            return _heights[column];
        }

        /// <summary>
        /// Checks if the given move is valid on this board
        /// </summary>
        public bool CanMakeMove(Move move)
        {
            int column = move.MoveColumn;
            if (column >= Width || column < 0)
            {
                move.MoveResult = Move.Result.Fail;
                move.Message = $"Please select a value smaller than {Width} and larger than 0 for column";
            }
            else if (move.MoveType == Move.Type.PushTop)
            {
                if (GetColumnHeight(column) >= Width - 1)
                {
                    move.MoveResult = Move.Result.Fail;
                    move.Message = $"Can't insert to a full column";
                }
                else
                {
                    move.MoveResult = Move.Result.Success;
                }
            }
            else if (move.MoveType == Move.Type.PopBottom)
            {
                SpaceState expected = ConvertPlayerToSpaceState(move.PlayerType);
                if (GetSpace(0, column) != expected)
                {
                    move.MoveResult = Move.Result.Fail;
                    move.Message = $"Can't remove from column, the bottom tile of the selected column should be yours";
                }
                else
                {
                    move.MoveResult = Move.Result.Success;
                }
            }
            
            return move.MoveResult == Move.Result.Success;
        }

        /// <summary>
        /// Executes the given move on the board
        /// </summary>
        public void MakeMove(Move move)
        {
            if (move.MoveType == Move.Type.PushTop)
            {
                int currentHeight = GetColumnHeight(move.MoveColumn);
                InsertToSpace(currentHeight, move.MoveColumn, ConvertPlayerToSpaceState(move.PlayerType));
            }
            else if (move.MoveType == Move.Type.PopBottom)
            {
                RemoveFromSpace(move.MoveColumn);
            }
        }

        public static SpaceState ConvertPlayerToSpaceState(PlayerType playerType)
        {
            if (playerType == PlayerType.Player1)
            {
                return SpaceState.Player1;
            }
            else
            {
                return SpaceState.Player2;
            }
        }

        /// <summary>
        /// It's a draw only if the board is full
        /// </summary>
        /// <returns></returns>
        public bool IsDraw()
        {
            foreach (int height in _heights!)
            {
                if (height < Height - 1)
                {
                    return false;
                }
            }
            return true;
        }

        public Board GetCopyForAiIteration(bool resetMoveCount = false)
        {
            Board b = Clone();
            if (!resetMoveCount)
            {
                b.MoveCount = MoveCount;
            }
            return b;
        }

        /// <summary>
        /// Checks if the player wins the game after the given move
        /// </summary>
        public bool CheckIfMoveWinsPlayerTheGame(Move move)
        {
            int column = move.MoveColumn;
            SpaceState spaceState = ConvertPlayerToSpaceState(move.PlayerType);

            if (move.MoveType == Move.Type.PushTop)
            {
                int row = Math.Max(GetColumnHeight(column),0);
                return CheckIfCellEndsTheGame_Internal(row, column, spaceState);
            }
            else // PopBottom
            {
                Board cloneBoard = Clone();
                cloneBoard.MakeMove(move);

                for (int i = 0; i < cloneBoard.Height; ++i)
                {
                    if (cloneBoard.GetSpace(i, column) == spaceState)
                    {
                        bool success = cloneBoard.CheckIfCellEndsTheGame_Internal(i, column, spaceState);
                        if (success)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        private bool CheckIfCellEndsTheGame_Internal(int row, int column, SpaceState spaceState)
        {
            foreach (((int, int), (int, int)) valueTuple in Directions)
            {
                int count = 1;
                int columnAddition = valueTuple.Item1.Item1;
                int rowAddition = valueTuple.Item1.Item2;
                int columnCheck = column + columnAddition;
                int rowCheck = row + rowAddition;
                while (columnCheck >= 0 && columnCheck < Width && rowCheck >= 0 && rowCheck < Height
                       && GetSpace(rowCheck, columnCheck) == spaceState)
                {
                    count++;
                    columnCheck += columnAddition;
                    rowCheck += rowAddition;
                }
                
                columnAddition = valueTuple.Item2.Item1;
                rowAddition = valueTuple.Item2.Item2;
                columnCheck = column + columnAddition;
                rowCheck = row + rowAddition;

                while (columnCheck >= 0 && columnCheck < Width && rowCheck >= 0 && rowCheck < Height
                       && GetSpace(rowCheck, columnCheck) == spaceState)
                {
                    count++;
                    columnCheck += columnAddition;
                    rowCheck += rowAddition;
                }
                if (count >= NumberOfTilesToWin)
                {
                    return true;
                }
            }

            return false;
        }

        public int GetBoardEvaluationScore(PlayerType playerType)
        {
            int sum = 0;
            SpaceState spaceState = ConvertPlayerToSpaceState(playerType);
            for (int row = 0; row < Height; row++)
            {
                for (int column = 0; column < Width; column++)
                {
                    SpaceState cellSpaceState = GetSpace(row, column);
                    if (cellSpaceState == spaceState)
                    {
                        sum += CurrentGameMode!.Tuning.EvaluationTable[row,column];
                    }
                    else if (cellSpaceState != SpaceState.Empty)
                    {
                        sum -= CurrentGameMode!.Tuning.EvaluationTable[row,column];
                    }
                }
            }

            return CurrentGameMode!.Tuning.EvaluationConstant + sum;
        }
    }
}