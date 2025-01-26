using AP_Groupe3_Hotel.Models;
using AP_Groupe3_Hotel.Repositories;
using AP_Groupe3_Hotel.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AP_Groupe3_Hotel.Views
{
    /// <summary>
    /// Logique d'interaction pour ClientForm.xaml
    /// Codé par Maëlle
    /// Modifié par Maëlle et Alexandre
    /// </summary>
    public partial class ClientForm : Window
    {
        private TbClient client;
        private List<TbLocalite> localites;

        private LocaliteRepository? localiteRepository;

        public TbClient Client { get => client; set => client = value; }
        public List<TbLocalite> Localites { get => localites; set => localites = value; }
        public ClientForm(TbClient _client)
        {
            InitializeComponent();

            // Récupérer les localités
            localiteRepository = new LocaliteRepository();
            Localites = new List<TbLocalite>(localiteRepository.GetAllLocalites());

            // Si le client est null, on crée un nouveau client
            Client = new TbClient
            {
                PkCli = _client.PkCli,
                NomCli = _client.NomCli,
                PreCli = _client.PreCli,
                RueCli = _client.RueCli,
                TelCli = _client.TelCli,
                MailCli = _client.MailCli,
                DatNaisCli = _client.DatNaisCli,
                FkCliLoc = _client.FkCliLoc,
                FkCliLocNavigation = _client.FkCliLocNavigation,
                TbReservations = _client.TbReservations,
            };

            this.DataContext = this;
        }

        private void btnClientRetour_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (Client.NomCli == null)
            {

                MessageBox.Show("Veuillez saisir un nom.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Client.PreCli == null)
            {
                MessageBox.Show("Veuillez saisir un prénom.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if(Client.RueCli == null)
            {
               MessageBox.Show("Veuillez saisir une rue.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Client.FkCliLocNavigation == null)
            {
                MessageBox.Show("Veuillez sélectionner une localité.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(Client.DatNaisCli == null)
            {
                MessageBox.Show("Veuillez saisir une date de naissance.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            // Vérifier l'âge du client
            if (Client.DatNaisCli.HasValue)
            {
                var age = CalculeAgeClient(Client.DatNaisCli.Value);
                if (age < 18 || age > 105)
                {
                    MessageBox.Show("L'âge du client doit être supérieur à 18 ans et inférieur à 105 ans.", "Erreur d'âge", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            if (Client.TelCli == null)
            {
                MessageBox.Show("Veuillez saisir un numéro de téléphone.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Client.MailCli== null)
            {
                MessageBox.Show("Veuillez saisir une adresse mail.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Client.FkCliLoc = Client.FkCliLocNavigation.PkLoc;
            DialogResult = true;

        }
        /// <summary>
        /// permet de calculer l'age du client
        /// </summary>
        /// J'ai trouvé cette fonction sur developpez.net
        /// <param name="birthDate"></param>
        /// <returns name="age"></returns>
        private int CalculeAgeClient(DateOnly birthDate)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var age = today.Year - birthDate.Year;

            // Vérifier si l'anniversaire de cette année n'est pas encore passé
            if (birthDate > today.AddYears(-age))
                age--;

            return age;
        }

    }
}
