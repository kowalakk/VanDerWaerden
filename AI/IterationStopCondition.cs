namespace Ai
{
    public class IterationStopCondition : IStopCondition
    {
        private int MaxIterations { get; set; }
        private int CurrentIterations { get; set; }
        public IterationStopCondition(int iterations)
        {
            MaxIterations = CurrentIterations = iterations;
        }
        public void Advance()
        {
            CurrentIterations--;
        }
        public bool Occured()
        {
            if (CurrentIterations <= 0)
            {
                CurrentIterations = MaxIterations;
                return true;
            }
            return false;
        }
    }
}
