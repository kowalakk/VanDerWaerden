namespace VanDerWaerden
{
    public readonly struct State
    {
        public Player CurrentPlayer { get; }

        public readonly Player[] Numbers;

        public Sequence LongestSequence { get; }

        public State(int numbersCount)
        {
            Numbers = new Player[numbersCount];
            for (int i = 0; i < numbersCount; i++)
            {
                Numbers[i] = Player.None;
            }
            CurrentPlayer = Player.One;
            LongestSequence = new(0,0,0);
        }

        public State(Player[] numbers, Player currentPlayer, Sequence sequence)
        {
            Numbers = new Player[numbers.Length];
            for (int i = 0; i < numbers.Length; i++)
            {
                Numbers[i] = numbers[i];
            }
            CurrentPlayer = currentPlayer;
            LongestSequence = sequence;
        }
    }
}