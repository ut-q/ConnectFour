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
        
        public int MoveCount
        {
            get { return _moveCount; }
        }

        public readonly int Width = 7;
        public readonly int Height = 6;

        public readonly int NumberOfTilesToWin = 4;

        private readonly SpaceState[,] _board;
        private readonly int[] _heights;
        private int _moveCount;
        
        private static List<((int, int), (int, int))> _directions = new List<((int, int), (int, int))>
        {
            ((-1,0),(1,0)),
            ((0,-1),(0,1)),
            ((-1,-1),(1,1)),
            ((-1,1),(1,-1))
        };

        public Board()
        {
            _board = new SpaceState[Height, Width];
            for (int i = 0; i < Height; ++i)
            {
                for (int j = 0; j < Width; ++j)
                {
                    _board[i, j] = SpaceState.Empty;
                }
            }

            _heights = new int[Width];
            _moveCount = 0;
        }

        public Board(Board board)
        {
            _board = new SpaceState[Height, Width];
            _heights = new int[Width];
            
            Width = board.Width;
            Height = board.Height;
            NumberOfTilesToWin = board.NumberOfTilesToWin;
            
            //TODO this will need optimization anyways
            
            for (int i = 0; i < Height; ++i)
            {
                for (int j = 0; j < Width; ++j)
                {
                    SpaceState spaceState = board.GetSpace(i, j);
                    if (spaceState == SpaceState.Empty)
                    {
                        _board[i, j] = SpaceState.Empty;
                    }
                    else
                    {
                        InsertToSpace(i,j,spaceState);
                    }
                }
            }
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
            _moveCount++;
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
                move.Result = Move.MoveResult.Fail;
                move.Message = $"Please select a value smaller than {Width} and larger than 0 for column";
            }
            else if (GetColumnHeight(column) >= Width - 1)
            {
                move.Result = Move.MoveResult.Fail;
                move.Message = $"Can't insert to a full column";
            }
            
            return move.Result == Move.MoveResult.Success;
        }

        public void MakeMove(Move move)
        {
            int currentHeight = GetColumnHeight(move.MoveColumn);
            InsertToSpace(currentHeight, move.MoveColumn, ConvertPlayerToSpaceState(move.PlayerType));
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
            return _moveCount >= Height * Width;
        }

        public Board GetCopyForAiIteration(bool resetMoveCount = false)
        {
            Board b = new Board(this);
            if (resetMoveCount)
            {
                b.ResetMoveCount();
            }
            return b;
        }

        private void ResetMoveCount()
        {
            _moveCount = 0;
        }

        public bool CheckIfMoveWinsPlayerTheGame(Move move)
        {
            int column = move.MoveColumn;
            int height = Math.Max(GetColumnHeight(column),0);
            SpaceState spaceState = ConvertPlayerToSpaceState(move.PlayerType);

            foreach (var valueTuple in _directions)
            {
                int count = 1;
                int columnAddition = valueTuple.Item1.Item1;
                int rowAddition = valueTuple.Item1.Item2;
                int columnCheck = column + columnAddition;
                int rowCheck = height + rowAddition;
                while (columnCheck >= 0 && columnCheck < Width && rowCheck >= 0 && rowCheck < Height
                       && _board[rowCheck, columnCheck] == spaceState)
                {
                    count++;
                    columnCheck += columnAddition;
                    rowCheck += rowAddition;
                }
                
                columnAddition = valueTuple.Item2.Item1;
                rowAddition = valueTuple.Item2.Item2;
                columnCheck = column + columnAddition;
                rowCheck = height + rowAddition;

                while (columnCheck >= 0 && columnCheck < Width && rowCheck >= 0 && rowCheck < Height
                       && _board[rowCheck, columnCheck] == spaceState)
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
    }
}