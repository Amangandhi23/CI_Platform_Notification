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
    public class AdminRepository : IAdminRepository
    {
        private readonly CiPlatformContext _db;

        public AdminRepository(CiPlatformContext db)
        {
            _db = db;
        }


        public List<CmsPage> CMSdata()
        {
            List<CmsPage> cmsPages = _db.CmsPages.Where(id => id.DeletedAt == null).ToList();
            return cmsPages;
        }

        public List<Banner> Bannerdata()
        {
            List<Banner> banners = _db.Banners.Where(id => id.DeletedAt == null).ToList();    
            return banners;
        }

        public List<Story> StoryData()
        {
            List<Story> stories = _db.Stories.Where(status => status.Status != "DRAFT" && status.DeletedAt == null).Include(user => user.User).Include(mission => mission.Mission).ToList();
            return stories;
        }

        public List<StoryMedium> GetStoryMedia()
        {
            return _db.StoryMedia.ToList();
        }

        public List<GoalMission> goalMissions()
        {
            return _db.GoalMissions.ToList();
        }

        public List<User> GetUserData()
        {
            return _db.Users.ToList();
        }

        public List<MissionMedium> GetMissionMediumData()
        {
            return _db.MissionMedia.ToList();
        }

        public List<MissionDocument> GetMissionDocuments()
        {
            return _db.MissionDocuments.ToList();
        }
    }
}
