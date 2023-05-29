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
        public State PerformActionButSlower(int action, State state)
        {
            Player[] players = (Player[])state.Numbers.Clone();
            players[action] = state.CurrentPlayer;
            Dictionary<Player, Sequence> newDict = new Dictionary<Player, Sequence>(state.LongestSequences);
            Sequence longestSequence = new(action, 0, 1);

            List<int> numbers = Enumerable.Range(0, state.Numbers.Length)
             .Where(i => state.Numbers[i] == state.CurrentPlayer || i == action)
             .ToList();
            
            if(numbers.Count == 1)
            {
                if (1 > newDict[state.CurrentPlayer].Length)
                    newDict[state.CurrentPlayer] = longestSequence;
                return new State(players, state.CurrentPlayer == Player.One ? Player.Two : Player.One, newDict);
            }

            int max_lenght = 1;
            for (int i = 0; i < numbers.Count; i++)
            {
                for (int j = i + 1; j < numbers.Count; j++)
                {
                    int lenght = 2;
                    int step = numbers[j] - numbers[i];
                    if (step + 1 > Math.Ceiling(NumbersCount * 1.0 / WinningSequenceCount)) continue; // Heuristics 
                    int k = j + 1;
                    while(k < numbers.Count && numbers[k] <= numbers[i] + lenght * step)
                    {
                        if(numbers[k] == numbers[i] + lenght * step)
                        {
                            lenght++;
                        }
                        k++;
                    }
                    if (lenght > max_lenght)
                    {
                        max_lenght = lenght;
                        longestSequence = new Sequence(numbers[i], step, lenght);
                    }
                }
            }

            if (longestSequence.Length > newDict[state.CurrentPlayer].Length)
                newDict[state.CurrentPlayer] = longestSequence;
            return new State(players, state.CurrentPlayer == Player.One ? Player.Two : Player.One, newDict);
        }

        public State NextState(int action, Player player, State state)
        {
            Player[] numbers = (Player[])state.Numbers.Clone();
            numbers[action] = player;
            Dictionary<Player, Sequence> newDict = new Dictionary<Player, Sequence>(state.LongestSequences);
            return new State(numbers, state.CurrentPlayer, newDict);
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
            if (PossibleActions(state).Count() == 0) // can optimize
                return GameResult.Draw;
            return GameResult.InProgress;
        }

    }
}