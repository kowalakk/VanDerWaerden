namespace Ai
{
    public class GameTree
    {
        public Node Root { get; private set; }
        public Node SelectedNode { get; private set; }

        public GameTree(State state)
        {
            Root = new Node(state);
            SelectedNode = Root;
        }

        public void SelectChildNode(Action action)
        {
            foreach (Node child in SelectedNode.ExpandedChildren)
            {
                if (action!.Equals(child.CorespondingAction))
                {
                    SelectedNode = child;
                    return;
                }
            }
            throw new ArgumentException();
        }
    }
}