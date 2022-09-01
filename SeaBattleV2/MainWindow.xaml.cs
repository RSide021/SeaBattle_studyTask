using System.Windows;

namespace SeaBattleV2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Main main = new Main(OutputView);
            this.OutputView.Content = main;
        }
    }
}
