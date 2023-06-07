using VanDerWaerden;

namespace Ai
{
    public class GameTree
    {
        public Node Root { get; private set; }
        public Node SelectedNode { get; private set; }
        public Game Game { get; }

        public GameTree(State state, Game game)
        {
            Root = new Node(state);
            SelectedNode = Root;
            Game = game;
        }

        public void SelectChildNode(int action)
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

        public void MoveGameToNextState(int action)
        {
            State childState = Game.PerformAction(action, SelectedNode.CorespondingState);
            Node node = new Node(action, childState, SelectedNode);
            SelectedNode = node;
        }
    }
}