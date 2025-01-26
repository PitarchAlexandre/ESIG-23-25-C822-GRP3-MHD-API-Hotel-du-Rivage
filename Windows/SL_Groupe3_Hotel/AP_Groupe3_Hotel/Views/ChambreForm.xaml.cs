using AP_Groupe3_Hotel.Models;
using AP_Groupe3_Hotel.Repositories;
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
    /// Logique d'interaction pour ChambreForm.xaml
    /// </summary>
    public partial class ChambreForm : Window
    {
        private TbChambre chambre;
        private List<TbEtage> etages;

        private EtageRepository etageRepository;

        public TbChambre Chambre { get => chambre; set => chambre = value; }
        public List<TbEtage> Etages { get => etages; set => etages = value; }
        

        //Prends les champs de la table pour créer une nouvelle chambre
        //Le repository de l'étage reprend toutes les données pour ensuite faire une liste dans la combo box
        public ChambreForm(TbChambre _chambre)
        {
            InitializeComponent();
            etageRepository = new EtageRepository();
            Etages = new List<TbEtage>(etageRepository.GetAllEtages());

            Chambre = new TbChambre
            {
                PkCha = _chambre.PkCha,
                CodeCha = _chambre.CodeCha,
                CapCha = _chambre.CapCha,
                PrixCha = _chambre.PrixCha,
                PfkChaEta = _chambre.PfkChaEta,
                PfkChaEtaNavigation = _chambre.PfkChaEtaNavigation,
                TbReservations = _chambre.TbReservations
            };


            this.DataContext = this;
        }

        private void btnChambreFormRetour_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btnEnregistrer_Click(object sender, RoutedEventArgs e)
        {
            Chambre.PfkChaEta = Chambre.PfkChaEtaNavigation.PkEta; 
            DialogResult = true;
        }
    }
}
