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
   public class VolunteerRepository : IVolunteerRepository
    {
        private readonly CiPlatformContext _db;
        private IMissionRepository _mission;

        public VolunteerRepository(CiPlatformContext db ,IMissionRepository mission)
        {
            _db = db;
            _mission = mission;
        }


        public Mission GetLandingMissionData(long id)
        {
            Mission mission = _db.Missions.Include(m => m.Country).Include(e => e.MissionDocuments).Include(m => m.City).Include(m => m.Theme).Include(m => m.MissionSkills.Where(skill => skill.DeletedAt == null)).Include(m => m.MissionSkills).Include(m => m.FavoriteMissions).FirstOrDefault(m => m.MissionId == id);
            return mission;

        }



        public IEnumerable<Mission> GetRelatedMission(long id)
        {
            Mission ms = GetLandingMissionData(id);
            List<Mission> missions = _db.Missions.Include(m => m.City).Include(m => m.Theme).Where(theme => theme.DeletedAt == null).Include(m => m.Country).Include(timesheet => timesheet.Timesheets).ToList();
            List<Mission> RelatedMission = new List<Mission>();
            foreach (Mission m in missions)
            {
                if(m.City.CityId == ms.CityId)
                {
                    RelatedMission.Add(m);
                }
            }

            foreach (Mission m in missions)
            {
                if (m.Theme.MissionThemeId == ms.ThemeId)
                {
                    RelatedMission.Add(m);
                }
            }

            foreach (Mission m in missions)
            {
                if (m.Country.CountryId == ms.CountryId)
                {
                    RelatedMission.Add(m);
                }
            }
            RelatedMission = RelatedMission.Distinct().Take(3).ToList();
            return RelatedMission;

        }


     

        public List<User> Getuser_data()
        {
            List<User> users = _db.Users.ToList();
            return users;
        }

        public int GetMissionRating_data(long id, long UserId)
        {
           
            List<MissionRating> Rating_data = _db.MissionRatings.Include(m => m.Mission).Include(m => m.User).ToList();
            
            foreach(var rating in Rating_data)
            {
                if(rating.MissionId == id && rating.UserId == UserId)
                {
                    return rating.Rating;
                }
            }
            return 0;
            
        }

        public IEnumerable<MissionRating> Rating_data()
        {

            List<MissionRating> Rating_data = _db.MissionRatings.Include(m => m.Mission).Include(m => m.User).ToList();
            return Rating_data;

        }

        public List<Skill> GetFilterSkill(long id)
        {
            List<MissionSkill> MSkill = _db.MissionSkills.Where(m => m.MissionId == id).ToList();
            List<Skill> skill = _db.Skills.ToList();

            return skill;
        }
        
        public List<MissionApplication> GetRecentVolunteer(long id)
        {
            List<MissionApplication> missionApplications = _db.MissionApplications.Where(m => m.MissionId == id).Include(m => m.User).ToList();
            return missionApplications;
        }

        public MissionRating RatingData(long id, long userid)
        {
            MissionRating rating = _db.MissionRatings.FirstOrDefault(m => m.MissionId == id && m.UserId == userid);
            if(rating == null)
            {
                rating = new MissionRating();
            }
            
            return rating;
            
        }

        public List<Comment> Getcomment_data(long id)
        {
            List<Comment> comments = _db.Comments.Include(m => m.Mission).Include(m => m.User).OrderByDescending(m => m.CreatedAt).Where(m => m.Mission.MissionId == id).ToList();
            return comments;
        }
    }
}
