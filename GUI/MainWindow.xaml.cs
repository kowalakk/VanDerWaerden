using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using Ai;
using VanDerWaerden;
using System.Collections;
using System.ComponentModel;
using System.Windows.Threading;
using System.Globalization;
using static System.Collections.Specialized.BitVector32;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<int> PlayerOneNumbers = new List<int>();
        List<int> PlayerTwoNumbers = new List<int>();
        Brush PlayerOneColor = Brushes.Blue;
        Brush PlayerTwoColor = Brushes.Red;
        VanDerWaerdenGameSettings configuration;
        BackgroundWorker backgroundWorker;
        DispatcherTimer gameTimer;
        DispatcherTimer moveTimer;
        Game game;
        State currentState;
        GameTree gameTree;
        IAlgorithm playerOne;
        IAlgorithm playerTwo;
        public MainWindow()
        {
            InitializeComponent();
            Window settings = new GameSettings();
            settings.ShowDialog();
            Configuration GameConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration = (VanDerWaerdenGameSettings)GameConfiguration.GetSection("VanDerWaerdenGameSettings");

            gameTimer = new DispatcherTimer();
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Interval = new TimeSpan(0, 0, 1);

            moveTimer = new DispatcherTimer();
            moveTimer.Tick += MoveTimer_Tick;
            moveTimer.Interval = new TimeSpan(0, 0, 1);

            backgroundWorker = new BackgroundWorker()
            {
                WorkerReportsProgress = true,
            };
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;

            RedrawBoard();
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            RefreshUI();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            GameResult gameResult = game.Result(currentState);
            while (gameResult == GameResult.InProgress)
            {
                moveTimer.Start();

                IAlgorithm currentPlayer = currentState.CurrentPlayer == Player.One ? playerOne : playerTwo;

                int nextAction = currentPlayer.ReturnNextMove(gameTree.SelectedNode)!;
                if (currentState.CurrentPlayer == Player.One)
                {
                    PlayerOneNumbers.Add(nextAction);
                }
                else
                {
                    PlayerTwoNumbers.Add(nextAction);
                }
                try
                {
                    gameTree.SelectChildNode(nextAction);
                }
                catch
                {
                    State childState = game.PerformAction(nextAction, gameTree.SelectedNode.CorespondingState);
                    Node childNode = new(nextAction, childState, gameTree.SelectedNode);
                    gameTree.SelectedNode.ExpandedChildren.Add(childNode);
                    gameTree.SelectChildNode(nextAction);
                }

                currentState = gameTree.SelectedNode.CorespondingState;
                gameResult = game.Result(currentState);

                moveTimer.Stop();
                backgroundWorker.ReportProgress(1);
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            gameTimer.Stop();
            Dispatcher.BeginInvoke(new Action(() => { StartButton.IsEnabled = true; }));
            GameResult gameResult = game.Result(currentState);

            string caption = "Koniec Gry!";
            string messageBoxText;
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.None;

            if (gameResult == GameResult.PlayerOneWins)
                messageBoxText = "Gracz pierwszy wygrywa!";
            else if (gameResult == GameResult.PlayerTwoWins)
                messageBoxText = "Gracz drugi wygrywa!";
            else
                messageBoxText = "Remis";

            MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.OK);
        }
        private void RedrawBoard()
        {
            FlowDocument flowDocument = new FlowDocument();
            Paragraph paragraph = new Paragraph();
            foreach (int i in Enumerable.Range(0, configuration.N))
            {
                if (PlayerOneNumbers != null && PlayerOneNumbers.Contains(i))
                {
                    paragraph.Inlines.Add(ColorNumber(i, PlayerOneColor));
                }
                else if (PlayerTwoNumbers != null && PlayerTwoNumbers.Contains(i))
                {
                    paragraph.Inlines.Add(ColorNumber(i, PlayerTwoColor));
                }
                else
                {
                    paragraph.Inlines.Add(i.ToString());
                }
                paragraph.Inlines.Add(", ");
            }
            flowDocument.Blocks.Add(paragraph);

            GameStateTextBox.Document = flowDocument;
        }
        private void RefreshUI()
        {
            RedrawBoard();

            PlayerOneScoreTB.Text = currentState.LongestSequences[Player.One].Length.ToString();
            PlayerTwoScoreTB.Text = currentState.LongestSequences[Player.Two].Length.ToString();

            PlayerOneSequenceTB.Text = GetSequenceString(Player.One);
            PlayerTwoSequenceTB.Text = GetSequenceString(Player.Two);

            MoveTimerTB.Text = "00:00";
        }

        private string GetSequenceString(Player player)
        {
            return string.Join(", ", Enumerable.Range(0, currentState.LongestSequences[player].Length).Select(y => y * currentState.LongestSequences[player].Step).Select(x => x + currentState.LongestSequences[player].FirstElement).ToArray());
        }

        private Run ColorNumber(int number, Brush brush)
        {
            Run run = new Run(number.ToString());
            run.Foreground = brush;
            return run;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PlayerOneNumbers = new List<int>();
            PlayerTwoNumbers = new List<int>();
            StartButton.IsEnabled = false;

            GameTimerTB.Text = "00:00";
            gameTimer.Start();

            game = new(configuration.N, configuration.K);

            playerOne = InitiatePlayerAlgorithm(configuration.Player1);
            playerTwo = InitiatePlayerAlgorithm(configuration.Player2);

            currentState = game.InitialState();
            gameTree = new(currentState, game);
            backgroundWorker.RunWorkerAsync();
        }
        private IAlgorithm InitiatePlayerAlgorithm(string alg)
        {
            switch (alg)
            {
                case "UCT":
                    return new Uct(1.414, new IterationStopCondition(1000), game);
                case "MinMax":
                    return new MiniMax(game, 3);
                case "Random":
                    Random r = new Random();
                    return new RandomPick(game, r.Next());
                default:
                    break;
            }
            return null;
        }

        private void GameTimer_Tick(object? sender, EventArgs e)
        {
            DateTime time = DateTime.ParseExact(GameTimerTB.Text, "mm:ss", CultureInfo.InvariantCulture);
            time = time.AddSeconds(1);
            GameTimerTB.Text = time.ToString("mm:ss");
        }

        private void MoveTimer_Tick(object? sender, EventArgs e)
        {
            DateTime time = DateTime.ParseExact(MoveTimerTB.Text, "mm:ss", CultureInfo.InvariantCulture);
            time = time.AddSeconds(1);
            MoveTimerTB.Text = time.ToString("mm:ss");
        }
    }
}
