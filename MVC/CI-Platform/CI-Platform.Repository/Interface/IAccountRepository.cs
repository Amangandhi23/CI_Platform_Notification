using CI_Platform.Entities.ViewModel;
using CI_Platform.Entities.Models;

namespace CI_Platform.Repository.Interface
{
    public interface IAccountRepository
    {
        //public List<Login> GetUserData();

        public bool logged(Login model);

        public bool user_exsists_forRegistration(Registration model);

        public void user_exsists_forRegistration(User user);

                                    
    }
}
