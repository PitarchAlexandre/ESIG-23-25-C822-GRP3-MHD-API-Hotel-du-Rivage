using AP_Groupe3_Hotel.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AP_Groupe3_Hotel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(); // Initialisation du ViewModel
        }

        private void btnQuitter_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Voulez-vous quitter l'application ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                // Ne rien faire ici, car il n'y a pas de paramètre e pour annuler la fermeture
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

        private void btnQuestionMark_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Si vous avez des questions, appelez notre hotline au :\n 032 420 77 90.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}