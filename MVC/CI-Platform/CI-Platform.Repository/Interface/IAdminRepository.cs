using CI_Platform.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IAdminRepository
    {
        public List<CmsPage> CMSdata();

        public List<Story> StoryData();

        public List<User> GetUserData();

        public List<Banner> Bannerdata();

        public List<GoalMission> goalMissions();

        public List<MissionMedium> GetMissionMediumData();

        public List<StoryMedium> GetStoryMedia();

        public List<MissionDocument> GetMissionDocuments();

    }
}
