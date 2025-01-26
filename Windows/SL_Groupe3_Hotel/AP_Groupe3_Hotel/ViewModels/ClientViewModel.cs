using AP_Groupe3_Hotel.Models;
using AP_Groupe3_Hotel.Repositories;
using AP_Groupe3_Hotel.Utilities;
using AP_Groupe3_Hotel.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows;
using MaterialDesignColors;

namespace AP_Groupe3_Hotel.ViewModels
{
    /// <summary>
    /// Codé par Maëlle et Alexandre
    /// Déclare une classe appelée ClientViewModel qui hérite de la classe BaseViewModel.
    /// Cette classe est utilisée pour gérer la logique et les données liées à ClientView dans une application
    /// </summary>
    class ClientViewModel : BaseViewModel
    {
        private ObservableCollection<TbClient> clients;
        private TbClient clientSelectionne;
        private ICollectionView clientsView;
        private ClientRepository clientRepository;

        private ObservableCollection<TbLocalite> localites;
        private TbLocalite localiteSelectionnee;
        private LocaliteRepository localiteRepository;

        private string filterText;

        public ObservableCollection<TbClient> Clients
        {
            get => clients;
            set
            {
                clients = value;
                NotifyPropertyChanged(nameof(Clients));
                InitializeCollectionClientView(); // Appelle cette méthode à chaque fois que la collection est modifiée 
            }
        }
        public ICollectionView ClientsView
        {
            get => clientsView;
            set
            {
                clientsView = value;
                NotifyPropertyChanged(nameof(ClientsView));
            }
        }
        public TbClient ClientSelectionne
        {
            get => clientSelectionne;
            set
            {
                clientSelectionne = value;
                NotifyPropertyChanged(nameof(ClientSelectionne));
            }
        }
        public string FilterText
        {
            get => filterText;
            set
            {
                filterText = value;
                NotifyPropertyChanged(nameof(FilterText));
                ApplyFilter();
            }
        }
        public ObservableCollection<TbLocalite> Localites
        {
            get => localites;
            set
            {
                localites = value;
                NotifyPropertyChanged(nameof(Localites));
            }
        }

        public TbLocalite LocaliteSelectionnee
        {
            get => localiteSelectionnee;
            set
            {
                localiteSelectionnee = value;
                NotifyPropertyChanged(nameof(LocaliteSelectionnee));
            }
        }

        /// <summary>
        /// Commande
        /// </summary>
        public ICommand DisplayClientFormCommand { get; }
        public ICommand EditClientCommand { get; }
        public ICommand DeleteClientCommand { get; }
        public ICommand InsertLocaliteCommand { get; }
        public ICommand SortingListCommand { get; }

        public ClientViewModel()
        {
            clientRepository = new ClientRepository();
            Clients = new ObservableCollection<TbClient>(clientRepository.GetAllClients());

            localiteRepository = new LocaliteRepository();
            Localites = new ObservableCollection<TbLocalite>(localiteRepository.GetAllLocalites());

            DisplayClientFormCommand = new RelayCommand(o => DisplayClientForm());
            EditClientCommand = new RelayCommand(o => EditClient());
            DeleteClientCommand = new RelayCommand(o => DeleteClient());
            InsertLocaliteCommand = new RelayCommand(o => DisplayLocalite());
            SortingListCommand = new RelayCommand(o => SortingList(o));
        }

        /// <summary>
        /// Initinalise la vue de la collection des clients pour 'Clients'
        /// </summary>
        private void InitializeCollectionClientView()
        {
            ClientsView = CollectionViewSource.GetDefaultView(Clients);
        }
        private void ApplyFilter()
        {
            if (string.IsNullOrEmpty(FilterText))
            {
                // Si le champ de texte est vide, afficher toutes les données
                ClientsView.Filter = null;
            }
            else
            {
                // Sinon, appliquer le filtre sur les données
                ClientsView.Filter = item =>
                {
                    var client = item as TbClient;
                    return client != null &&
                           (client.NomCli.ToLower().Contains(FilterText.ToLower()) ||
                            client.PreCli.ToLower().Contains(FilterText.ToLower()));
                };
            }
        }

