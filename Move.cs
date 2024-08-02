namespace ConnectFour
{
    public class Move
    {
        public enum MoveResult
        {
            Success,
            Fail
        }
        
        public MoveResult Result { get; set; }
        public string Message { get; set; }
        public int MoveColumn { get; set; }
        public PlayerType PlayerType { get; set; }
    }
}