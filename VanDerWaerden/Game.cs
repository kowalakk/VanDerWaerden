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
        public int NumbersCount { get; }
        public int WinningSequenceCount { get; }

        public Game(int numbersCount, int winningSequenceCount)
        {
            NumbersCount = numbersCount;
            WinningSequenceCount = winningSequenceCount;
        }

        public State InitialState()
        {
            return new State(NumbersCount);
        }

        public Player CurrentPlayer(State state)
        {
            return state.CurrentPlayer;
        }

        public IEnumerable<int> PossibleActions(State state)
        {
            List<int> actions = new List<int>();
            for (int i = 0; i < NumbersCount; i++)
            {
                if (state.Numbers[i] == Player.None)
                    actions.Add(i);
            }
            return actions;
        }

        public State PerformAction(int action, State state)
        {
            Player[] numbers = (Player[])state.Numbers.Clone();
            numbers[action] = state.CurrentPlayer;
            bool[] checkedNumbers = new bool[NumbersCount];
            Sequence longestSequence = new(action, 0, 1);
            for (int step = 1; step < Math.Max(action + 1, NumbersCount - action); step++)
            {
                int sequenceLength = 1;
                int previousNumber = action - step;
                while (previousNumber >= 0 && !checkedNumbers[previousNumber]) // can optimize
                {
                    checkedNumbers[previousNumber] = true;
                    if (numbers[previousNumber] == state.CurrentPlayer)
                    {
                        sequenceLength++;
                        previousNumber -= step;
                    }
                }
                int nextNumber = action + step;
                while (nextNumber < NumbersCount && !checkedNumbers[nextNumber]) // can optimize
                {
                    checkedNumbers[nextNumber] = true;
                    if (numbers[nextNumber] == state.CurrentPlayer)
                    {
                        sequenceLength++;
                        nextNumber += step;
                    }
                }
                if (sequenceLength > longestSequence.Length)
                    longestSequence = new(previousNumber + step, step, sequenceLength);
            }
            return new State(numbers, state.CurrentPlayer == Player.One ? Player.Two : Player.One, longestSequence);
        }

        public GameResult Result(State state)
        {
            if (state.LongestSequence.Length >= WinningSequenceCount)
                return (GameResult)state.CurrentPlayer;
            if (PossibleActions(state).Count() == 0) // can optimize
                return GameResult.Draw;
            return GameResult.InProgress;
        }

    }
}