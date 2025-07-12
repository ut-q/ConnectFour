using System.Collections.Generic;

namespace ConnectFour
{
    public class PopOutGameMode : GameMode
    {
        public static readonly GameModeTuning GameModeTuning = new GameModeTuning()
        {
            Height = 6,
            Width = 7,
            NumberOfTilesToWin = 4,
            GameModeName = "Pop Out",
            GameModeExplanation = "A unique variant where players can remove the bottom disk as well if it belongs to them.\n" +
                                  "Use commands \"pop <column_name>\" or \"push <column_name>\" to play",
            EvaluationConstant = 138,
            MaxBoardEvaluationValue = 1000,
            EvaluationTable = new[,]
            {
                { 3, 4, 5, 7, 5, 4, 3 },
                { 4, 6, 8, 10, 8, 6, 4 },
                { 5, 8, 11, 13, 11, 8, 5 },
                { 5, 8, 11, 13, 11, 8, 5 },
                { 4, 6, 8, 10, 8, 6, 4 },
                { 3, 4, 5, 7, 5, 4, 3 }
            }
        };

        public override GameModeTuning Tuning
        {
            get { return GameModeTuning; }
        }
        
        public override List<Move> GetAllPossibleMoves(Board board, PlayerType playerType, int[]? columnOrder = null)
        {
            List<Move> moves = new List<Move>();
            for (int i = 0; i < board.Width; ++i)
            {
                Move pushMove = new Move()
                {
                    MoveColumn = columnOrder != null ? columnOrder[i] : i,
                    PlayerType = playerType,
                    MoveType = Move.Type.PushTop
                };
                if (board.CanMakeMove(pushMove))
                {
                    moves.Add(pushMove);
                }
                
                Move popMove = new Move()
                {
                    MoveColumn = columnOrder != null ? columnOrder[i] : i,
                    PlayerType = playerType,
                    MoveType = Move.Type.PopBottom
                };
                if (board.CanMakeMove(popMove))
                {
                    moves.Add(popMove);
                }
            }

            return moves;
        }
        
        public override Move GetMoveFromInput(string input, PlayerType playerType)
        {
            Move move = new Move
            {
                PlayerType = playerType
            };
            
            if (string.IsNullOrEmpty(input))
            {
                move.MoveResult = Move.Result.Fail;
                move.Message = "Can't read input - Use commands \"pop <column_name>\" or \"push <column_name>\" to play";
                move.MoveColumn = int.MaxValue;
            }
            else
            {
                string[] splitInput = input.Split();
                if (splitInput.Length != 2)
                {
                    move.MoveResult = Move.Result.Fail;
                    move.Message = "Can't read input - Use commands \"pop <column_name>\" or \"push <column_name>\" to play";
                    move.MoveColumn = int.MaxValue;
                }
                else
                {
                    Move.Type moveType = Move.Type.Invalid;
                    if (splitInput[0] == "pop")
                    {
                        moveType = Move.Type.PopBottom;
                    }
                    else if (splitInput[0] == "push")
                    {
                        moveType = Move.Type.PushTop;
                    }

                    if (moveType == Move.Type.Invalid)
                    {
                        move.MoveResult = Move.Result.Fail;
                        move.Message =
                            "Can't read input - Use commands \"pop <column_name>\" or \"push <column_name>\" to play";
                        move.MoveColumn = int.MaxValue;
                    }

                    if (!int.TryParse(splitInput[1], out int column))
                    {
                        move.MoveResult = Move.Result.Fail;
                        move.Message = "Can't read input - column value needs to be a positive integer";
                        move.MoveColumn = int.MaxValue;
                    }

                    if (move.MoveResult != Move.Result.Fail)
                    {
                        move.MoveResult = Move.Result.Success;
                        move.MoveType = moveType;
                        move.MoveColumn = column - 1;
                    }
                }
            }

            return move;
        }
    }
}