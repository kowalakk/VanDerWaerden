using Ai;
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
            Uct uct = new(1.414, new IterationStopCondition(1000), game);
            MiniMax miniMax = new(new IterationStopCondition(1000), game);
            State currentState = game.InitialState();
            GameTree gameTree = new(currentState);

            GameResult gameResult = game.Result(currentState);
            while (gameResult == GameResult.InProgress)
            {
                int nextAction = (int)uct.ChooseAction(gameTree)!;
                Console.WriteLine($"Gracz pierwszy koloruje {nextAction}");
                uct.MoveGameToNextState(gameTree, nextAction);
                currentState = gameTree.SelectedNode.CorespondingState;
                gameResult = game.Result(currentState);
                if(gameResult != GameResult.InProgress)
                    break;

                nextAction = miniMax.Search(currentState);
                Console.WriteLine($"Gracz drugi koloruje {nextAction}");
                uct.MoveGameToNextState(gameTree, nextAction);
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