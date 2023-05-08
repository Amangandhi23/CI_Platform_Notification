using CI_Platform.Entities.Models;
using CI_Platform.Entities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IVolunteerRepository
    {
        public Mission GetLandingMissionData(long id);
        public IEnumerable<Mission> GetRelatedMission(long id);

        public List<User> Getuser_data();

        public List<MissionApplication> GetRecentVolunteer(long id);
        public int GetMissionRating_data(long id, long UserId);

        public IEnumerable<MissionRating> Rating_data();

        public List<Comment> Getcomment_data(long id);

        public MissionRating RatingData(long id, long userid);

    }
}
