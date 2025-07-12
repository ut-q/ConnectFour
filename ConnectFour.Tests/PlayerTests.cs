using Xunit;

namespace ConnectFour.Tests
{
    public class PlayerTests
    {
        [Fact]
        public void HumanPlayer_Initialization_SetsCorrectProperties()
        {
            var board = new Board();
            board.Init(new ClassicGameMode());
            var player = new HumanPlayer(PlayerType.Player1, board);
            
            Assert.Equal(PlayerType.Player1, player.PlayerType);
            Assert.Equal("Player1", player.Name);
            Assert.Contains("Type: Human", player.PlayerInfoString);
        }

        [Fact]
        public void AIPlayer_Initialization_SetsCorrectProperties()
        {
            var board = new Board();
            board.Init(new ClassicGameMode());
            var player = new AIPlayer(PlayerType.Player2, board, AIDifficultyTuning.MediumTuning);
            
            Assert.Equal(PlayerType.Player2, player.PlayerType);
            Assert.Equal("Player2", player.Name);
            Assert.Contains("Type: Ai", player.PlayerInfoString);
            Assert.Contains("Difficulty: Medium", player.PlayerInfoString);
        }

        [Fact]
        public void AIPlayer_MakeMove_ReturnsValidMove()
        {
            var board = new Board();
            board.Init(new ClassicGameMode());
            var player = new AIPlayer(PlayerType.Player1, board, AIDifficultyTuning.EasyTuning);
            player.Init();
            
            var move = player.MakeMove();
            Assert.NotNull(move);
            Assert.Equal(PlayerType.Player1, move.PlayerType);
            Assert.True(move.MoveColumn >= 0 && move.MoveColumn < board.Width);
        }
    }
} 