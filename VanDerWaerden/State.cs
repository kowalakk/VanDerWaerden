namespace VanDerWaerden
{
    public class State
    {
        public Player CurrentPlayer { get; set; }

        public List<int[]>[] ArithmeticSequences;

        public State(int numbers)
        {
            ArithmeticSequences = new List<int[]>[numbers - 1];
        }
    }
}