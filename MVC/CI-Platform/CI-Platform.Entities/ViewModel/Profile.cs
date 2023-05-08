using CI_Platform.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModel
{
    public class Profile
    {
        static DateTime newDateTime = DateTime.Parse(DateTime.Now.ToShortDateString());


        public User LoginUser { get; set; }

        public ContactUs ContactUs { get; set; }

        public List<Country> CountryList { get; set; }

        public List<City> CityList { get; set; }

        public List<CmsPage> Policy { get; set; }

        public List<Skill> SkillList { get; set; }

        public List<UserSkill> UserSkills { get; set; }

        public List<MissionApplication> missionApplications { get; set; }

        public List<Mission> MissionList { get; set; }

        public List<Timesheet> timesheets { get; set; }


        [Required(ErrorMessage = "Title is required")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Title is required")]
        public long MissionId { get; set; }

        
        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }


        [Range(0, 23, ErrorMessage = "Please enter value between 0 to 23")]
        [Required(ErrorMessage = "Hours is required")]
        public int hours { get; set; }

        [Range(0, 59, ErrorMessage = "Please enter value between 0 to 59")]
        [Required(ErrorMessage = "Minutes is required")]
        public int minutes { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Please enter greater or equal to zero value")]
        [Required(ErrorMessage = "Action is required")]
        public int action { get; set; }


        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        public long timesheetsId { get; set; }

        public String message { get; set; }

        public string CityId { get; set; }

        public string CountryId { get; set; }


        [Required(ErrorMessage = "Name is required")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Surname is required")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Profile text is required")]
        public string MyProfile { get; set; }


        [Required(ErrorMessage = "Old Password is required")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }


        [Required(ErrorMessage = "New Password is required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }


        [Required(ErrorMessage = "Password is required")]
        /*[Compare("NewPassword", ErrorMessage = "Password can not match")]*/
        public string ConfirmNewPassword { get; set; }
        // public IEnumerable<User> Users { get; set; } = Enumerable.Empty<User>();
    }
}
