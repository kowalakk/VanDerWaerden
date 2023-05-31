using Ai;
using System.Collections.Generic;
using System.Diagnostics;
using VanDerWaerden;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Ile gier Van der Waerdena ma rozegrać program");
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
            Dictionary<string, List<TimeSpan>> times = new Dictionary<string, List<TimeSpan>>();
            times.Add("pierwszy", new List<TimeSpan>());
            times.Add("drugi", new List<TimeSpan>());

            Game game = new(numbersCount, winningSequenceCount);

            IAlgorithm playerOne = InitiatePlayerAlgorithm(firstAlg, game);
            IAlgorithm playerTwo = InitiatePlayerAlgorithm(secondAlg, game);

            State currentState = game.InitialState();
            GameTree gameTree = new(currentState, game);

            GameResult gameResult = game.Result(currentState);
            while (gameResult == GameResult.InProgress)
            {
                IAlgorithm currentPlayer = currentState.CurrentPlayer == Player.One ? playerOne : playerTwo;
                string player = currentState.CurrentPlayer == Player.One ? "pierwszy" : "drugi";

                Stopwatch stopwatch = Stopwatch.StartNew();
                int nextAction = (int)currentPlayer.ReturnNextMove(gameTree.SelectedNode)!;
                stopwatch.Stop();
                times[player].Add(stopwatch.Elapsed);

                Console.WriteLine($"Gracz {player} koloruje {nextAction}, czas namysłu {stopwatch.Elapsed}");
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
            Console.WriteLine(result);
            var avg = Average(times["pierwszy"]);
            Console.WriteLine($"Średni czas ruchu pierwszego gracza to: {avg}");
            avg = Average(times["drugi"]);
            Console.WriteLine($"Średni czas ruchu drugiego gracza to: {avg}");
            Console.WriteLine("######################");
        }


    }
    public static TimeSpan Average(IEnumerable<TimeSpan> timeSpans)
    {
        IEnumerable<long> ticksPerTimeSpan = timeSpans.Select(t => t.Ticks);
        double averageTicks = ticksPerTimeSpan.Average();
        long averageTicksLong = Convert.ToInt64(averageTicks);

        TimeSpan averageTimeSpan = TimeSpan.FromTicks(averageTicksLong);

        return averageTimeSpan;
    }

    private static IAlgorithm InitiatePlayerAlgorithm(string alg, Game game)
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