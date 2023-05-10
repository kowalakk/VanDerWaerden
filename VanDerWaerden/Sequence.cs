namespace VanDerWaerden
{
    public struct Sequence
    {
        public int FirstElement;
        public int Step;
        public int Length;

        public Sequence(int firstElement, int step, int length)
        {
            FirstElement = firstElement;
            Step = step;
            Length = length;
        }
    }
}