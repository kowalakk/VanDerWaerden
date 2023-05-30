using VanDerWaerden;

namespace Ai
{
    public class MiniMax
    {
        private IStopCondition StopCondition { get; set; }
        private Game Game { get; }
        public MiniMax(IStopCondition condition, Game game)
        {
            StopCondition = condition;
            Game = game;
        }
        public int Search(State state)
        {
            if (StopCondition.Occured())
                return 0;
            int bestScore = int.MinValue;
            int bestAction = default;
            int score;
            IList<int> actions = Game.PossibleActions(state).ToList();
            actions.Shuffle();
            foreach (int action in actions)
            {
                State newState = Game.PerformAction(action, state);
                score = -NegaMax(newState);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestAction = action;
                }
            }
            return bestAction;
        }

        private int NegaMax(State state)
        {
            if (StopCondition.Occured())
                return 0;
            if (Game.Result(state) == GameResult.InProgress)
            {
                int bestScore = int.MinValue;
                int score;
                IList<int> actions = Game.PossibleActions(state).ToList();
                actions.Shuffle();
                foreach (int action in actions)
                {
                    State newState = Game.PerformAction(action, state);
                    score = -NegaMax(newState);
                    if (score > bestScore)
                    {
                        bestScore = score;
                    }
                }
                return bestScore;
            }
            else
            {
                StopCondition.Advance();
                if (Game.Result(state) == GameResult.Draw)
                    return 0;
                else if (Game.Result(state) == (GameResult)Game.CurrentPlayer(state))
                    return 1;
                else
                    return -1;
            }
        }
    }
}
