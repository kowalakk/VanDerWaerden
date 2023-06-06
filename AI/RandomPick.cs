using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VanDerWaerden;

namespace Ai
{
    public class RandomPick : IAlgorithm
    {
        private int Seed { get; }
        private Random random;
        private Game Game { get; }

        public RandomPick(Game game, int seed = 2137)
        {
            Seed = seed;
            random = new Random(seed);
            Game = game;
        }

        public int ReturnNextMove(Node node)
        {
            var actions = Game.PossibleActions(node.CorespondingState).ToList();
            return actions[random.Next(actions.Count)];
        }
    }
}
