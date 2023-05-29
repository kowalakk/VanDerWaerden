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
      /*  [Fact]
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


        }*/
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
            Assert.Equal(new Sequence(3, 2, 3), state.LongestSequences[Player.Two]);

        }

        [Fact]
        public void LongestSequenceOnePlayer()
        {
            Game game = new Game(101, 10);
            State state = game.InitialState();

            state = game.NextState(80, Player.One, state);
            state = game.NextState(5, Player.One, state);
            state = game.NextState(28, Player.One, state);
            state = game.NextState(77, Player.One, state);
            state = game.NextState(78, Player.One, state);
            state = game.NextState(47, Player.One, state);
            state = game.NextState(94, Player.One, state);
            state = game.NextState(38, Player.One, state);
            state = game.NextState(37, Player.One, state);
            state = game.NextState(36, Player.One, state);
            state = game.NextState(97, Player.One, state);
            state = game.NextState(4, Player.One, state);
            state = game.NextState(84, Player.One, state);
            state = game.NextState(76, Player.One, state);
            state = game.NextState(13, Player.One, state);
            state = game.NextState(75, Player.One, state);
            state = game.NextState(61, Player.One, state);
            state = game.NextState(43, Player.One, state);
            state = game.NextState(23, Player.One, state);
            state = game.NextState(52, Player.One, state);
            state = game.NextState(60, Player.One, state);
            state = game.NextState(20, Player.One, state);
            state = game.PerformAction(44, state);
            Assert.Equal(new Sequence(20, 8, 6), state.LongestSequences[Player.One]);
        }
    }
}