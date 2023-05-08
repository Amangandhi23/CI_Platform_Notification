using CI_Platform.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModel
{
    public class Mission_data 
    {
        public IEnumerable<Mission> Mission { get; set; }

        public IEnumerable<MissionMedium> missionMedia { get; set; }

        public IEnumerable<Country> Country { get; set; }

        public IEnumerable<Skill> Skill { get; set; }

        public IEnumerable<MissionRating> MissionRating { get; set; }

        public IEnumerable<GoalMission> Goals { get; set; }

        public IEnumerable<Timesheet> timesheetdata { get; set; }

        public IEnumerable<MissionTheme> MissionTheme { get; set; }

        public IEnumerable<MissionMedium> MissionMedium { get; set; }

        public IEnumerable<MissionSkill> MissionSkill { get; set; }

        public IEnumerable<Comment> Comment { get; set; }   

        public IEnumerable<MissionApplication> MissionApplication { get; set; } 

        public IEnumerable<MissionApplication> Related_MissionApplication { get; set; }

        public IEnumerable<GoalMission> goalMissions { get; set; }

        public IEnumerable<FavoriteMission> FavoriteMission { get; set;}

        public IEnumerable<City> City { get; set; }

        public Mission mission_vol { get; set; }

        public IEnumerable<Mission> related_mission { get; set; }

        public IEnumerable<Comment> comments { get; set; }

        public IEnumerable<User> User { get; set; }

        public long UserId { get; set; }

        public long count { get; set; }
        public User user { get; set; }

        public long Sheet_count { get; set; }

        public int RatingCount { get; set; }


        public Pager pager { get; set; }

        
    }
}


