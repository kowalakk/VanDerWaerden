namespace Ai
{
    public interface IStopCondition
    {
        public bool Occured();
        public void Advance();
    }
}