using Xunit;

namespace ConnectFour.Tests
{
    public class BoardTests
    {
        [Fact]
        public void NewBoard_IsEmpty()
        {
            var board = new Board();
            board.Init(new ClassicGameMode());
            for (int row = 0; row < board.Height; row++)
            for (int col = 0; col < board.Width; col++)
                Assert.Equal(Board.SpaceState.Empty, board.GetSpace(row, col));
        }

        [Fact]
        public void CanMakeMove_ValidMove_ReturnsTrue()
        {
            var board = new Board();
            board.Init(new ClassicGameMode());
            var move = new Move { MoveColumn = 0, PlayerType = PlayerType.Player1 };
            Assert.True(board.CanMakeMove(move));
        }

        [Fact]
        public void CanMakeMove_InvalidColumn_ReturnsFalse()
        {
            var board = new Board();
            board.Init(new ClassicGameMode());
            var move = new Move { MoveColumn = 100, PlayerType = PlayerType.Player1 };
            Assert.False(board.CanMakeMove(move));
        }

        [Fact]
        public void MakeMove_Player1Wins_Vertical()
        {
            var board = new Board();
            board.Init(new ClassicGameMode());
            for (int i = 0; i < 3; i++)
            {
                var move = new Move { MoveColumn = 0, PlayerType = PlayerType.Player1 };
                board.MakeMove(move);
            }
            var winningMove = new Move { MoveColumn = 0, PlayerType = PlayerType.Player1 };
            Assert.True(board.CheckIfMoveWinsPlayerTheGame(winningMove));
        }

        [Fact]
        public void IsDraw_FullBoard_ReturnsTrue()
        {
            var board = new Board();
            board.Init(new ClassicGameMode());
            // Fill the board with alternating moves
            for (int col = 0; col < board.Width; col++)
            for (int row = 0; row < board.Height - 1; row++)
                board.MakeMove(new Move { MoveColumn = col, PlayerType = (row % 2 == 0) ? PlayerType.Player1 : PlayerType.Player2 });
            Assert.True(board.IsDraw());
        }
    }
} 