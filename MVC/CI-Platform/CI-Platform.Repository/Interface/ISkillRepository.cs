using CI_Platform.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface ISkillRepository
    {
        public List<Skill> GetSkillData();

        public List<UserSkill> GetUserSkillData(long userid);
        public List<MissionSkill> GetMissionSkillData();
    }
}
