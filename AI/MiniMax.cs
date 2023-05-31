using VanDerWaerden;

namespace Ai
{
    public class MiniMax
    {
        private Game Game { get; }
        private int Depth { get; set; }
        public MiniMax(Game game, int depth)
        {
            Depth = depth;
            Game = game;
        }
        public int ReturnNextMove(Node node)
        {
            return Search(node.CorespondingState);
        }
        public int Search(State state)
        {
            int bestScore = int.MinValue;
            int bestAction = default;
            foreach (int action in Game.PossibleActions(state))
            {
                State newState = Game.PerformAction(action, state);
                int score = -NegaMax(newState, Depth);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestAction = action;
                }
            }
            return bestAction;
        }

        private int NegaMax(State state, int depth)
        {
            if (depth == 0)
                return Game.Heuristic(state);
            if (Game.Result(state) == GameResult.InProgress)
            {
                int bestScore = -1;
                foreach (int action in Game.PossibleActions(state))
                {
                    State newState = Game.PerformAction(action, state);
                    int score = -NegaMax(newState, depth - 1);
                    if (score > bestScore)
                    {
                        bestScore = score;
                    }
                }
                return bestScore;
            }
            else if (Game.Result(state) == GameResult.Draw)
                return 0;
            else if (Game.Result(state) == (GameResult)Game.CurrentPlayer(state))
                return 1;
            else
                return -1;
        }
    }
}
