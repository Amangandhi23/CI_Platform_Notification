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
    public class MissionRepository : IMissionRepository
    {
        private readonly CiPlatformContext _db;

        public MissionRepository(CiPlatformContext db)
        {
            _db = db;
        }

        public List<Mission> GetMissionData()
        {
            List<Mission> missionlist = _db.Missions.Include(m=> m.Country).Include(m=> m.City).Include(m => m.Theme).Include(m => m.GoalMissions).Include(m => m.Timesheets).Include(m => m.FavoriteMissions).Include(m => m.MissionSkills).Include(m => m.MissionMedia).ToList();

            return missionlist;
        }

        public List<MissionApplication> GetMissionApplications()
        {
            List<MissionApplication> missionApplications= _db.MissionApplications.Include(user => user.User).ToList();
            return missionApplications;
        }

        public List<MissionApplication> GetMissionApplications(long userid)
        {
            return _db.MissionApplications.Include(m => m.Mission).Where(user => user.UserId == userid && user.DeletedAt == null).ToList();
        }

    }
}
