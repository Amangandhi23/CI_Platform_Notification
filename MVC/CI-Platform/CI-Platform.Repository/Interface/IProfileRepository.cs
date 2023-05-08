using CI_Platform.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IProfileRepository
    {
        public List<Timesheet> GetTimeSheetData(long userid);
    }
}
