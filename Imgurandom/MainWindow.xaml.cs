using System.Windows;

namespace Imgurandom
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModel VM;
        public MainWindow()
        {
            InitializeComponent();
            VM = new ViewModel(this);
            DataContext = VM;
        }
    }
}
