using AP_Groupe3_Hotel.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace AP_Groupe3_Hotel.Repositories
{
    /// <summary>
    /// Codé par Alexandre
    /// Repository pour gérer les opérations CRUD sur l'entité TbChambre.
    /// </summary>
    class ChambreRepository
    {
        private readonly MyDBContext _dbContext;

        public ChambreRepository()
        {
            _dbContext = new MyDBContext();
        }

        /// <summary>
        /// Récupère toutes les chambres avec leurs étages associés depuis la base de données.
        /// </summary>
        /// <returns>Liste de toutes les chambres avec détails d'étage.</returns>
        public List<TbChambre> GetAllChambres()
        {
            var chambres = _dbContext.TbChambres
                                     .Include(c => c.PfkChaEtaNavigation) // Inclure l'entité TbEtage associée
                                     .ToList();

            return chambres;
        }

        /// <summary>
        /// Ajoute une nouvelle chambre à la base de données.
        /// </summary>
        /// <param name="chambre">La chambre à ajouter.</param>
        public void AddChambreRep(TbChambre chambre)
        {
            if (chambre == null)
            {
                throw new ArgumentNullException(nameof(chambre), "La chambre ne peut pas être nulle.");
            }

            // Vérifie si l'étage associé à la chambre existe
            using (MyDBContext _dbContext = new MyDBContext())
            {
                bool isUnique = false;

                // Vérifie l'unicité de CodeCha pour l'étage
                while (!isUnique)
                {
                    // Concaténation de PfkChaEta (le PK de TbEtage) et CodeCha pour obtenir l'étage et le numéro de la chambre
                    string chambreNumero = $"{chambre.PfkChaEta}{chambre.CodeCha:D2}";

                    // Vérifie si la combinaison de PfkChaEta et CodeCha existe déjà
                    var existingChambre = _dbContext.TbChambres
                        .FirstOrDefault(c => c.PfkChaEta == chambre.PfkChaEta && c.CodeCha == chambre.CodeCha);

                    // Si la chambre existe déjà avec le même CodeCha dans l'étage, incrémenter CodeCha
                    if (existingChambre != null)
                    {
                        chambre.CodeCha++;
                    }
                    else
                    {
                        isUnique = true;
                    }
                }

                // Récupérer l'étage associé à la chambre
                var existingEtage = _dbContext.TbEtages.Find(chambre.PfkChaEta);

                if (existingEtage == null)
                {
                    throw new Exception($"L'étage avec l'ID {chambre.PfkChaEta} n'existe pas dans la base de données.");
                }

                // Associer l'étage à la chambre
                chambre.PfkChaEtaNavigation = existingEtage;

                // Ajouter la chambre au contexte et sauvegarder les modifications
                _dbContext.TbChambres.Add(chambre);
                _dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Met à jour les données d'une chambre dans la base de données.
        /// </summary>
        /// <param name="chambre">La chambre à mettre à jour.</param>
        public void EditChambreRep(TbChambre chambre)
        {
            if (chambre == null)
            {
                throw new ArgumentNullException(nameof(chambre), "La chambre ne peut pas être nulle.");
            }

            using (MyDBContext _dbContext = new MyDBContext())
            {
                // Charger l'entité existante de la base de données
                var existingChambre = _dbContext.TbChambres
                                                .Include(c => c.PfkChaEtaNavigation) // Charge l'étage associé
                                                .FirstOrDefault(c => c.PkCha == chambre.PkCha);

                if (existingChambre == null)
                {
                    throw new Exception($"La chambre avec l'ID {chambre.PkCha} n'existe pas dans la base de données.");
                }

                // Vérifier l'unicité de CodeCha pour le nouvel étage
                bool isUnique = false;
                while (!isUnique)
                {
                    // Vérifie si la combinaison de PfkChaEta et CodeCha existe déjà
                    var existingChambreWithCode = _dbContext.TbChambres
                        .FirstOrDefault(c => c.PfkChaEta == chambre.PfkChaEta && c.CodeCha == chambre.CodeCha);

                    // Si la chambre existe déjà avec le même CodeCha dans le nouvel étage, incrémenter CodeCha
                    if (existingChambreWithCode != null && existingChambreWithCode.PkCha != chambre.PkCha)
                    {
                        chambre.CodeCha++;
                    }
                    else
                    {
                        isUnique = true;
                    }
                }

                // Créer une nouvelle entité TbChambre avec les nouvelles valeurs
                var newChambre = new TbChambre
                {
                    PkCha = existingChambre.PkCha,
                    CodeCha = chambre.CodeCha,
                    CapCha = chambre.CapCha,
                    PrixCha = chambre.PrixCha,
                    PfkChaEta = chambre.PfkChaEta,
                    PfkChaEtaNavigation = _dbContext.TbEtages.Find(chambre.PfkChaEta),
                    TbReservations = existingChambre.TbReservations // Conserver les réservations existantes
                };

                if (newChambre.PfkChaEtaNavigation == null)
                {
                    throw new Exception($"L'étage avec l'ID {chambre.PfkChaEta} n'existe pas dans la base de données.");
                }

                // Supprimer l'entité existante
                _dbContext.TbChambres.Remove(existingChambre);
                _dbContext.SaveChanges(); // Confirmer la suppression

                // Ajouter la nouvelle entité
                _dbContext.TbChambres.Add(newChambre);
                _dbContext.SaveChanges(); // Sauvegarder les modifications
            }
        }

        /// Supprime une chambre de la base de données.
        /// </summary>
        /// <param name="chambreId">L'identifiant de la chambre à supprimer.</param>

        /// Supprime une chambre de la base de données.
        /// </summary>
        /// <param name="chambreId">L'identifiant de la chambre à supprimer.</param>
        public void DeleteChambreRep(TbChambre chambre)
        {
            if (chambre == null)
            {
                throw new ArgumentNullException(nameof(chambre), "La chambre ne peut pas être nulle.");

            }

            var existingChambre = _dbContext.TbChambres.Local
                                                        .FirstOrDefault(c => c.PkCha == chambre.PkCha) ??
                                                        _dbContext.TbChambres.Find(chambre.PkCha);

            if (existingChambre != null)
            {
                chambre = existingChambre;
            }

            _dbContext.Entry(chambre).State = EntityState.Unchanged;
            _dbContext.TbChambres.Remove(chambre);
            _dbContext.SaveChanges();
        }
    }
}