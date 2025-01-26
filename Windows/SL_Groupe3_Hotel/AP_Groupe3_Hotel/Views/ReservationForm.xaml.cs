using AP_Groupe3_Hotel.Models;
using AP_Groupe3_Hotel.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Logique d'interaction pour ReservationForm.xaml
    /// </summary>
    public partial class ReservationForm : Window
    {
        private TbReservation reservation;
        private List<TbClient> clients;
        private ClientRepository clientRepository;
        private List<TbChambre> chambres;
        private ChambreRepository chambreRepository;

        public TbReservation Reservation { get => reservation; set => reservation = value; }
        public List<TbClient> Clients { get => clients; set => clients = value; }
        public List<TbChambre> Chambres { get => chambres; set => chambres = value; }

        public ReservationForm(TbReservation _reservation)
        {
            InitializeComponent();

            clientRepository = new ClientRepository();
            Clients = new List<TbClient>(clientRepository.GetAllClients());

            chambreRepository = new ChambreRepository();
            Chambres = new List<TbChambre>(chambreRepository.GetAllChambres());

            Reservation = new TbReservation
            {
                PkRes = _reservation.PkRes,
                NbrPerRes = _reservation.NbrPerRes,
                DejRes = _reservation.DejRes,
                DatJouRes = _reservation.DatJouRes,
                DatArrRes = _reservation.DatArrRes,
                DatDepRes = _reservation.DatDepRes,
                FkResCli = _reservation.FkResCli,
                FkResCha = _reservation.FkResCha,
                FkResChaEta = _reservation.FkResChaEta,
                FkResCliNavigation = _reservation.FkResCliNavigation,
                TbChambre = _reservation.TbChambre
            };

            this.DataContext = this;
        }

        private void btnReservationFormRetour_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // Ferme la fenêtre et retourne false
            this.Close();
        }

        private void btnReservationFormEnregistrer_Click(object sender, RoutedEventArgs e)
        {
            if(Reservation.FkResCliNavigation == null)
            {
                MessageBox.Show("Veuillez sélectionner un client", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Reservation.TbChambre == null)
            {
                MessageBox.Show("Veuillez sélectionner une chambre", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Reservation.NbrPerRes == null)
            {
                MessageBox.Show("Veuillez sélectionner le nombre de personnes", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DateOnly date = DateOnly.FromDateTime(DateTime.Now);

            if (Reservation.DatArrRes < date || Reservation.DatDepRes < date)
            {
                MessageBox.Show("La date d'arrivée et de départ doit être supérieur à la date du jour", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Reservation.DatArrRes > Reservation.DatDepRes)
            {
                MessageBox.Show("La date d'arrivée doit être inférieur à la date de départ", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //foreach (TbReservation reservation in ReservationsView)
            //{
            //    if (Reservation.TbChambre.PkCha == reservation.TbChambre.PkCha && Reservation.TbChambre.PfkChaEtaNavigation.PkEta == reservation.TbChambre.PfkChaEtaNavigation.PkEta)
            //    {
            //        if (Reservation.DatArrRes >= reservation.DatArrRes && Reservation.DatArrRes <= reservation.DatDepRes)
            //        {
            //            MessageBox.Show("La chambre est déjà réservée pour cette période", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            //            return;
            //        }
            //        if (Reservation.DatDepRes >= reservation.DatArrRes && Reservation.DatDepRes <= reservation.DatDepRes)
            //        {
            //            MessageBox.Show("La chambre est déjà réservée pour cette période", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            //            return;
            //        }
            //    }
            //}

            Reservation.FkResCli = Reservation.FkResCliNavigation.PkCli;
            Reservation.FkResCha = Reservation.TbChambre.PkCha;
            Reservation.FkResChaEta = Reservation.TbChambre.PfkChaEta;
            DialogResult = true; // Indique que la fenêtre s'est fermée avec succès et retourne true
        }
    }
}
