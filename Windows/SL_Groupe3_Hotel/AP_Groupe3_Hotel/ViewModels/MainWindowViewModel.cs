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
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
namespace AP_Groupe3_Hotel.ViewModels
{
    /// <summary>
    /// Codé par : Alexandre
    /// Modifié par : 
    /// </summary>
    class MainWindowViewModel : BaseViewModel
    {
        // Commandes pour les boutons du menu
        public ICommand DisplayReservationViewCommand { get; }
        public ICommand DisplayClientViewCommand { get; }
        public ICommand DisplayChambreViewCommand { get; }

        public MainWindowViewModel()
        {
            // Initialisation des commandes
            DisplayReservationViewCommand = new RelayCommand(DisplayReservationView);
            DisplayClientViewCommand = new RelayCommand(DisplayClientView);
            DisplayChambreViewCommand = new RelayCommand(DisplayChambreView);
        }

        /// <summary>
        /// Méthode pour afficher la vue des réservations
        /// </summary>
        /// <param name="parameter"></param>
        private void DisplayReservationView(object parameter)
        {
            ReservationView InsertWindowReservationView = new ReservationView();
            InsertWindowReservationView.Show();
        }

        /// <summary>
        /// Méthode pour afficher la vue des clients
        /// </summary>
        /// <param name="parameter"></param>
        private void DisplayClientView(object parameter)
        {
            ClientView WindowClientView = new ClientView();
            WindowClientView.Show();
        }

        // Méthode pour afficher la vue de gestion des chambres
        private void DisplayChambreView(object parameter)
        {
            ChambreView WindowChambreView = new ChambreView();
            WindowChambreView.Show();
        }
    }
}
