using VanDerWaerden;

namespace Ai
{
    public record Node
    {
        public int? CorespondingAction { get; set; }
        public State CorespondingState { get; set; }
        public Node? Parent { get; set; }
        public List<Node> ExpandedChildren { get; set; }
        public Queue<Node>? UnexpandedChildren { get; set; }
        public long VisitCount { get; set; }
        public long SuccessCount { get; set; }
        public Node(State state)
        {
            CorespondingState = state;
            CorespondingAction = default;
            ExpandedChildren = new List<Node>();
            UnexpandedChildren = null;
            Parent = null;
            VisitCount = 0;
            SuccessCount = 0;
        }
        public Node(int action, State state, Node? parent = null)
        {
            CorespondingAction = action;
            CorespondingState = state;
            ExpandedChildren = new List<Node>();
            UnexpandedChildren = null;
            Parent = parent;
            VisitCount = 0;
            SuccessCount = 0;
        }

    }
}