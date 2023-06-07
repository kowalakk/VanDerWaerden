using Ai;
using System.Diagnostics;
using VanDerWaerden;

internal class Program
{
    private static (GameResult, TimeSpan, TimeSpan, int, int) PlayGame(int numbersCount, int winningSequenceCount, IAlgorithm playerOne, IAlgorithm playerTwo)
    {
        Dictionary<string, List<TimeSpan>> times = new Dictionary<string, List<TimeSpan>>();
        times.Add("pierwszy", new List<TimeSpan>());
        times.Add("drugi", new List<TimeSpan>());

        Game game = new(numbersCount, winningSequenceCount);
        State currentState = game.InitialState();
        GameTree gameTree = new(currentState, game);

        GameResult gameResult = game.Result(currentState);
        while (gameResult == GameResult.InProgress)
        {
            (IAlgorithm currentAlgorithm, string player) =
                currentState.CurrentPlayer == Player.One ? (playerOne, "pierwszy") : (playerTwo, "drugi");

            Stopwatch stopwatch = Stopwatch.StartNew();
            int nextAction = currentAlgorithm.ReturnNextMove(gameTree.SelectedNode)!;
            stopwatch.Stop();
            times[player].Add(stopwatch.Elapsed);

            //gameTree.SelectChildNode(nextAction);
            gameTree.MoveGameToNextState(nextAction);

            currentState = gameTree.SelectedNode.CorespondingState;
            gameResult = game.Result(currentState);
        }

        return (gameResult, Average(times["pierwszy"]), Average(times["drugi"]), times["pierwszy"].Count, times["drugi"].Count);
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
                return new Uct(1.414, new IterationStopCondition(100000), game);
            case "MinMax":
                return new MiniMax(game, 3);
            case "Random":
                return new RandomPick(game);
            default:
                break;
        }
        return null;
    }
    private static void Main(string[] args)
    {
        string filename = "minmaxvsuct2.txt";
        for (int i = 0; i < 100; i++)
        {
            Console.WriteLine((i + 1).ToString());
            int n = 20;
            int k = 5;
            Game game = new Game(n, k);
            IAlgorithm playerOne = InitiatePlayerAlgorithm("MinMax", game);
            IAlgorithm playerTwo = InitiatePlayerAlgorithm("UCT", game);
            (GameResult gameResult, TimeSpan avg1, TimeSpan avg2, int n1, int n2) = PlayGame(n, k, playerOne, playerTwo);

            List<string> tokens = new List<string>();
            tokens.Add(n.ToString());
            tokens.Add(k.ToString());
            tokens.Add(gameResult.ToString());
            tokens.Add(avg1.ToString());
            tokens.Add(avg2.ToString());
            tokens.Add(n1.ToString());
            tokens.Add(n2.ToString());

            using (StreamWriter writer = new StreamWriter(filename, true))
            {
                writer.WriteLine(string.Join(";", tokens));
            }
        }

        /*while (true)
        {

            Console.WriteLine("Program rozegra grę Van der Waerdena");
            Console.WriteLine("Podaj długość zbioru dostępnych liczb");
            int numbersCount = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Podaj długość wygrywającego ciągu arytmetycznego");
            int winningSequenceCount = Convert.ToInt32(Console.ReadLine());
            Game game = new(numbersCount, winningSequenceCount);
            IAlgorithm playerOne = new Uct(1.414, new IterationStopCondition(1000), game);
            IAlgorithm playerTwo = new MiniMax(game, 50/numbersCount);

            State currentState = game.InitialState();
            GameTree gameTree = new(currentState);

            GameResult gameResult = game.Result(currentState);
            while (gameResult == GameResult.InProgress)
            {
                (IAlgorithm currentAlgorithm, string player) = 
                    currentState.CurrentPlayer == Player.One ? (playerOne, "pierwszy") : (playerTwo, "drugi");

                int nextAction = currentAlgorithm.ReturnNextMove(gameTree.SelectedNode)!;

                Console.WriteLine($"Gracz {player} koloruje {nextAction}");
                gameTree.SelectChildNode(nextAction);

                currentState = gameTree.SelectedNode.CorespondingState;
                gameResult = game.Result(currentState);
            }
            if (gameResult == GameResult.PlayerOneWins)
                Console.WriteLine($"Gracz pierwszy wygrywa!");
            else if (gameResult == GameResult.PlayerTwoWins)
                Console.WriteLine($"Gracz drugi wygrywa!");
            else
                Console.WriteLine($"Remis");
        }*/
    }
}