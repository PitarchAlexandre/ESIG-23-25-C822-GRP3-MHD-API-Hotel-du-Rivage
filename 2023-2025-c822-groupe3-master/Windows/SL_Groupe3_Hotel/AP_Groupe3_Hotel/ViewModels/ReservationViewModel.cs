using AP_Groupe3_Hotel.Models;
using AP_Groupe3_Hotel.Repositories;
using AP_Groupe3_Hotel.Utilities;
using AP_Groupe3_Hotel.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace AP_Groupe3_Hotel.ViewModels
{
    class ReservationViewModel : BaseViewModel
    {
        private ObservableCollection<TbReservation> reservations;
        private ICollectionView reservationsView;
        private ObservableCollection<TbClient> clients;
        private ReservationRepository reservationRepository;
        private TbReservation reservationSelectionnee;
        private TbClient clientSelectionne;
        private string filterText;

        public ObservableCollection<TbReservation> Reservations
        {
            get => reservations;
            set
            {
                reservations = value;
                NotifyPropertyChanged(nameof(Reservations));
                InitializeCollectionView();
            }
        }


        public ICollectionView ReservationsView
        {
            get => reservationsView;
            set
            {
                reservationsView = value;
                NotifyPropertyChanged(nameof(ReservationsView));
            }
        }

        public TbReservation ReservationSelectionnee
        {
            get => reservationSelectionnee;
            set
            {
                reservationSelectionnee = value;
                NotifyPropertyChanged(nameof(ReservationSelectionnee));
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

        public ObservableCollection<TbClient> Clients
        {
            get => clients;
            set
            {
                clients = value;
                NotifyPropertyChanged(nameof(Clients));
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


        public ICommand InsertReservationCommand { get; }
        public ICommand EditReservationCommand { get; }
        public ICommand DeleteReservationCommand { get; }
        public ICommand SortingListCommand { get; }
        public ICommand PrintReservationCommand { get; }


        private void ApplyFilter()
        {
            if (string.IsNullOrEmpty(FilterText))
            {
                // Si le champ de texte est vide, afficher toutes les données
                ReservationsView.Filter = null;
            }
            else
            {
                // Sinon, appliquer le filtre sur les données
                ReservationsView.Filter = item =>
                {
                    var reservation = item as TbReservation;
                    return reservation != null &&
                           (reservation.FkResCliNavigation.NomCli.ToLower().Contains(FilterText.ToLower()) ||
                            reservation.FkResCliNavigation.PreCli.ToLower().Contains(FilterText.ToLower()));
                };
            }
        }

        private void SortingList(object parameter)
        {
            string sortColumn = (string)parameter;

            // Vérifier si la colonne est déjà triée
            if (ReservationsView.SortDescriptions.Any(sd => sd.PropertyName == sortColumn))
            {
                // Si oui, obtenir la direction de tri actuelle
                ListSortDirection currentDirection = ReservationsView.SortDescriptions.First(sd => sd.PropertyName == sortColumn).Direction;

                // Basculement entre le tri ascendant et descendant
                ListSortDirection newDirection = (currentDirection == ListSortDirection.Ascending) ? ListSortDirection.Descending : ListSortDirection.Ascending;

                // Effacer tous les tris existants pour cette colonne
                ReservationsView.SortDescriptions.Clear();

                // Ajouter le nouveau tri
                ReservationsView.SortDescriptions.Add(new SortDescription(sortColumn, newDirection));
            }
            else
            {
                // Si la colonne n'est pas déjà triée, ajouter un tri ascendant par défaut
                ReservationsView.SortDescriptions.Clear();
                ReservationsView.SortDescriptions.Add(new SortDescription(sortColumn, ListSortDirection.Ascending));
            }
        }




        //Fonction pour initialiser la collection(liste) de réservations
        private void InitializeCollectionView()
        {
            ReservationsView = CollectionViewSource.GetDefaultView(Reservations);
        }

        public ReservationViewModel()
        {

            reservationRepository = new ReservationRepository();
            Reservations = new ObservableCollection<TbReservation>(reservationRepository.GetAllReservations());
            InsertReservationCommand = new RelayCommand(o => InsertReservation());
            EditReservationCommand = new RelayCommand(o => EditReservation());
            DeleteReservationCommand = new RelayCommand(o => DeleteReservation());
            SortingListCommand = new RelayCommand(o => SortingList(o));
            PrintReservationCommand = new RelayCommand(o => PrintReservation());

        }

        private void PrintReservation()
        {
            string outputPath = AppDomain.CurrentDomain.BaseDirectory + "\\Reservations.pdf";

            PdfGenerator pdfGenerator = new PdfGenerator();

            pdfGenerator.GenerateReservationsPdf(ReservationsView, outputPath);

            if (System.IO.File.Exists(outputPath))
            {
                // Utilisez le processus de démarrage pour ouvrir le fichier PDF
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = outputPath,
                    UseShellExecute = true
                };

                Process.Start(startInfo);
            }
            else
            {
                MessageBox.Show("Le fichier PDF n'existe pas.");
            }
        }



        //cette fonction permet d'ajouter une réservation
        private void InsertReservation()
        {
            TbReservation newReservation = new TbReservation();

            ReservationForm InsertWindowNewReservationForm = new ReservationForm(newReservation);

            bool? result = InsertWindowNewReservationForm.ShowDialog();

            if (result == true)
            {

                newReservation = InsertWindowNewReservationForm.Reservation;

                foreach (TbReservation reservation in ReservationsView)
                {
                    if (newReservation.TbChambre.PkCha == reservation.TbChambre.PkCha && newReservation.TbChambre.PfkChaEtaNavigation.PkEta == reservation.TbChambre.PfkChaEtaNavigation.PkEta)
                    {
                        if (newReservation.DatArrRes >= reservation.DatArrRes && newReservation.DatArrRes <= reservation.DatDepRes)
                        {
                            MessageBox.Show("La chambre est déjà réservée pour cette période", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        if (newReservation.DatDepRes >= reservation.DatArrRes && newReservation.DatDepRes <= reservation.DatDepRes)
                        {
                            MessageBox.Show("La chambre est déjà réservée pour cette période", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                }

                reservationRepository.AddReservationRep(newReservation);
                Reservations.Add(newReservation);

            }
        }

        //cette fonction permet de modifier une réservation
        private void EditReservation()
        {
            if (ReservationSelectionnee == null)
            {
                MessageBox.Show("Veuillez sélectionner une réservation");
                return;
            }

            ReservationForm InsertWindowReservationForm = new ReservationForm(ReservationSelectionnee);

            bool? result = InsertWindowReservationForm.ShowDialog();

            if (result == true)
            {

                var editedReservation = InsertWindowReservationForm.Reservation;

                foreach (TbReservation reservation in ReservationsView)
                {
                    if (editedReservation.TbChambre.PkCha == reservation.TbChambre.PkCha && editedReservation.TbChambre.PfkChaEtaNavigation.PkEta == reservation.TbChambre.PfkChaEtaNavigation.PkEta)
                    {
                        if (editedReservation.DatArrRes >= reservation.DatArrRes && editedReservation.DatArrRes <= reservation.DatDepRes)
                        {
                            MessageBox.Show("La chambre est déjà réservée pour cette période", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        if (editedReservation.DatDepRes >= reservation.DatArrRes && editedReservation.DatDepRes <= reservation.DatDepRes)
                        {
                            MessageBox.Show("La chambre est déjà réservée pour cette période", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                }

                int index = Reservations.IndexOf(ReservationSelectionnee);

                Reservations[index] = editedReservation;

                reservationRepository.EditReservationRep(editedReservation);

                ReservationSelectionnee = editedReservation;
            }
                

        }

        private void DeleteReservation()
        {
            if (ReservationSelectionnee == null)
            {
                MessageBox.Show("Veuillez sélectionner une réservation");
                return;
            }

            if (MessageBox.Show($"Êtes-vous sûr de vouloir supprimer la réservation n°{ReservationSelectionnee.PkRes} ?", "Confirmation de suppression", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                //Supprimer dans la base de données
                reservationRepository.DeleteReservationRep(ReservationSelectionnee);

                // Supprimer l'objet Film dans la collection 
                Reservations.Remove(ReservationSelectionnee);
            }
        }



    }
}
