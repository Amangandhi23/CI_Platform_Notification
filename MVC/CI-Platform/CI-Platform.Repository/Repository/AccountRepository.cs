using CI_Platform.Entities.Data;
using CI_Platform.Entities.Models;
using CI_Platform.Entities.ViewModel;
using CI_Platform.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private CiPlatformContext _db;

        public AccountRepository(CiPlatformContext db)
        {
            _db = db;
        }


        public bool logged(Login model)
        {
            return _db.Users.Any(u => u.Email == model.Email && u.Password == model.Password);
        }

        public bool user_exsists_forRegistration(Registration model)
        {
            return _db.Users.Any(u => u.Email == model.Email );
        }

  
        
     
        public void user_exsists_forRegistration(User user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();
        }
    }
}
