using Xunit;

namespace ConnectFour.Tests
{
    public class GameModeTests
    {
        [Fact]
        public void ClassicGameMode_ValidInput_ReturnsValidMove()
        {
            var gameMode = new ClassicGameMode();
            var move = gameMode.GetMoveFromInput("3", PlayerType.Player1);
            Assert.Equal(Move.Result.Success, move.MoveResult);
            Assert.Equal(2, move.MoveColumn); // 3-1 = 2
        }

        [Fact]
        public void ClassicGameMode_InvalidInput_ReturnsFailedMove()
        {
            var gameMode = new ClassicGameMode();
            var move = gameMode.GetMoveFromInput("abc", PlayerType.Player1);
            Assert.Equal(Move.Result.Fail, move.MoveResult);
        }

        [Fact]
        public void PopOutGameMode_ValidPushInput_ReturnsValidMove()
        {
            var gameMode = new PopOutGameMode();
            var move = gameMode.GetMoveFromInput("push 3", PlayerType.Player1);
            Assert.Equal(Move.Result.Success, move.MoveResult);
            Assert.Equal(Move.Type.PushTop, move.MoveType);
            Assert.Equal(2, move.MoveColumn);
        }

        [Fact]
        public void PopOutGameMode_ValidPopInput_ReturnsValidMove()
        {
            var gameMode = new PopOutGameMode();
            var move = gameMode.GetMoveFromInput("pop 3", PlayerType.Player1);
            Assert.Equal(Move.Result.Success, move.MoveResult);
            Assert.Equal(Move.Type.PopBottom, move.MoveType);
            Assert.Equal(2, move.MoveColumn);
        }

        [Fact]
        public void PopOutGameMode_InvalidInput_ReturnsFailedMove()
        {
            var gameMode = new PopOutGameMode();
            var move = gameMode.GetMoveFromInput("invalid command", PlayerType.Player1);
            Assert.Equal(Move.Result.Fail, move.MoveResult);
        }
    }
} 