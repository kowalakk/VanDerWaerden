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
            IAlgorithm playerOne = new Uct(1.414, new IterationStopCondition(1000), game);
            IAlgorithm playerTwo = new MiniMax(new IterationStopCondition(1000), game);
 
            State currentState = game.InitialState();
            GameTree gameTree = new(currentState);

            GameResult gameResult = game.Result(currentState);
            while (gameResult == GameResult.InProgress)
            {
                IAlgorithm currentPlayer = currentState.CurrentPlayer == Player.One ? playerOne : playerTwo;
                string player = currentState.CurrentPlayer == Player.One ? "pierwszy" : "drugi";

                int nextAction = (int)currentPlayer.ReturnNextMove(gameTree.SelectedNode)!;

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
        }
    }
}