using CI_Platform.Entities.Data;
using CI_Platform.Entities.Models;
using CI_Platform.Entities.ViewModel;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CI_Platform.Controllers
{
    public class ProfileController : Controller
    {
        private readonly CiPlatformContext _db;
        private ICountryRepository _country;
        private ICityRepository _city;
        private ISkillRepository _skill;
        private IMissionRepository _mission;
        private IProfileRepository _profile;
        private IPasswordRepository _password;
        public ProfileController(CiPlatformContext db, IPasswordRepository password, ICountryRepository country, ICityRepository city, ISkillRepository skill, IMissionRepository mission, IProfileRepository profile)
        {
            _db = db;
            _country = country;
            _city = city;
            _skill = skill;
            _mission = mission;
            _profile = profile;
            _password = password;
        }

        public IActionResult UserProfile()
        {
            var user_session = HttpContext.Session.GetString("Login");
            var userid = _db.Users.FirstOrDefault(m => m.Email == user_session).UserId;

            /*ViewBag.username = _db.Users.FirstOrDefault(m => m.Email == user_session).FirstName + _db.Users.FirstOrDefault(m => m.Email == user_session).LastName;*/


            Profile pf = new Profile();
            pf.LoginUser = _db.Users.FirstOrDefault(m => m.Email == user_session);
            pf.CountryList = _country.GetCountryData();
            pf.CityList = _city.GetCityData();
            pf.SkillList = _skill.GetSkillData().Where(status => status.DeletedAt == null).ToList();
            pf.MyProfile = _db.Users.FirstOrDefault(m => m.Email == user_session).ProfileText;
            pf.UserSkills = _skill.GetUserSkillData(userid).Where(userskilldatacheck => userskilldatacheck.DeletedAt == null).ToList();

            return View(pf);
        }

        public IActionResult GetAllSkillData()
        {
            var user_session = HttpContext.Session.GetString("Login");
            var userid = _db.Users.FirstOrDefault(m => m.Email == user_session).UserId;

            if (HttpContext.Session.GetString("username") != null)
            {
                ViewBag.username = HttpContext.Session.GetString("username");
            }
            var data = _skill.GetUserSkillData(userid).Where(status => status.DeletedAt == null);
            return Json(data);
        }

        [HttpPost]
        public IActionResult UserProfile(Profile model, string CityList, string CountryList, string SkillList)
        {
            var user_session = HttpContext.Session.GetString("Login");
            var userid = _db.Users.FirstOrDefault(m => m.Email == user_session).UserId;

            ViewBag.username = _db.Users.FirstOrDefault(m => m.Email == user_session).FirstName + " " + _db.Users.FirstOrDefault(m => m.Email == user_session).LastName;


            bool flag = true;
            if (!ModelState.IsValid)
            {
                var nameValidationState = ModelState.GetFieldValidationState("FirstName");
                if (nameValidationState == ModelValidationState.Invalid)
                {
                    flag = false;
                }
                var nameValidationState1 = ModelState.GetFieldValidationState("LastName");
                if (nameValidationState1 == ModelValidationState.Invalid)
                {
                    flag = false;
                }
                var nameValidationState2 = ModelState.GetFieldValidationState("MyProfile");
                if (nameValidationState2 == ModelValidationState.Invalid)
                {
                    flag = false;
                }

            }
            if (flag)
            {
                var User = _db.Users.FirstOrDefault(u => u.Email == user_session);
                var UserSkill = _db.UserSkills.Where(u => u.UserId == userid).ToList();
                User.FirstName = model.FirstName;
                User.LastName = model.LastName;
                User.EmployeeId = model.LoginUser.EmployeeId;
                User.Title = model.LoginUser.Title;
                User.Department = model.LoginUser.Department;
                User.ProfileText = model.MyProfile;
                User.WhyIVolunteer = model.LoginUser.WhyIVolunteer;
                User.CountryId = Convert.ToInt64(CountryList);
                User.CityId = Convert.ToInt64(CityList);
                User.LinkedInUrl = model.LoginUser.LinkedInUrl;


                _db.UserSkills.RemoveRange(UserSkill);
                if (SkillList != null)
                {
                    string[] multiArray = SkillList.Split(new[] { "\r\n" }, StringSplitOptions.None);
                    var skills = multiArray.Select(u => _db.Skills.FirstOrDefault(m => m.SkillName == u)).ToList();
                    foreach (var skill in skills)
                    {
                        if (skill != null)
                        {
                            _db.UserSkills.Add(new Entities.Models.UserSkill { Skill = skill, UserId = userid });

                        }

                    }
                    _db.SaveChanges();
                }


                _db.Users.Update(User);
                _db.SaveChanges();


                return RedirectToAction("Landing", "Mission");
            }
            else
            {
                TempData["error"] = "Something Wrong here!";
                Profile pf = new Profile();
                pf.LoginUser = _db.Users.FirstOrDefault(m => m.Email == user_session);
                pf.CountryList = _country.GetCountryData();
                pf.CityList = _city.GetCityData();
                pf.SkillList = _skill.GetSkillData();
                pf.UserSkills = _skill.GetUserSkillData(userid);
                return View(pf);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(Profile passdetail)
        {
            var user_session = HttpContext.Session.GetString("Login");
            var userid = _db.Users.FirstOrDefault(m => m.Email == user_session).UserId;

            ViewBag.username = _db.Users.FirstOrDefault(m => m.Email == user_session).FirstName + " " + _db.Users.FirstOrDefault(m => m.Email == user_session).LastName;

            bool flag = true;
            if (!ModelState.IsValid)
            {
                var nameValidationState = ModelState.GetFieldValidationState("OldPassword");
                if (nameValidationState == ModelValidationState.Invalid)
                {
                    flag = false;
                }
                var nameValidationState1 = ModelState.GetFieldValidationState("NewPassword");
                if (nameValidationState1 == ModelValidationState.Invalid)
                {
                    flag = false;
                }
                var nameValidationState2 = ModelState.GetFieldValidationState("ConfirmNewPassword");
                if (nameValidationState2 == ModelValidationState.Invalid)
                {
                    flag = false;
                }

            }
            if (flag)
            {
                var exsiting = _db.Users.FirstOrDefault(m => m.Email == user_session);
                if (exsiting != null)
                {
                    var encryptpass = _password.EncryptPassword(passdetail.OldPassword);
                    var newencryptpass = _password.EncryptPassword(passdetail.NewPassword);

                    if (exsiting.Password == encryptpass)
                    {
                        if (passdetail.NewPassword == passdetail.ConfirmNewPassword)
                        {

                            exsiting.Password = newencryptpass;
                            _db.Users.Update(exsiting);
                            _db.SaveChanges();
                            TempData["Success"] = "PassWord Change Successfully!";
                        }
                        else
                        {
                            TempData["error"] = "New And Confirm Password Can Not Match!";
                        }
                    }
                    else
                    {
                        TempData["error"] = "OldPassword Is Wrong!";
                    }


                }
                return RedirectToAction("UserProfile", "Profile");
            }
            else
            {
                TempData["error"] = "All The Filed Required!";
                return RedirectToAction("UserProfile", "Profile");
            }

        }

        [HttpPost]
        public IActionResult SaveAvtar(IFormFile files)
        {
            var user_session = HttpContext.Session.GetString("Login");
            var userid = _db.Users.FirstOrDefault(m => m.Email == user_session).UserId;

            ViewBag.username = _db.Users.FirstOrDefault(m => m.Email == user_session).FirstName + " " + _db.Users.FirstOrDefault(m => m.Email == user_session).LastName;

            var user = _db.Users.FirstOrDefault(u => u.UserId == userid);
            var filePaths = new List<string>();
            if (files != null)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/StoryImg", files.FileName);
                filePaths.Add(filePath);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    files.CopyTo(stream);
                }
                user.Avatar = "/StoryImg/" + files.FileName;
                _db.Users.Update(user);
                _db.SaveChanges();
            }
            else
            {
                user.Avatar = "/StoryImg/" + "user1.png";
                _db.Users.Update(user);
                _db.SaveChanges();
            }

            return RedirectToAction("UserProfile", "Profile");

        }


        public IActionResult VolunteeringTimeSheet()
        {
            var user_session = HttpContext.Session.GetString("Login");
            var userid = _db.Users.FirstOrDefault(m => m.Email == user_session).UserId;

            Profile pf = new Profile();
            pf.LoginUser = _db.Users.FirstOrDefault(u => u.UserId == userid);
            pf.MissionList = _mission.GetMissionData().Where(id => id.DeletedAt == null && id.Status == "1").ToList();
            pf.missionApplications = _mission.GetMissionApplications(userid).ToList();
            pf.timesheets = _profile.GetTimeSheetData(userid);
            return View(pf);
        }

        [HttpPost]
        public IActionResult AddTimeBasedData(Profile model)
        {
            var user_session = HttpContext.Session.GetString("Login");
            var userid = _db.Users.FirstOrDefault(m => m.Email == user_session).UserId;

            ViewBag.username = _db.Users.FirstOrDefault(m => m.Email == user_session).FirstName + " " + _db.Users.FirstOrDefault(m => m.Email == user_session).LastName;

            
            if (model.timesheetsId == 0)
            {
                var exist = _profile.GetTimeSheetData(userid).FirstOrDefault(t => t.MissionId == model.MissionId && t.DateVolunteered == model.Date);
                if (exist == null)
                {
                    TimeOnly time = new TimeOnly(model.hours, model.minutes, 00);
                    Timesheet data = new Timesheet();

                    data.MissionId = model.MissionId;
                    data.Time = time;
                    data.DateVolunteered = model.Date;
                    data.Notes = model.message;
                    data.UserId = userid;
                    _db.Timesheets.Add(data);
                    _db.SaveChanges();
                }
                else
                {
                    TempData["error"] = "For this Mission and that day Time Sheet Already Filled!";
                }
            }
            else
            {
                TimeOnly time = new TimeOnly(model.hours, model.minutes, 00);
                var data = _profile.GetTimeSheetData(userid).FirstOrDefault(t => t.TimesheetId == model.timesheetsId);

                data.Time = time;
                data.DateVolunteered = model.Date;
                data.Notes = model.message;
                _db.Timesheets.Update(data);
                _db.SaveChanges();
            }
            

            return RedirectToAction("volunteeringTimeSheet", "Profile");
        }

        [HttpPost]
        public IActionResult AddGoalBasedData(Profile model)
        {
            var user_session = HttpContext.Session.GetString("Login");
            var userid = _db.Users.FirstOrDefault(m => m.Email == user_session).UserId;

            ViewBag.username = _db.Users.FirstOrDefault(m => m.Email == user_session).FirstName + " " + _db.Users.FirstOrDefault(m => m.Email == user_session).LastName;


            if (model.timesheetsId == 0)
            {
                var exist = _profile.GetTimeSheetData(userid).FirstOrDefault(t => t.MissionId == model.MissionId && t.DateVolunteered == model.Date);
                if (exist == null)
                {
                    Timesheet data = new Timesheet();

                    data.MissionId = model.MissionId;
                    data.Action = model.action;
                    data.DateVolunteered = model.Date;
                    data.Notes = model.message;
                    data.UserId = userid;
                    _db.Timesheets.Add(data);
                    _db.SaveChanges();
                }
                else
                {
                    TempData["error"] = "For this Mission and that day Time Sheet Already Filled!";
                }
            }
            else
            {
                var data = _profile.GetTimeSheetData(userid).FirstOrDefault(t => t.TimesheetId == model.timesheetsId);

                data.Action = model.action;
                data.DateVolunteered = model.Date;
                data.Notes = model.message;
                _db.Timesheets.Update(data);
                _db.SaveChanges();
            }


            return RedirectToAction("volunteeringTimeSheet", "Profile");
        }


        public IActionResult GetTimeSheetData(long id)
        {
            var user_session = HttpContext.Session.GetString("Login");
            var userid = _db.Users.FirstOrDefault(m => m.Email == user_session).UserId;

            var data = _profile.GetTimeSheetData(userid).FirstOrDefault(t => t.TimesheetId == id);
            if (data != null)
            {
                if (data.Action != null)
                {
                    var detail = new
                    {
                        TimeSheetId = data.TimesheetId,
                        Title = _mission.GetMissionData().FirstOrDefault(m => m.MissionId == data.MissionId).Title,
                        Type = _mission.GetMissionData().FirstOrDefault(m => m.MissionId == data.MissionId).MissionType,
                        Action = data.Action,
                        date = data.DateVolunteered,
                        message = data.Notes
                    };
                    return new JsonResult(detail);
                }
                else
                {
                    TimeOnly timeSpan = (TimeOnly)data.Time;
                    var detail = new
                    {
                        TimeSheetId = data.TimesheetId,
                        Title = _mission.GetMissionData().FirstOrDefault(m => m.MissionId == data.MissionId).Title,
                        Type = _mission.GetMissionData().FirstOrDefault(m => m.MissionId == data.MissionId).MissionType,
                        Hours = timeSpan.Hour,
                        Minutes = timeSpan.Minute,
                        date = data.DateVolunteered,
                        message = data.Notes
                    };
                    return new JsonResult(detail);
                }

            }
            else
            {
                return new JsonResult(new { error = "Data Not Available" });
            }
        }

        [HttpPost]
        public IActionResult Deleteitem(long id)
        {
            var user_session = HttpContext.Session.GetString("Login");
            var userid = _db.Users.FirstOrDefault(m => m.Email == user_session).UserId;

            var data = _profile.GetTimeSheetData(userid).FirstOrDefault(t => t.TimesheetId == id);
            _db.Timesheets.Remove(data);
            _db.SaveChanges();
            return RedirectToAction("volunteeringTimeSheet", "Profile");
        }


        [HttpPost]
        public IActionResult ContactUs(Profile model)
        {

            var contactus = new ContactU
            {
                Email = model.LoginUser.Email,
                Name = model.LoginUser.FirstName,
                Subject = model.ContactUs.Subject,
                Message = model.ContactUs.Message,
            };

            _db.ContactUs.Add(contactus);
            _db.SaveChanges();

            TempData["success"] = "Contect Detail Share Successfully!";
            return RedirectToAction("UserProfile", "Profile");
        }
    }
}
