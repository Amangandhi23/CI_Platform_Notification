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
    public class CityRepository : ICityRepository
    {
        private readonly CiPlatformContext _db;

        public CityRepository(CiPlatformContext db)
        {
            _db = db;
        }

        public List<City> GetCityData()
        {
            List<City> citylist = _db.Cities.ToList();

            return citylist;
        }
    }
}
