using CI_Platform.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModel
{
    public class StoryModel
    {
        public IEnumerable<Story> Stories { get; set; }

        public IEnumerable<MissionTheme> MissionTheme { get; set; }

        public IEnumerable<MissionMedium> missionMedia { get; set; }

        public IEnumerable<User> User { get; set; }

        public long UserId { get; set; }

        /*[BindProperty]*/

        public Story Storydata { get; set; }

        public long storyuserid { get; set; }

        public long StoryId { get; set; }

        public long MissionId { get; set; }

        public long StoryIdsForPriview { get; set; }    

        [Required]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "The title must be between 5 and 255 characters long.")]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(40000, ErrorMessage = "The Story Content Range in 40000 characters.")]
        public string Description { get; set; } = null!;

        public IEnumerable<MissionApplication> MissionApplication { get; set; }

        public IEnumerable<StoryMedium> StoryMedium { get; set; }

        public IEnumerable<StoryMedium> storymedia { get; set; }

        public List<string> videoUrl { get; set; }
    }
}
