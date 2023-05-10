using CI_Platform.Entities.Models;
using CI_Platform.Entities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IMissiondataRepository
    {
        public Mission_data GetMissiondata();

        public IEnumerable<Mission> ApplyFilter(string[] country, string[] city, string[] skill, string[] theme, string sort, long userid, string search,string Explore);

    }
}
