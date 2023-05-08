using CI_Platform.Entities.Data;
using CI_Platform.Entities.Models;
using CI_Platform.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repository
{
    
    public class CountryRepository : ICountryRepository
    {
        private readonly CiPlatformContext _db;

        public CountryRepository(CiPlatformContext db)
        {
            _db = db;
        }

        public List<Country> GetCountryData()
        {
            List<Country> countrylist = _db.Countries.ToList();
           
            return countrylist;
        }

        
    }
}
