namespace VanDerWaerden
{
    public readonly struct State
    {
        public Player CurrentPlayer { get; }

        public readonly Player[] Numbers;

        public Dictionary<Player, Sequence> LongestSequences { get; }

        public State(int numbersCount)
        {
            Numbers = new Player[numbersCount];
            for (int i = 0; i < numbersCount; i++)
            {
                Numbers[i] = Player.None;
            }
            CurrentPlayer = Player.One;
            LongestSequences = new() { {Player.One,new(0,0,0) }, { Player.Two, new(0, 0, 0) } };
        }

        public State(Player[] numbers, Player currentPlayer, Dictionary<Player, Sequence> longestSequences)
        {
            Numbers = new Player[numbers.Length];
            for (int i = 0; i < numbers.Length; i++)
            {
                Numbers[i] = numbers[i];
            }
            CurrentPlayer = currentPlayer;
            LongestSequences = longestSequences;
        }
    }
}