using System.Windows;
using System.Windows.Controls;

namespace SeaBattleV2
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : UserControl
    {
        ContentPresenter OutputView;
        public Main(ContentPresenter _OutputView)
        {
            InitializeComponent();
            OutputView = _OutputView;
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            CreateField createField = new CreateField(OutputView);
            OutputView.Content = createField;
        }
    }
}
