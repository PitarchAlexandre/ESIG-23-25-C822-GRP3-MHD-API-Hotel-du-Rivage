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
    /// <summary>
    /// Codé par : Alexandre
    /// Edité par : Alexandre
    /// 
    ///La classe ClientRepository permet de gérer les données de l'entité TbClient dans la base de données. 
    ///Elle fournit des méthodes pour créer, lire, mettre à jour et supprimer ces données.
    /// 
    /// Note pour moi-même :
    /// Un repository peut écrire directement dans la base de données en utilisant les données fournies par l'application.
    /// Il agit comme une couche intermédiaire qui simplifie ces opérations et masque les détails techniques de la base de données sous-jacente.
    /// </summary>
    class ClientRepository
    {
        private readonly MyDBContext _dbContext;

        public ClientRepository()
        {
            _dbContext = new MyDBContext();
        }
        /// <summary>
        /// Fait une liste de tous les clients de la base de donnée
        /// </summary>
        /// <returns>Toute la table TbClients</returns>
        public List<TbClient> GetAllClients()
        {
            var clients = _dbContext.TbClients.Include(f => f.FkCliLocNavigation)
                                              .Include(f => f.TbReservations)
                                              .ThenInclude(f => f.TbChambre)
                                              .ThenInclude(f => f.PfkChaEtaNavigation)
                                              .ToList();

            return clients;
        }
        /// <summary>
        /// Ajoute un nouveau client à la base de données.
        /// </summary>
        /// <param name="client">Le client à ajouter.</param>
        public void InsertClientRep(TbClient client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client), "Le client ne peut pas être nul.");
            }
            var existingLocalite = _dbContext.TbLocalites.Local
                                                         .FirstOrDefault(l => l.PkLoc == client.FkCliLoc) ??
                                                         _dbContext.TbLocalites.Find(client.FkCliLoc);

            if (existingLocalite != null)
            {
                client.FkCliLocNavigation = existingLocalite;
            }

            _dbContext.TbClients.Add(client);
            _dbContext.SaveChanges();
        }
        /// <summary>
        /// Modifie les données d'un client et l'actualise dans la base de donnée Infomaniak
        /// </summary>
        /// <param name="client"></param>
        public void EditClientRep(TbClient client)
        {
            var existingClient = _dbContext.TbClients.Find(client.PkCli);

            // Vérifier si le client existe dans la base de données
            if (existingClient != null)
            {
                // Mettre à jour les valeurs de l'entité existante
                _dbContext.Entry(existingClient).CurrentValues.SetValues(client);

            }
            else
            {
                // Ajouter le nouveau client si l'entité n'existe pas
                _dbContext.TbClients.Update(client);
            }

            _dbContext.SaveChanges();
        }




        /// <summary>
        /// Efface un client et l'actualise dans la base de donnée Infomaniak
        /// </summary>
        /// <param name="client"></param>
        public void DeleteClientRep(TbClient client)
        {
            var ExistingClient = _dbContext.TbClients.Find(client.PkCli);
            if (ExistingClient != null)
            {
                _dbContext.TbClients.Remove(ExistingClient);
                _dbContext.SaveChanges();
            }
        }
        /// <summary>
        /// Controle si le client a des réservations
        /// </summary>
        /// <param name="client"></param>
        public bool ControlClientReservation(int clientId)
        {
            var reservations = _dbContext.TbReservations.Where(r => r.FkResCli == clientId).ToList();
            return reservations.Any();
        }

    }
}
