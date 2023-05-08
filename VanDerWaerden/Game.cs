namespace VanDerWaerden
{
    public enum GameResult
    {
        InProgress = 0,
        PlayerOneWins = 1,
        PlayerTwoWins = 2,
        Draw = 3,
    }
    public enum Player
    {
        None = GameResult.InProgress,
        One = GameResult.PlayerOneWins,
        Two = GameResult.PlayerTwoWins,
    }
    public class Game
    {
        public int Numbers { get; }
        public Game(int numbers)
        {
            Numbers = numbers;
        }

        public State InitialState()
        {
            throw new NotImplementedException();
        }

        public Player CurrentPlayer(State state)
        {
            return state.CurrentPlayer;
        }

        public IEnumerable<Action> PossibleActions(State state)
        {
            throw new NotImplementedException();
        }

        public State PerformAction(Action action, State state)
        {
            throw new NotImplementedException();
        }

        public GameResult Result(State state)
        {
            throw new NotImplementedException();
        }

    }
}