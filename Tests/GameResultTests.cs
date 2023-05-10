using VanDerWaerden;
using Xunit;

namespace Tests
{
    public class GameResultTests
    {
        Game game = new(5, 3);

        [Fact]
        public void GameToWin()
        {
            State state = game.InitialState();

            state = game.PerformAction(0, state);
            Assert.Equal(GameResult.InProgress, game.Result(state));
            state = game.PerformAction(3, state);
            Assert.Equal(GameResult.InProgress, game.Result(state));
            state = game.PerformAction(1, state);
            Assert.Equal(GameResult.InProgress, game.Result(state));
            state = game.PerformAction(4, state);
            Assert.Equal(GameResult.InProgress, game.Result(state));
            state = game.PerformAction(2, state);
            Assert.Equal(GameResult.PlayerOneWins, game.Result(state));

        }
        [Fact]
        public void GameToDraw()
        {
            State state = game.InitialState();

            state = game.PerformAction(0, state);
            Assert.Equal(GameResult.InProgress, game.Result(state));
            state = game.PerformAction(3, state);
            Assert.Equal(GameResult.InProgress, game.Result(state));
            state = game.PerformAction(1, state);
            Assert.Equal(GameResult.InProgress, game.Result(state));
            state = game.PerformAction(2, state);
            Assert.Equal(GameResult.InProgress, game.Result(state));
            state = game.PerformAction(4, state);
            Assert.Equal(GameResult.Draw, game.Result(state));


        }
    }
}