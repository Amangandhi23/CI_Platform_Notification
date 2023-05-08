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
    public class SkillRepository : ISkillRepository
    {
        private readonly CiPlatformContext _db;

        public SkillRepository(CiPlatformContext db)
        {
            _db = db;
        }

        
        public List<Skill> GetSkillData()
        {
            List<Skill> skilllist = _db.Skills.ToList();

            return skilllist;
        }

        public List<UserSkill> GetUserSkillData(long userid)
        {
            List<UserSkill> userSkill = _db.UserSkills.Where(user => user.UserId == userid).Include(s => s.Skill).ToList();
            return userSkill;
        }
        public List<MissionSkill> GetMissionSkillData()
        {
            List<MissionSkill> missionskilllist = _db.MissionSkills.ToList();

            return missionskilllist;
        }
    }
}
