namespace ConnectFour
{
    /// <summary>
    /// This represents an atomic move a player can take on the board
    /// </summary>
    public class Move
    {
        public enum Result
        {
            Invalid,
            Success,
            Fail
        }

        public enum Type
        {
            Invalid,
            PushTop,
            PopBottom
        }

        public Type MoveType { get; set; } = Type.PushTop;
        public Result MoveResult { get; set; } = Result.Invalid;
        public string Message { get; set; }
        public int MoveColumn { get; set; }
        public PlayerType PlayerType { get; set; }
    }
}