        private void SortingList(object parameter)
        {
            string sortColumn = (string)parameter;

            // Vérifier si la colonne est déjà triée
            if (ClientsView.SortDescriptions.Any(sd => sd.PropertyName == sortColumn))
            {
                // Si oui, obtenir la direction de tri actuelle
                ListSortDirection currentDirection = ClientsView.SortDescriptions.First(sd => sd.PropertyName == sortColumn).Direction;

                // Basculement entre le tri ascendant et descendant
                ListSortDirection newDirection = (currentDirection == ListSortDirection.Ascending) ? ListSortDirection.Descending : ListSortDirection.Ascending;

                // Effacer tous les tris existants pour cette colonne
                ClientsView.SortDescriptions.Clear();

                // Ajouter le nouveau tri
                ClientsView.SortDescriptions.Add(new SortDescription(sortColumn, newDirection));
            }
            else
            {
                // Si la colonne n'est pas déjà triée, ajouter un tri ascendant par défaut
                ClientsView.SortDescriptions.Clear();
                ClientsView.SortDescriptions.Add(new SortDescription(sortColumn, ListSortDirection.Ascending));
            }
        }
        /// <summary>
        /// cette fonction est appelée lorsqu'un utilisateur souhaite créer un nouveau client
        /// </summary>
        public void DisplayClientForm()
        {
                // Créer une nouvelle instance de TbClient pour le formulaire de création
                TbClient newClient = new TbClient();

                // Passer ce nouvel objet TbClient au constructeur de ClientForm
                ClientForm insertWindowClientForm = new ClientForm(newClient);

                // Afficher la nouvelle fenêtre en tant que boîte de dialogue modale
                bool? result = insertWindowClientForm.ShowDialog();

                // Si l'insertion est confirmée (l'utilisateur appuie sur OK)
                if (result == true)
                {
                    // Affecte les données du formulaire à la propriété Client de la fenêtre
                    newClient = insertWindowClientForm.Client;

                    // Ajouter le nouveau client à la liste et à la base de données
                    clientRepository.InsertClientRep(newClient);
                    Clients.Add(newClient);
                }

        }
        /// <summary>
        /// cette fonction est appelée lorsqu'un utilisateur souhaite modifier un client existant
        /// </summary>
        private void EditClient()
        {
            // Vérifier si un client est sélectionné
            if (ClientSelectionne == null)
            {
                MessageBox.Show("Veuillez sélectionner un client.");
                return;
            }

            // Créer une nouvelle fenêtre avec le client sélectionné en paramètre
            ClientForm editClientForm = new ClientForm(ClientSelectionne);

            // Afficher la nouvelle fenêtre en tant que boîte de dialogue modale
            bool? result = editClientForm.ShowDialog();

            if (result == true)
            {
                var editedClient = editClientForm.Client;
                int index = Clients.IndexOf(ClientSelectionne);

                Clients[index] = editedClient;

                clientRepository.EditClientRep(editedClient);
            }
        }

        private void DeleteClient()
        {
            if (ClientSelectionne == null)
            {
                MessageBox.Show("Veuillez sélectionner un client.");
                return;
            }

            // Vérifier si le client a des réservations
            if (clientRepository.ControlClientReservation(ClientSelectionne.PkCli))
            {
                MessageBox.Show($"Le client {ClientSelectionne.PreCli} {ClientSelectionne.NomCli} a des réservations en cours et ne peut pas être supprimé.");
                return;
            }

            // Demander une confirmation avant de supprimer le client
            if (MessageBox.Show($"Êtes-vous sûr de vouloir supprimer {ClientSelectionne.PreCli} {ClientSelectionne.NomCli} ?", "Confirmation de suppression", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                // Supprimer le client de la base de données
                clientRepository.DeleteClientRep(ClientSelectionne);

                // Supprimer le client de la liste
                Clients.Remove(ClientSelectionne);
            }
        }
        private void DisplayLocalite()
        {
            // Créer une nouvelle instance de TbLocalite pour le formulaire de création
            TbLocalite newLocalite = new TbLocalite();
            // Passer ce nouvel objet TbLocalite au constructeur de LocaliteForm
            LocaliteForm insertWindowLocaliteForm = new LocaliteForm(newLocalite);

            bool? result = insertWindowLocaliteForm.ShowDialog();

            // Si l'insertion est confirmée (l'utilisateur appuie sur OK)
            if (result == true)
            {
                // Ajouter la nouvelle localité à la liste et à la base de données
                newLocalite = insertWindowLocaliteForm.Localite;

                // Ajouter la nouvelle localité à la base de données
                LocaliteRepository localiteRepository = new LocaliteRepository();
                localiteRepository.InsertLocaliteRep(newLocalite);

                Localites.Add(newLocalite);
            }
        }
    }
}