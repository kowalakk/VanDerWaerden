﻿using VanDerWaerden;
using Action = VanDerWaerden.Action;

namespace Ai
{
    public class Uct
    { 
        private IStopCondition StopCondition { get; set; }
        private double UctConstant { get; }
        private Game Game { get; }

        public Uct(double uctConstant, IStopCondition condition, Game game)
        {
            UctConstant = uctConstant;
            StopCondition = condition;
            Game = game;
        }

        public List<(Action, double)> MoveAssessment(GameTree gameTree)
        {
            UctSearch(gameTree.SelectedNode);
            return gameTree.SelectedNode.ExpandedChildren
                .Select(child => (child.CorespondingAction!, -(double)child.SuccessCount / child.VisitCount))
                .ToList();
        }

        public Action ChooseAction(GameTree gameTree)
        {
            return MoveAssessment(gameTree)
                .MaxBy(action => { return action.Item2; })
                .Item1;
        }

        private void UctSearch(Node root)
        {
            var watch = new System.Diagnostics.Stopwatch();
            int iterations = 0;
            watch.Start();

            while (!StopCondition.StopConditionOccured())
            {
                Node node = TreePolicy(root);
                GameResult gameResult = DefaultPolicy(node.CorespondingState);
                Backup(node, gameResult);
                iterations++;
            }

            watch.Stop();
            Console.WriteLine($"UCT search execution time: {watch.ElapsedMilliseconds} ms" +
                $" - {(double)watch.ElapsedMilliseconds / iterations} ms/iteration");
        }

        private Node TreePolicy(Node node)
        {
            if (Game.Result(node.CorespondingState) == GameResult.InProgress)
            {
                Expand(node);
                if (node.UnexpandedChildren!.Any()) // node not fully expanded
                {
                    Node unexpandedChild = node.UnexpandedChildren!.Dequeue();
                    node.ExpandedChildren.Add(unexpandedChild);
                    return unexpandedChild;
                }
                // game is InProgress so a child exists
                return TreePolicy(BestChild(node)!);
            }
            return node;
        }

        private void Expand(Node node)
        {
            if (node.UnexpandedChildren == null)
            {
                IEnumerable<Action> possibleActions = Game.PossibleActions(node.CorespondingState);
                List<Node> unexpandedChildren = new();
                foreach (Action action in possibleActions)
                {
                    State childState = Game.PerformAction(action, node.CorespondingState);
                    Node childNode = new(action, childState, node);
                    unexpandedChildren.Add(childNode);
                }
                unexpandedChildren.Shuffle();
                node.UnexpandedChildren = new Queue<Node>(unexpandedChildren);
            }
        }

        private GameResult DefaultPolicy(State state)
        {
            GameResult gameResult = Game.Result(state);
            while (gameResult == GameResult.InProgress)
            {
                IEnumerable<Action> possibleActions = Game.PossibleActions(state);
                Action randomAction = possibleActions.RandomElement();
                state = Game.PerformAction(randomAction, state);
                gameResult = Game.Result(state);
            }
            return gameResult;
        }

        private void Backup(Node node, GameResult gameResult)
        {
            int delta = -1;
            if (gameResult == GameResult.Draw)
                delta = 0;
            if (gameResult == (GameResult)Game.CurrentPlayer(node.CorespondingState))
                delta = 1;
            Node? predecessor = node;
            while (predecessor != null)
            {
                predecessor.VisitCount++;
                predecessor.SuccessCount += delta;
                delta = -delta;
                predecessor = predecessor.Parent;
            }
        }

        private Node? BestChild(Node node)
        {
            return node.ExpandedChildren.MaxBy(ArgMax);
        }

        private double ArgMax(Node node)
        {
            return (double)node.SuccessCount / node.VisitCount + UctConstant * Math.Sqrt(2 * Math.Log(node.Parent!.VisitCount) / node.VisitCount);
        }

        public void MoveGameToNextState(GameTree gameTree, Action action)
        {
            gameTree.SelectChildNode(action);
        }

    }
}