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

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<int> PlayeOneNumbers = new List<int>();
        List<int> PlayeTwoNumbers = new List<int>();
        Brush PlayerOneColor = Brushes.Blue;
        Brush PlayerTwoColor = Brushes.Red;
        VanDerWaerdenGameSettings? configuration;
        BackgroundWorker backgroundWorker;
        Game? game;
        State currentState;
        GameTree? gameTree;
        Uct? uct;
        public MainWindow()
        {
            InitializeComponent();
            Window settings = new GameSettings();
            settings.ShowDialog();
            Configuration GameConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration = (VanDerWaerdenGameSettings)GameConfiguration.GetSection("VanDerWaerdenGameSettings");

            backgroundWorker = new BackgroundWorker()
            {
                WorkerReportsProgress = true,
            };
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Refresh();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            GameResult gameResult = game.Result(currentState);
            while (gameResult == GameResult.InProgress)
            {
                int nextAction = (int)uct.ChooseAction(gameTree)!;
                string gracz = currentState.CurrentPlayer == Player.One ? "pierwszy" : "drugi";
                if (currentState.CurrentPlayer == Player.One) { PlayeOneNumbers.Add(nextAction); } else { PlayeTwoNumbers.Add(nextAction); }
                uct.MoveGameToNextState(gameTree, nextAction);
                currentState = gameTree.SelectedNode.CorespondingState;
                gameResult = game.Result(currentState);
                backgroundWorker.ReportProgress(1);
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() => { StartButton.IsEnabled = true; }));
        }

        public void Refresh()
        {
            FlowDocument flowDocument = new FlowDocument();
            Paragraph paragraph = new Paragraph();
            foreach (int i in Enumerable.Range(0, configuration.N)){
                if (PlayeOneNumbers.Contains(i)) { 
                    paragraph.Inlines.Add(ColorNumber(i, PlayerOneColor));
                }
                else if (PlayeTwoNumbers.Contains(i))
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

            PlayerOneScoreTB.Text = currentState.LongestSequences[Player.One].Length.ToString();
            PlayerTwoScoreTB.Text = currentState.LongestSequences[Player.Two].Length.ToString();
        }

        public Run ColorNumber(int number, Brush brush)
        {
            Run run = new Run(number.ToString());
            run.Foreground = brush;
            return run;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PlayeOneNumbers = new List<int>();
            PlayeTwoNumbers = new List<int>();
            StartButton.IsEnabled = false;
            game = new(configuration.N, configuration.K);
            uct = new(1.414, new IterationStopCondition(1000), game);
            currentState = game.InitialState();
            gameTree = new(currentState);
            backgroundWorker.RunWorkerAsync();
        }
    }
}
