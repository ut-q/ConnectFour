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
            get { return CurrentGameMode.Tuning.Width; }
        }
        
        public int Height 
        {
            get { return CurrentGameMode.Tuning.Height; }
        }

        public int NumberOfTilesToWin
        {
            get { return CurrentGameMode.Tuning.NumberOfTilesToWin; }
        }

        public GameMode CurrentGameMode { get; private set; }

        private SpaceState[,] _board;
        private int[] _heights;

        private static List<((int, int), (int, int))> _directions = new List<((int, int), (int, int))>
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

        public Board Clone()
        {
            Board clone = new Board();
            clone.Init(CurrentGameMode);
            
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

            return _board[row, column];
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

            _board[row, column] = newState;
            _heights[column]++;
            MoveCount++;
        }

        private void RemoveFromSpace(int column)
        {
            if (column >= Width || column < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(column), "Value is out of bounds, we shouldn't be here, something is wrong");
            }

            if (_board[0, column] == SpaceState.Empty)
            {
                throw new InvalidOperationException("Trying to pop from empty column, something is wrong");

            }

            for (int i = 0; i < Height - 1; ++i)
            {
                _board[i, column] = _board[i + 1, column];
            }

            _board[Height - 1, column] = SpaceState.Empty;
            _heights[column]--;
        }

        private int GetColumnHeight(int column)
        {
            if (column >= _heights.Length || column < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(column), "Value is out of bounds, we shouldn't be here, something is wrong");
            }

            return _heights[column];
        }

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

        public bool IsDraw()
        {
            return MoveCount >= Height * Width;
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

        private void ResetMoveCount()
        {
            MoveCount = 0;
        }

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
            foreach (var valueTuple in _directions)
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
        
        private static int[][] _evaluationTable =
        {
            new int[] { 3, 4, 5, 7, 5, 4, 3 }, new int[] { 4, 6, 8, 10, 8, 6, 4 }, new int[] { 5, 8, 11, 13, 11, 8, 5 },
            new int[] { 5, 8, 11, 13, 11, 8, 5 }, new int[] { 4, 6, 8, 10, 8, 6, 4 }, new int[] { 3, 4, 5, 7, 5, 4, 3 }
        };

        private static int _evaluationConstant = 138;
        private static int _maxValue = 1000;
        
        public static int MaxBoardEvaluationValue
        {
            get { return _maxValue; }
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
                        sum += _evaluationTable[row][column];
                    }
                    else if (cellSpaceState != SpaceState.Empty)
                    {
                        sum -= _evaluationTable[row][column];
                    }
                }
            }

            return _evaluationConstant + sum;
        }
    }
}