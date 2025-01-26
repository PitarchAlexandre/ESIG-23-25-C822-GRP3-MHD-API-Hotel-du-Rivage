using AP_Groupe3_Hotel.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AP_Groupe3_Hotel.Repositories
{
    class ReservationRepository
    {
        private readonly MyDBContext _dbContext;

        public ReservationRepository()
        {
            _dbContext = new MyDBContext();
        }

        //Reprend les données de la base et les met dans une liste grâce au DbContext
        //Inclus la table des chambres, des étages et des clients
        public List<TbReservation> GetAllReservations()
        {
            var reservations = _dbContext.TbReservations.Include(cli => cli.FkResCliNavigation)
                                                        .ThenInclude(loc => loc.FkCliLocNavigation)
                                                        .Include(f => f.TbChambre)
                                                        .ThenInclude(c => c.PfkChaEtaNavigation)
                                                        .ToList();


            return reservations;


        }

        //Ajoute une réservation à la base de donnée
        public void AddReservationRep(TbReservation reservation)
        {
            if (reservation == null)
            {
                throw new ArgumentNullException(nameof(reservation), "La réservation ne peut pas être nulle.");
            }


            // Vérifier si le client existe dans la base de données
            var existingClient = _dbContext.TbClients
                .Local
                .FirstOrDefault(c => c.PkCli == reservation.FkResCli) ??
                _dbContext.TbClients.Find(reservation.FkResCli);

            if (existingClient != null)
            {
                reservation.FkResCliNavigation = existingClient;
            }

            // Vérifier si la chambre existe dans la base de données
            var existingChambre = _dbContext.TbChambres
                .Local
                .FirstOrDefault(c => c.PkCha == reservation.FkResCha && c.PfkChaEta == reservation.FkResChaEta) ??
                _dbContext.TbChambres.Find(reservation.FkResCha, reservation.FkResChaEta);

            if (existingChambre != null)
            {
                reservation.TbChambre = existingChambre;
            }

            // Vérifier si l'étage existe dans la base de données
            var existingEtage = _dbContext.TbEtages
                .Local
                .FirstOrDefault(e => e.PkEta == reservation.FkResChaEta) ??
                _dbContext.TbEtages.Find(reservation.FkResChaEta);

            if (existingEtage != null)
            {
                // Associer l'étage à la chambre si nécessaire
                if (existingChambre != null)
                {
                    existingChambre.PfkChaEtaNavigation = existingEtage;
                }
            }
            _dbContext.TbReservations.Add(reservation);
            _dbContext.SaveChanges();
        }

        //Modifie les données d'une réservation et l'actualise dans la base de donnée
        public void EditReservationRep(TbReservation reservation)
        {

            var existingReservaation = _dbContext.TbReservations.Find(reservation.PkRes);
            if (existingReservaation != null)
            {
                _dbContext.Entry(existingReservaation).CurrentValues.SetValues(reservation);
            }
            else
            {
                _dbContext.TbReservations.Update(reservation);
            }
            _dbContext.SaveChanges();

    }

        public void DeleteReservationRep(TbReservation reservation)
        {
            if (reservation == null)
            {
                throw new ArgumentNullException(nameof(reservation), "La réservation ne peut pas être nulle.");
            }

            var existingReservation = _dbContext.TbReservations
                .Local
                .FirstOrDefault(e => e.PkRes == reservation.PkRes) ??
                _dbContext.TbReservations.Find(reservation.PkRes);

            if (existingReservation != null)
            {
                reservation = existingReservation;
            }

            _dbContext.TbReservations.Remove(reservation);
            _dbContext.SaveChanges();

        }
    }
}
