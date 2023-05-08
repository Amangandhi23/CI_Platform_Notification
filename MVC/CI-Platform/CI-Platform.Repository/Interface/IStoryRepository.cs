using CI_Platform.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IStoryRepository
    {
        public List<Story> GetStoryData();

        public void updateStory(Story story);
    }
}
