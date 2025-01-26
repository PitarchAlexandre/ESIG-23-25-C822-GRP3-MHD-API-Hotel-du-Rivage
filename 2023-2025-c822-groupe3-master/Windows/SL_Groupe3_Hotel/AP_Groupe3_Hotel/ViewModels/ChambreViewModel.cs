using AP_Groupe3_Hotel.Models;
using AP_Groupe3_Hotel.Repositories;
using AP_Groupe3_Hotel.Utilities;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows;
using AP_Groupe3_Hotel.Views;

namespace AP_Groupe3_Hotel.ViewModels
{
    /// <summary>
    /// ViewModel pour la gestion des chambres.
    /// </summary>
    class ChambreViewModel : BaseViewModel
    {
        private ObservableCollection<TbChambre> chambres;
        private TbChambre chambreSelectionnee;
        private ICollectionView chambresView;
        private ChambreRepository chambreRepository;
        private string filterText;

        public ObservableCollection<TbChambre> Chambres
        {
            get => chambres;
            set
            {
                chambres = value;
                NotifyPropertyChanged(nameof(Chambres));
                InitializeCollectionChambreView();
            }
        }

        public ICollectionView ChambresView
        {
            get => chambresView;
            set
            {
                chambresView = value;
                NotifyPropertyChanged(nameof(ChambresView));
            }
        }

