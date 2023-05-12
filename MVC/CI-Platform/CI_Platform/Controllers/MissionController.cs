using CI_Platform.Entities.Data;
using CI_Platform.Entities.Models;
using CI_Platform.Entities.ViewModel;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CI_Platform.Controllers
{
    public class MissionController : Controller
    {
        private CiPlatformContext _db;
        private ICountryRepository _countryname;
        private ICityRepository _cityname;
        private ISkillRepository _skillname;
        private IThemeRepository _themename;
        private IMissionRepository _mission;
        private IMissiondataRepository _missiondata;
        private IVolunteerRepository _volunteerRepository;
        private IStoryRepository _storyRepository;


        public MissionController(CiPlatformContext db, ICountryRepository countryname, ICityRepository cityname, ISkillRepository skillname,IThemeRepository themename, IMissionRepository mission, IMissiondataRepository missiondata, IVolunteerRepository volunteer, IStoryRepository storyRepository)
        {
            _db = db;
            _countryname = countryname;
            _cityname = cityname;
            _skillname = skillname;
            _themename = themename;
            _mission = mission;
            _missiondata = missiondata;
            _volunteerRepository = volunteer;
            _storyRepository = storyRepository; 
        }

        public IActionResult Landing(string filter,long countryId=0 ,int pg=1)
        {
            Mission_data md = _missiondata.GetMissiondata();
            md.user = _db.Users.FirstOrDefault(m => m.Email == HttpContext.Session.GetString("Login"));
            md.User = _volunteerRepository.Getuser_data();
            md.TotalMissionCount = md.Mission.Count();



            const int pageSize = 3;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = md.Mission.Count();

            var pager = new Mission_data(recsCount,pg,pageSize);

            int recSkip = (pg-1) * pageSize;

            md.Mission = md.Mission.Skip(recSkip).Take(pager.PageSize).ToList();

            this.ViewBag.Pager = pager;


            var session_data = HttpContext.Session.GetString("Login");
            if (session_data == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(md);
            }
        }

        public IActionResult GetCityByCountry(long CountryId)
        {
            var cities = _db.Cities.Where(c => c.CountryId == CountryId).ToList();
            return Json(cities);
        }
        public IActionResult GetCityByCountryAll(long CountryId)
        {
            var cities = _db.Cities.ToList();
            return Json(cities);
        }


        public IActionResult GetAllFilterData(string[] country,  string[] city, string[] skill, string[] theme, string sort,string search , int page,string Explore,string Exploretheme)
        {
            var user_session = HttpContext.Session.GetString("Login");
            var userid = _db.Users.FirstOrDefault(m => m.Email == user_session).UserId;

            ViewBag.username = _db.Users.FirstOrDefault(m => m.Email == user_session).FirstName + " " + _db.Users.FirstOrDefault(m => m.Email == user_session).LastName;

            Mission_data ms = _missiondata.GetMissiondata();
            IEnumerable<Mission> filter = _missiondata.ApplyFilter(country,city,skill,theme,sort,userid,search??"",Explore??"", Exploretheme??"");
            ms.user = _db.Users.FirstOrDefault(m => m.Email == HttpContext.Session.GetString("Login"));
            ms.User = _volunteerRepository.Getuser_data();
            ms.Mission = filter;

            ms.TotalMissionCount = ms.Mission.Count();

            const int pageSize = 3;
            if (page < 1)
            {
                page = 1;
            }
            int recsCount = ms.Mission.Count();

            var pager = new Mission_data(recsCount, page, pageSize);

            int recSkip = (page - 1) * pageSize;

            ms.Mission = ms.Mission.Skip(recSkip).Take(pager.PageSize).ToList();

            this.ViewBag.Pager = pager;

            

            return PartialView("_Missions", ms);

            
        }


        public IActionResult Volunteering(long id)
        {
            var user_session = HttpContext.Session.GetString("Login");
            var Volunteer = GetData(id, user_session);
            
            List<MissionRating> mis = _db.MissionRatings.Where(del => del.DeletedAt == null).ToList();

            float number = 0;
            long sum = 0;
            long count = 0;
            for (int i = 0; i < mis.Count; i++)
            {
                if (mis[i].MissionId == id)
                {
                    sum += mis[i].Rating;
                    number++;
                    count++;
                }
            }

            float avg = sum / number;
            ViewBag.Count = count;
            ViewBag.Avg = avg;
            Console.WriteLine(avg);

            if (HttpContext.Session.GetString("username") != null)
            {
                ViewBag.username = HttpContext.Session.GetString("username");
            }
            if(HttpContext.Session.GetString("Login") != null)
            {
                return View(Volunteer);
            }
            else
            {
                return RedirectToAction("Index","Home");
            }
        }

        public IActionResult GetRating(string rating_detail)
        {
            var parseObject = JObject.Parse(rating_detail);
            int ratingvalue = parseObject.Value<int>("Rating");
            var MId = parseObject.Value<long>("MissionId");
            var loggeduserid = parseObject.Value<long>("UserId");
            


            var recObj = new MissionRating()
            {
                Rating = ratingvalue,
                MissionId = MId,
                UserId = loggeduserid,
            };

            Mission_data ms = new Mission_data();
            ms.RatingCount = ratingvalue;

            var check = _db.MissionRatings.FirstOrDefault(m => m.UserId == recObj.UserId && m.MissionId == recObj.MissionId);

            if (check != null)
            {
                  check.Rating = ratingvalue;
                 _db.MissionRatings.Update(check);
                 _db.SaveChanges();
                
            }
            else
            {
                if (recObj.Rating != 0)
                {
                    _db.MissionRatings.Add(recObj);
                    _db.SaveChanges();
                }
                
            }
            var user_session  = HttpContext.Session.GetString("Login");
            IEnumerable<MissionRating> rating  = _volunteerRepository.Rating_data();
            Mission_data mw = new Mission_data();
            mw.mission_vol = _volunteerRepository.GetLandingMissionData(MId);
            mw.user = _db.Users.FirstOrDefault(m => m.Email == user_session);
            mw.User = _volunteerRepository.Getuser_data();
            mw.RatingCount = ratingvalue;
            return PartialView("_Ratingview", mw);

        }



        public Mission_data GetData(long id,string user_session)
        {
            Mission_data ms = new Mission_data();
            Mission_data mission = new Mission_data();
            ms.Skill = _db.Skills.ToList(); 
            ms.mission_vol = _volunteerRepository.GetLandingMissionData(id);
            ms.user = _db.Users.FirstOrDefault(m => m.Email == user_session);
            ms.User = _volunteerRepository.Getuser_data();
            ms.comments = _volunteerRepository.Getcomment_data(id);
            ms.related_mission = _volunteerRepository.GetRelatedMission(id);
            ms.Related_MissionApplication = _db.MissionApplications;
            ms.MissionApplication = _volunteerRepository.GetRecentVolunteer(id);
            ms.FavoriteMission = _db.FavoriteMissions.ToList();
            ms.MissionRating = _db.MissionRatings.ToList();
            ms.goalMissions = _db.GoalMissions.ToList();
            ms.missionMedia = _db.MissionMedia.ToList();
            ms.timesheetdata = _db.Timesheets.Where(sheets => sheets.DeletedAt == null).ToList();

            int Rating ; 
            if (ms.user.UserId == 0)
            {
                Rating = 0;
            }
            else
            {
                Rating = _volunteerRepository.GetMissionRating_data(id, ms.user.UserId);
            }
            
            if (Rating != 0)
            {
                ms.RatingCount = Rating;
            }
            
            List<Skill> skill = new List<Skill>();

            return ms;
        }

        public void Recommandation(string recommend)
        {
            var parseObject = JObject.Parse(recommend);
            var usermissionid = parseObject.Value<long>("MissionId");
            var UId = parseObject.Value<long>("UserId");
            var loggeduserid = parseObject.Value<long>("FromUserId");
            var Email = parseObject.Value<string>("UserEmail");
             
            var recObj = new MissionInvite()
            {
                MissionId = usermissionid,
                FromUserId = loggeduserid,
                ToUserId = UId,
            };

            if (Email != null)
            {
                _db.MissionInvites.Add(recObj);
                _db.SaveChanges();
                SendmailtoFriends(Email,usermissionid);
            }
        }


        public void SendmailtoFriends(string email,long id)
        {
            
            var passwordresetlink = Url.Action("Volunteering", "Mission", new { id = id }, Request.Scheme);
            
            var link = Url.Action("Index", "Home", new {url = passwordresetlink}, Request.Scheme);
            TempData["link"] = passwordresetlink;
            var emailfrom = new MailAddress("amangandhi0523@gmail.com");
            var frompwd = "ifuempfdxibysfbg";
            var toEmail = new MailAddress(email);

            String body = "Here is Mission link <br>" + link;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(emailfrom.Address, frompwd)
            };

            MailMessage message = new MailMessage(emailfrom, toEmail);
            message.Subject = "Recommandation from co-worker";
            message.Body = body;
            message.IsBodyHtml = true;
            smtp.Send(message);

        }


        public void AddToFavourite(string favObj)
        {
            var parseObj = JObject.Parse(favObj);
            var missionId = parseObj.Value<long>("missionId");
            var userId = parseObj.Value<long>("userId");

            var obj = new FavoriteMission()
            {
                MissionId = missionId,
                UserId = userId,
            };
            var favouritemission = _db.FavoriteMissions.FirstOrDefault(m => m.MissionId == missionId && m.UserId == userId);

            if (favouritemission != null)
            {
                _db.FavoriteMissions.Remove(favouritemission);
                _db.SaveChanges();
            }
            else
            {
                _db.FavoriteMissions.Add(obj);
                _db.SaveChanges();
            }
        }

        

        
        public IActionResult GetComment(string Comment_details)
        {
            var parseObj = JObject.Parse(Comment_details);
            var missionId = parseObj.Value<long>("MissionId");
            var userId = parseObj.Value<long>("UserId");
            var ctext =  parseObj.Value<string>("commenttext");

            var commentObj = new Comment()
            {
                MissionId = missionId,
                UserId = userId,
                Commenttext = ctext,
            };

            _db.Comments.Add(commentObj);
            _db.SaveChanges();

            Mission_data model = new Mission_data();
            model.comments = _volunteerRepository.Getcomment_data(missionId);
            model.User = _db.Users.ToList();
            return PartialView("Commentview", model);

        }

        public void GetApaply(long Apply)
        {
            var user_session = HttpContext.Session.GetString("Login");
            var userid = _db.Users.FirstOrDefault(m => m.Email == user_session).UserId;


            var exsist = _db.MissionApplications.FirstOrDefault(id => id.UserId == userid  && id.MissionId == Apply && id.ApprovalStatus == "PENDING");

            if(exsist != null)
            {
                exsist.DeletedAt = null;
                exsist.ApprovalStatus = "PENDING";

                _db.MissionApplications.Update(exsist);
                _db.SaveChanges();
            }
            else
            {
                var obj = new MissionApplication()
                {
                    MissionId = Apply,
                    UserId = userid,
                    AppliedAt = DateTime.Now,
                    ApprovalStatus = "PENDING",

                };

                _db.MissionApplications.Add(obj);
                _db.SaveChanges();
            }

        }


    }
} 
