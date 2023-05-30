namespace VanDerWaerden
{
    public record struct Sequence(int FirstElement, int Step, int Length);

    public readonly record struct State
    {
        public Player CurrentPlayer { get; }

        public Dictionary<Player, HashSet<int>> Numbers { get; }

        public Dictionary<Player, Sequence> LongestSequences { get; }

        public State(int numbersCount)
        {
            Numbers = new Dictionary<Player, HashSet<int>>
            {
                [Player.None] = new HashSet<int>(),
                [Player.One] = new HashSet<int>(),
                [Player.Two] = new HashSet<int>()
            };
            for (int i = 0; i < numbersCount; i++)
            {
                Numbers[Player.None].Add(i);
            }
            CurrentPlayer = Player.One;
            LongestSequences = new() { {Player.One,new(0,0,0) }, { Player.Two, new(0, 0, 0) } };
        }

        public State(Dictionary<Player, HashSet<int>> numbers, Player currentPlayer, Dictionary<Player, Sequence> longestSequences)
        {
            Numbers = numbers;
            CurrentPlayer = currentPlayer;
            LongestSequences = longestSequences;
        }
    }
}