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
            return state.Numbers[Player.None];
        }

        public State NextState(int action, Player player, State state)
        {
            Dictionary<Player, HashSet<int>> numbers = new Dictionary<Player, HashSet<int>>
            {
                [Player.None] = new HashSet<int>(state.Numbers[Player.None]),
                [Player.One] = new HashSet<int>(state.Numbers[Player.One]),
                [Player.Two] = new HashSet<int>(state.Numbers[Player.Two])
            };
            numbers[Player.None].Remove(action);
            numbers[state.CurrentPlayer].Add(action);
            Dictionary<Player, Sequence> newDict = new Dictionary<Player, Sequence>(state.LongestSequences);
            return new State(numbers, state.CurrentPlayer, newDict);
        }

        public State PerformAction(int action, State state)
        {
            Dictionary<Player, HashSet<int>> numbers = new Dictionary<Player, HashSet<int>>
            {
                [Player.None] = new HashSet<int>(state.Numbers[Player.None]),
                [Player.One] = new HashSet<int>(state.Numbers[Player.One]),
                [Player.Two] = new HashSet<int>(state.Numbers[Player.Two])
            };
            numbers[Player.None].Remove(action);
            numbers[state.CurrentPlayer].Add(action);
            bool[] checkedNumbers = new bool[NumbersCount];
            Sequence longestSequence = new(action, 0, 1);
            for (int step = 1; step < Math.Max(action + 1, NumbersCount - action); step++)
            {
                int sequenceLength = 1;
                int previousNumber = action - step;
                while (previousNumber >= 0 && !checkedNumbers[previousNumber]) // can optimize
                {
                    checkedNumbers[previousNumber] = true;
                    if (numbers[state.CurrentPlayer].Contains(previousNumber))
                    {
                        sequenceLength++;
                        previousNumber -= step;
                    }
                }
                int nextNumber = action + step;
                while (nextNumber < NumbersCount && !checkedNumbers[nextNumber]) // can optimize
                {
                    checkedNumbers[nextNumber] = true;
                    if (numbers[state.CurrentPlayer].Contains(nextNumber))
                    {
                        sequenceLength++;
                        nextNumber += step;
                    }
                }
                if (sequenceLength > longestSequence.Length)
                    longestSequence = new(previousNumber + step, step, sequenceLength);
            }
            Dictionary<Player, Sequence> newDict = new Dictionary<Player, Sequence>(state.LongestSequences);
            if (longestSequence.Length > newDict[state.CurrentPlayer].Length)
                newDict[state.CurrentPlayer] = longestSequence;
            return new State(numbers, state.CurrentPlayer == Player.One ? Player.Two : Player.One, newDict);
        }

        public GameResult Result(State state)
        {
            if (state.LongestSequences[Player.One].Length >= WinningSequenceCount)
                return GameResult.PlayerOneWins;
            if (state.LongestSequences[Player.Two].Length >= WinningSequenceCount)
                return GameResult.PlayerTwoWins;
            if (!PossibleActions(state).Any())
                return GameResult.Draw;
            return GameResult.InProgress;
        }

        public int Heuristic(State state)
        {
            if (Result(state) == GameResult.InProgress)
            {
                int shortsightedBestAction = PossibleActions(state).MaxBy(action =>
                    { return PerformAction(action, state).LongestSequences[state.CurrentPlayer].Length; }
                    );
                return -Heuristic(PerformAction(shortsightedBestAction, state));
            }
            else if (Result(state) == GameResult.Draw)
                return 0;
            else if (Result(state) == (GameResult)CurrentPlayer(state))
                return 1;
            else
                return -1;
        }
    }
}