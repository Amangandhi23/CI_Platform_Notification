using CI_Platform.Entities.Data;
using CI_Platform.Repository.Interface;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using CI_Platform.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace CI_Platform.Repository.Repository
{
    public class StoryRepository : IStoryRepository
    {
        private readonly CiPlatformContext _db;

        public StoryRepository(CiPlatformContext db)
        {
            _db = db;
        }

        public void updateStory(Story story)
        {
            var model = _db.Entry(story);
            model.State = EntityState.Modified;
            _db.SaveChanges();
        }

        public List<Story> GetStoryData()
        {
            List<Story> data = _db.Stories.Where(m => m.Status != "DECLINED" && m.Status != "PENDING" && m.DeletedAt == null).Include(m => m.User).Include(m => m.Mission).ThenInclude(m => m.MissionApplications).Include(m => m.StoryMedia).ToList();
            return data;
        }
    }
}
