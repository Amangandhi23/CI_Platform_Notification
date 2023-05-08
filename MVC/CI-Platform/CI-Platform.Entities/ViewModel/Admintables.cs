using CI_Platform.Entities.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModel
{
    public class Admintables
    {

        public User user { get; set; }

        public IEnumerable<User> UserList { get; set; }

        public IEnumerable<Mission> MissionList { get; set; }

        public List<MissionApplication> missionApplications { get; set; }

        public List<Skill> SkillList { get; set; }

        public List<MissionTheme> MissionThemeList { get; set; }

        public IEnumerable<CmsPage> CmsPageList { get; set; }

        public List<MissionDocument> MissionDocumentList { get; set;}

        public List<Banner> BannerList { get; set; }

        public List<Story> StoryList { get; set; }

        public Story StoryDetail { get; set; }

        public IEnumerable<StoryMedium> storyMediaList { get; set; }    

        public List<City> CityList { get; set; }
        
        public List<Country> CountriesList { get; set; }

        public List<MissionTheme> missionThemes { get; set; }

        public List<MissionMedium> MissionMediumList { get; set;}


        public List<IFormFile> Images { get; set; }


        public List<IFormFile> Document { get; set; }

        public string Documentstr { get; set; }

        public string BannerImg { get; set; }

        public List<IFormFile> BannerImg1 { get; set; }


        public string Statuscheck { get; set; }

        public string TableName { get; set; }


        



        /* For Validation Purpose */
        public string SkillName { get; set; }

        [Required(ErrorMessage = "Please Enter FirstName Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email Address is required")]
        [EmailAddress(ErrorMessage = "Envalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter Status")]
        public string Status { get; set; }

        public long UserId { get; set; }
        public long CityID { get; set; }

        public long CountryId { get; set; }

        [Required(ErrorMessage = "Please Enter Phone Number")]
        public int PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please Enter EmployeeId")]
        public string EmployeeId { get; set; }

        [Required(ErrorMessage = "Please Enter Department Name")]
        public string DepartmentName { get; set; }

        public Missiontable Missiontable { get; set; }



        public long SkillId { get; set; }
        public long MissionId { get; set; }

        public long ThemeId { get; set; }

        public string Title { get; set; } = null!;

        public string? ShortDescription { get; set; }

        public string? Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string MissionType { get; set; } = null!;

        public string? OrganizationName { get; set; }

        public string? OrganizationDetail { get; set; }

        public string? Availability { get; set; }

        public long TotalSheet { get; set; }

        public DateTime deadLine { get; set; }

        public string? GoalObjectiveText { get; set; }

        public int GoalValue { get; set; }

        public string SkillIdArr { get; set; }

        public string VideoUrlString { get; set; }

        /*public  long[] SkillIdArr = new long[10];*/

        public string Imagedraft { get; set; }



        public long CmsPageId { get; set; }

        public string Slug { get; set; }


        public long BannerId { get; set; }

        public string? Text { get; set; }

        public int? SortOrder { get; set; }

        public string Department { get; set; }

        public long Phonenumber { get; set; }
    }
}
