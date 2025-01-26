using AP_Groupe3_Hotel.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_Groupe3_Hotel.Repositories
{
    class EtageRepository
    {
        private readonly MyDBContext _dbContext;

        public EtageRepository()
        {
            _dbContext = new MyDBContext();
        }

        /// <summary>
        /// Reprend les données de la base et les met dans une liste grâce au DbContext
        /// </summary>
        /// <returns></returns>
        public List<TbEtage> GetAllEtages()
        {
            var etages = _dbContext.TbEtages.ToList();

            return etages;
        }
    }
}
