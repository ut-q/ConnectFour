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

        public int Width = 7;
        public int Height = 6;

        private const int NumberOfTilesToWin = 4;

        private readonly SpaceState[,] _board;

        public Board()
        {
            _board = new SpaceState[Height, Width];
            for (int i = 0; i < _board.GetLength(0); ++i)
            {
                for (int j = 0; j < _board.GetLength(1); ++j)
                {
                    _board[i, j] = SpaceState.Empty;
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

        private void SetSpace(int row, int column, SpaceState newState)
        {
            if (row >= Height || row < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(row), "Value is out of bounds, we shouldn't be here, something is wrong");
            }

            if (column >= Width || column < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(column), "Value is out of bounds, we shouldn't be here, something is wrong");
            }

            _board[row, column] = newState;
        }

        private int GetColumnHeight(int column)
        {
            if (column >= Width)
            {
                throw new ArgumentOutOfRangeException(nameof(column), "Value is out of bounds, we shouldn't be here, something is wrong");
            }

            int i; 
            for(i = 0; i < Height; ++i)
            {
                if (_board[i, column] == SpaceState.Empty)
                {
                    break;
                }
            }

            return i;
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
            SetSpace(currentHeight, move.MoveColumn, ConvertMoveToSpaceState(move));
        }
        
        private static SpaceState ConvertMoveToSpaceState(Move move)
        {
            if (move.PlayerType == PlayerType.Player1)
            {
                return SpaceState.Player1;
            }
            else
            {
                return SpaceState.Player2;
            }
        }

        public bool CheckForGameOver(Move move)
        {
            // check for NumberOfTilesToWin or more
            List<((int, int), (int, int))> directions = new List<((int, int), (int, int))>();
            directions.Add(((-1,0),(1,0)));
            directions.Add(((0,-1),(0,1)));
            directions.Add(((-1,-1),(1,1)));
            directions.Add(((-1,1),(1,-1)));
            int column = move.MoveColumn;
            int height = Math.Max(GetColumnHeight(column) - 1,0);
            SpaceState spaceState = ConvertMoveToSpaceState(move);

            foreach (var valueTuple in directions)
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