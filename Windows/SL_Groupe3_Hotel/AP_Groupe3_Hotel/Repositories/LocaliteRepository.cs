using AP_Groupe3_Hotel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_Groupe3_Hotel.Repositories
{
    /// <summary>
    /// Codé par : Alexandre
    /// Edité par : Alexandre
    /// </summary>
    class LocaliteRepository
    {   
        private readonly MyDBContext _dbContext;

        public LocaliteRepository()
        {
            _dbContext = new MyDBContext();
        }

        // Reprend les données de la base et les met dans une liste grâce au DbContext
        public List<TbLocalite> GetAllLocalites()
        {
            var localites = _dbContext.TbLocalites.ToList();

            return localites;
        }


        // Fait une liste de toutes les localités de la base de donnée
        public void InsertLocaliteRep(TbLocalite localite)
        {
            if (localite == null)
            {
                throw new ArgumentNullException(nameof(localite), "La localité ne peut pas être nulle.");
            }


            _dbContext.TbLocalites.Add(localite);
            _dbContext.SaveChanges();
        }
    }
}