        public TbChambre ChambreSelectionnee
        {
            get => chambreSelectionnee;
            set
            {
                chambreSelectionnee = value;
                NotifyPropertyChanged(nameof(ChambreSelectionnee));
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

        public ICommand InsertChambreFormCommand { get; }
        public ICommand EditChambreCommand { get; }
        public ICommand DeleteChambreCommand { get; }
        public ICommand SortingListCommand { get; }

        public ChambreViewModel()
        {
            chambreRepository = new ChambreRepository();
            Chambres = new ObservableCollection<TbChambre>(chambreRepository.GetAllChambres());

            InsertChambreFormCommand = new RelayCommand(o => InsertChambre());
            EditChambreCommand = new RelayCommand(o => EditChambre());
            DeleteChambreCommand = new RelayCommand(o => DeleteChambre());
            SortingListCommand = new RelayCommand(o => SortingList(o));
        }

        private void InitializeCollectionChambreView()
        {
            ChambresView = CollectionViewSource.GetDefaultView(Chambres);
        }

        private void ApplyFilter()
        {
            if (string.IsNullOrEmpty(FilterText))
            {
                ChambresView.Filter = null;
            }
            else
            {
                ChambresView.Filter = item =>
                {
                    var chambre = item as TbChambre;
                    return chambre != null &&
                           (chambre.CodeCha.ToString().Contains(FilterText.ToLower()) ||
                            chambre.CapCha.ToLower().Contains(FilterText.ToLower()) ||
                            chambre.PrixCha.ToString().Contains(FilterText.ToLower()));
                };
            }
        }
        /// <summary>
        /// Trie les colone par ordre croissant puis décroissant au deuxième clique
        /// </summary>
        /// <param name="parameter"></param>
        private void SortingList(object parameter)
        {
            string sortColumn = (string)parameter;

            if (sortColumn == "PfkChaEtaNavigation.CodeEta")
            {
                // Tri personnalisé pour la colonne CodeEta de PfkChaEtaNavigation
                if (ChambresView.SortDescriptions.Any(sd => sd.PropertyName == "PfkChaEtaNavigation.CodeEta"))
                {
                    ListSortDirection currentDirection = ChambresView.SortDescriptions.First(sd => sd.PropertyName == "PfkChaEtaNavigation.CodeEta").Direction;
                    ListSortDirection newDirection = (currentDirection == ListSortDirection.Ascending) ? ListSortDirection.Descending : ListSortDirection.Ascending;

                    ChambresView.SortDescriptions.Clear();
                    ChambresView.SortDescriptions.Add(new SortDescription("PfkChaEtaNavigation.CodeEta", newDirection));
                }
                // Tri par ordre croissant si la colonne n'est pas déjà triée
                else
                {
                    ChambresView.SortDescriptions.Clear();
                    ChambresView.SortDescriptions.Add(new SortDescription("PfkChaEtaNavigation.CodeEta", ListSortDirection.Ascending));
                }
            }
            // Tri  pour les autres colones que PfkChaEtaNavigation.CodeEta das l'ordre croissant puis décroissant
            else
            {
                if (ChambresView.SortDescriptions.Any(sd => sd.PropertyName == sortColumn))
                {
                    ListSortDirection currentDirection = ChambresView.SortDescriptions.First(sd => sd.PropertyName == sortColumn).Direction;
                    ListSortDirection newDirection = (currentDirection == ListSortDirection.Ascending) ? ListSortDirection.Descending : ListSortDirection.Ascending;

                    ChambresView.SortDescriptions.Clear();
                    ChambresView.SortDescriptions.Add(new SortDescription(sortColumn, newDirection));
                }
                else
                {
                    ChambresView.SortDescriptions.Clear();
                    ChambresView.SortDescriptions.Add(new SortDescription(sortColumn, ListSortDirection.Ascending));
                }
            }
        }

        public void InsertChambre()
        {
            TbChambre newChambre = new TbChambre();
            ChambreForm insertWindowChambreForm = new ChambreForm(newChambre);

            bool? result = insertWindowChambreForm.ShowDialog();

            if (result == true)
            {
                newChambre = insertWindowChambreForm.Chambre;

                // Initialisation de CodeCha à une valeur non nulle si elle est à zéro
                if (newChambre.CodeCha == 0)
                {
                    newChambre.CodeCha = 1;
                }

                newChambre.PfkChaEta = newChambre.PfkChaEtaNavigation.PkEta;
                chambreRepository.AddChambreRep(newChambre);
                Chambres.Add(newChambre);
            }
        }


        /// <summary>
        /// fonction appelée lorsqu'un utilisateur souhaite modifier une chambre existante
        /// </summary>
        private void EditChambre()
        {
            if (ChambreSelectionnee == null)
            {
                MessageBox.Show("Veuillez sélectionner une chambre.");
                return;
            }

            // Créé une nouvelle fenetre avec la chambre selectionné en paramètre
            ChambreForm editChambreForm = new ChambreForm(ChambreSelectionnee);
            // Afficher la nouvelle fenêtre en tant que boîte de dialogue modale
            bool? result = editChambreForm.ShowDialog();

            // Si l'édition est confirmée (l'utilisateur appuie sur OK)
            if (result == true)
            {
                // Récupère la chambre éditée
                var editedChambre = editChambreForm.Chambre;
                // Récupère l'index de la chambre sélectionnée
                int index = Chambres.IndexOf(ChambreSelectionnee);

                // Remplace la chambre sélectionnée par la chambre éditée
                Chambres[index] = editedChambre;
                // Mettre à jour la chambre dans la base de données
                chambreRepository.EditChambreRep(editedChambre);
            }
        }


        /// <summary>
        /// Fonction pour supprimer une chambre
        /// </summary>
        private void DeleteChambre()
        {
            if (ChambreSelectionnee == null)
            {
                MessageBox.Show("Veuillez sélectionner une chambre.");
                return;
            }

            // Récupère l'étage de la chambre sélectionnée
            var etageChambreSelectionnee = ChambreSelectionnee.PfkChaEtaNavigation.CodeEta;

            // Ajoutez ici la logique de vérification des réservations associées à une chambre si nécessaire
            if (MessageBox.Show($"Êtes-vous sûr de vouloir supprimer la chambre numéro {etageChambreSelectionnee}-{ChambreSelectionnee.CodeCha} ?", 
                "Confirmation de suppression", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {

                // Supprimer la chambre de la base de données grace au Repository
                chambreRepository.DeleteChambreRep(ChambreSelectionnee);
                // Supprimer la chambre de la listeview 
                Chambres.Remove(ChambreSelectionnee);

            }
        }

    }
}
