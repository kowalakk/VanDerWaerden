using VanDerWaerden;
using Xunit;

namespace Tests
{
    public class PerformActionTests
    {
        Game game = new(8, 2);

        [Fact]
        public void BasicFunctionality()
        {
            State state = game.InitialState();

            state = game.PerformAction(2, state);
            Assert.Equal(Player.One, state.Numbers[2]);
            Assert.Equal(Player.Two, state.CurrentPlayer);
            Assert.Equal(new Sequence(2, 0, 1), state.LongestSequences[Player.One]);
            Assert.Equal(new Sequence(0, 0, 0), state.LongestSequences[Player.Two]);

            state = game.PerformAction(3, state);
            Assert.Equal(Player.One, state.Numbers[2]);
            Assert.Equal(Player.Two, state.Numbers[3]);
            Assert.Equal(Player.One, state.CurrentPlayer);
            Assert.Equal(new Sequence(2, 0, 1), state.LongestSequences[Player.One]);
            Assert.Equal(new Sequence(3, 0, 1), state.LongestSequences[Player.Two]);

            state = game.PerformAction(4, state);
            Assert.Equal(Player.One, state.Numbers[2]);
            Assert.Equal(Player.Two, state.Numbers[3]);
            Assert.Equal(Player.One, state.Numbers[4]);
            Assert.Equal(Player.Two, state.CurrentPlayer);
            Assert.Equal(new Sequence(2, 2, 2), state.LongestSequences[Player.One]);
            Assert.Equal(new Sequence(3, 0, 1), state.LongestSequences[Player.Two]);
        }
        [Fact]
        public void ChoosingLongestSequence()
        {
            State state = game.InitialState();

            state = game.PerformAction(2, state);
            state = game.PerformAction(3, state);
            Assert.Equal(new Sequence(2, 0, 1), state.LongestSequences[Player.One]);
            Assert.Equal(new Sequence(3, 0, 1), state.LongestSequences[Player.Two]);
            state = game.PerformAction(4, state);
            state = game.PerformAction(6, state);
            Assert.Equal(new Sequence(2, 2, 2), state.LongestSequences[Player.One]);
            Assert.Equal(new Sequence(3, 3, 2), state.LongestSequences[Player.Two]);
            state = game.PerformAction(1, state);
            state = game.PerformAction(5, state);
            Assert.Equal(new Sequence(2, 2, 2), state.LongestSequences[Player.One]);
            Assert.Equal(new Sequence(3, 3, 2), state.LongestSequences[Player.Two]);
            state = game.PerformAction(0, state);
            state = game.PerformAction(7, state);
            Assert.Equal(new Sequence(0, 1, 3), state.LongestSequences[Player.One]);
            Assert.Equal(new Sequence(5, 1, 3), state.LongestSequences[Player.Two]);


        }
    }
}