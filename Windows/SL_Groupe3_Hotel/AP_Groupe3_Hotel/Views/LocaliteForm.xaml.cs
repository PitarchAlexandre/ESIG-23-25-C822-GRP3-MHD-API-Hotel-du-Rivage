using AP_Groupe3_Hotel.Models;
using AP_Groupe3_Hotel.Repositories;
using AP_Groupe3_Hotel.ViewModels;
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
using System.Windows.Shapes;

namespace AP_Groupe3_Hotel.Views
{
    /// <summary>
    /// Logique d'interaction pour LocaliteForm.xaml
    /// </summary>
    public partial class LocaliteForm : Window
    {
        private TbLocalite localite;
        
        public TbLocalite Localite { get => localite; set => localite = value; }

        public LocaliteForm(TbLocalite _localite)
        {
            InitializeComponent();

            Localite = new TbLocalite
            {
                PkLoc = _localite.PkLoc,
                NpaLoc = _localite.NpaLoc,
                NomLoc = _localite.NomLoc,
            };

            this.DataContext = this;
        }

        private void btnLocaliteRetour_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void btnLocaliteEnregistrer_click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }
    }
}
