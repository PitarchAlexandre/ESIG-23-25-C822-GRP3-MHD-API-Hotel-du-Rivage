﻿using AP_Groupe3_Hotel.ViewModels;
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
    /// Logique d'interaction pour ChambreView.xaml
    /// </summary>
    public partial class ChambreView : Window
    {
        public ChambreView()
        {
            InitializeComponent();
            this.DataContext = new ChambreViewModel();
        }
        /// <summary>
        /// Retourne à la page précédente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChambreViewRetour_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void mitQuitter_click(object sender, RoutedEventArgs e)
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

        private void btnQuestionMark_click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Si vous avez des questions, appelez notre hotline au :\n 032 420 77 90.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}
