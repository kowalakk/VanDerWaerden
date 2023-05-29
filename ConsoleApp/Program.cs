using Ai;
using System.Diagnostics;
using VanDerWaerden;

internal class Program
{
    private static void Main(string[] args)
    {
        while (true)
        {

            Console.WriteLine("Program rozegra grę Van der Waerdena");
            Console.WriteLine("Podaj długość zbioru dostępnych liczb");
            int numbersCount = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Podaj długość wygrywającego ciągu arytmetycznego");
            int winningSequenceCount = Convert.ToInt32(Console.ReadLine());
            Game game = new(numbersCount, winningSequenceCount);
            Uct ai = new(1.414, new IterationStopCondition(1000), new(numbersCount, winningSequenceCount));
            State currentState = game.InitialState();
            GameTree gameTree = new(currentState);

            GameResult gameResult = game.Result(currentState);
            while (gameResult == GameResult.InProgress)
            {
                int nextAction = (int)ai.ChooseAction(gameTree)!;
                string gracz = currentState.CurrentPlayer == Player.One ? "pierwszy" : "drugi";
                Console.WriteLine($"Gracz {gracz} koloruje {nextAction}");
                ai.MoveGameToNextState(gameTree, nextAction);
                currentState = gameTree.SelectedNode.CorespondingState;
                gameResult = game.Result(currentState);

            }
            if (gameResult == GameResult.PlayerOneWins)
                Console.WriteLine($"Gracz pierwszy wygrywa!");
            else if (gameResult == GameResult.PlayerTwoWins)
                Console.WriteLine($"Gracz drugi wygrywa!");
            else
                Console.WriteLine($"Remis");
            Console.WriteLine(currentState.LongestSequences[Player.One]);
            Console.WriteLine(currentState.LongestSequences[Player.Two]);
        }
    }
}