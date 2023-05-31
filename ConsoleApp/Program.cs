using Ai;
using System.Collections.Generic;
using System.Diagnostics;
using VanDerWaerden;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Program rozegra g gier Van der Waerdena");
        int gamesCount = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Podaj długość zbioru dostępnych liczb");
        int numbersCount = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Podaj długość wygrywającego ciągu arytmetycznego");
        int winningSequenceCount = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Jakim algorytmem ma grać pierwszy gracz?[UCT/MinMax/Random]");
        string firstAlg = Console.ReadLine()!;
        Console.WriteLine("Jakim algorytmem ma grać drugi gracz?[UCT/MinMax/Random]");
        string secondAlg = Console.ReadLine()!;

        for (int i = 0; i < gamesCount; i++)
        {
            Game game = new(numbersCount, winningSequenceCount);

            IAlgorithm playerOne = new Uct(1.414, new IterationStopCondition(1000), game);
            IAlgorithm playerTwo = new RandomPick(game);
            //IAlgorithm playerTwo = new MiniMax(new IterationStopCondition(1000), game);

            State currentState = game.InitialState();
            GameTree gameTree = new(currentState, game);

            GameResult gameResult = game.Result(currentState);
            while (gameResult == GameResult.InProgress)
            {
                IAlgorithm currentPlayer = currentState.CurrentPlayer == Player.One ? playerOne : playerTwo;
                string player = currentState.CurrentPlayer == Player.One ? "pierwszy" : "drugi";

                int nextAction = (int)currentPlayer.ReturnNextMove(gameTree.SelectedNode)!;

                Console.WriteLine($"Gracz {player} koloruje {nextAction}");
                gameTree.MoveGameToNextState(nextAction);

                currentState = gameTree.SelectedNode.CorespondingState;
                gameResult = game.Result(currentState);
            }

            string result = "";
            if (gameResult == GameResult.PlayerOneWins)
                result = "Gracz pierwszy wygrywa!";
            else if (gameResult == GameResult.PlayerTwoWins)
                result = "Gracz drugi wygrywa!";
            else
                result = "Remis";
        }
    }
    private IAlgorithm InitiatePlayerAlgorithm(string alg, Game game)
    {
        switch (alg)
        {
            case "UCT":
                return new Uct(1.414, new IterationStopCondition(1000), game);
            case "MinMax":
                return new MiniMax(new IterationStopCondition(1000), game);
            case "Random":
                return new RandomPick(game);
            default:
                break;
        }
        return null;
    }

}