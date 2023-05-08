using CI_Platform.Entities.Data;
using CI_Platform.Entities.Models;
using CI_Platform.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repository
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly CiPlatformContext _db;

        public ProfileRepository(CiPlatformContext db)
        {
            _db = db;
        }

        public List<Timesheet> GetTimeSheetData(long userid)
        {
            List<Timesheet> timesheets = _db.Timesheets.Where(u => u.UserId == userid && u.DeletedAt == null).Include(m => m.Mission).ToList();
            return timesheets;
        }
    }
}
