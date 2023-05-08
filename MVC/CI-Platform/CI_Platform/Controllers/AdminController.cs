using CI_Platform.Entities.Data;
using CI_Platform.Entities.Models;
using CI_Platform.Entities.ViewModel;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform.Controllers
{
    public class AdminController : Controller
    {
        private readonly CiPlatformContext _db;
        private ICountryRepository _country;
        private ICityRepository _city;
        private ISkillRepository _skill;
        private IMissionRepository _mission;
        private IThemeRepository _theme;
        private IAdminRepository _admin;
        private IPasswordRepository _password;

        public AdminController(CiPlatformContext db, IPasswordRepository password , ICountryRepository country, ICityRepository city, ISkillRepository skill, IMissionRepository mission, IThemeRepository theme, IAdminRepository admin)
        {
            _db = db;
            _country = country;
            _city = city;
            _skill = skill;
            _mission = mission;
            _theme = theme;
            _admin = admin;
            _password = password;
        }


        /*-----------------------------------------------------------Main Admin Page View-----------------------------------------------------------*/

        public IActionResult AdminMission()
        {
            var user_session = HttpContext.Session.GetString("Login");
            var userid = _db.Users.FirstOrDefault(m => m.Email == user_session).UserId;

            


            Admintables admintables = new Admintables();
            admintables.MissionList = _mission.GetMissionData();
            admintables.UserList = _admin.GetUserData();
            admintables.missionApplications = _mission.GetMissionApplications();
            admintables.SkillList = _skill.GetSkillData();
            admintables.MissionThemeList = _theme.GetThemeData();
            admintables.CmsPageList = _admin.CMSdata().Where(cms => cms.DeletedAt == null);
            admintables.StoryList = _admin.StoryData();
            admintables.CountriesList = _country.GetCountryData();
            admintables.CityList = _city.GetCityData();
            admintables.BannerList = _admin.Bannerdata();
            admintables.user = _db.Users.FirstOrDefault(id => id.UserId == userid);
            
            return View(admintables);
        }




        /*-----------------------------------------------------------User Pages-----------------------------------------------------------*/

        [HttpPost]
        public IActionResult AddUserData(Admintables model)
        {
            var data = _db.Users.FirstOrDefault(user => user.Email == model.Email);
            var data2 = _db.Users.FirstOrDefault(users => users.EmployeeId == model.EmployeeId);


            if (model.UserId == 0)
            {
                if (data == null && data2 == null)
                {
                    var passwordgenrate = model.FirstName + "@abc123";
                    var encryptedPassword = _password.EncryptPassword(passwordgenrate);

                    User user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        Status = model.Status,
                        Department = model.DepartmentName,
                        CountryId = model.CountryId,
                        CityId = model.CityID,
                        EmployeeId = model.EmployeeId,
                        Password = encryptedPassword,
                        Avatar = "/StoryImg/" + "user1.png",
                    };

                    _db.Users.Add(user);
                    _db.SaveChanges();
                }
                else
                {
                    TempData["error"] = "This Email Id Already Registered";
                    return null;
                }
            }
            else
            {
                var useriddata = _db.Users.FirstOrDefault(user => user.UserId == model.UserId);

                useriddata.FirstName = model.FirstName;
                useriddata.LastName = model.LastName;
                useriddata.Email = model.Email;
                useriddata.PhoneNumber = model.PhoneNumber;
                useriddata.Status = model.Status;
                useriddata.Department = model.DepartmentName;
                useriddata.CountryId = model.CountryId;
                useriddata.CityId = model.CityID;
                useriddata.EmployeeId = model.EmployeeId;
                

                _db.Users.Update(useriddata);
                _db.SaveChanges();
            }


            Admintables admintables = new Admintables();
            admintables.UserList = _admin.GetUserData();
            admintables.CountriesList = _country.GetCountryData();
            admintables.CityList = _city.GetCityData();

            return PartialView("_AdminUser", admintables);

        }


        public IActionResult FillUserData(long userid)
        {
            var user = _admin.GetUserData().FirstOrDefault(u => u.UserId == userid);
            var city = _db.Cities.Where(s => s.CountryId == user.CountryId).ToList();


            if (user != null)
            {
                var data = new Admintables
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    EmployeeId = user.EmployeeId,
                    Status = user.Status,
                    Department = user.Department,
                    CityID = user.CityId,
                    CountryId = user.CountryId,
                    PhoneNumber = user.PhoneNumber,
                    UserId = user.UserId,
                    CityList = city
                };
                
                data.UserList = _admin.GetUserData();
                data.CountriesList = _country.GetCountryData();

                return PartialView("_AddEditUser", data);
            }

            Admintables admintables = new Admintables();
            admintables.UserList = _admin.GetUserData();
            admintables.CountriesList = _country.GetCountryData();
            admintables.CityList = _city.GetCityData();

            return PartialView("_AddEditUser", admintables);

        }

        public IActionResult PartialViewForAdminUser()
        {
            Admintables admintables = new Admintables();
            admintables.UserList = _admin.GetUserData();
            admintables.CountriesList = _country.GetCountryData();
            admintables.CityList = _city.GetCityData();

            return PartialView("_AdminUser", admintables);
        }

        public IActionResult PartialViewForAddeditUser()
        {
            Admintables admintables = new Admintables();
            admintables.CountriesList = _country.GetCountryData();
            admintables.CityList = _city.GetCityData();

            return PartialView("_AddEditUser", admintables);

        }

        /*-----------------------------------------------------------MissionApplication Pages-----------------------------------------------------------*/
        /*MissionApplication Status Approve-Decline*/
        public IActionResult MissionApplicationStatusCheck(string status, long id)
        {
            var data = _db.MissionApplications.FirstOrDefault(ids => ids.MissionApplicationId == id);

            data.ApprovalStatus = status;
            _db.MissionApplications.Update(data);
            _db.SaveChanges();


            Admintables admintables = new Admintables();
            admintables.missionApplications = _mission.GetMissionApplications();
            admintables.MissionList = _mission.GetMissionData();
            admintables.UserList = _admin.GetUserData();

            return PartialView("_AdminMissionApplication", admintables);
        }


        /*-----------------------------------------------------------Skill Pages-----------------------------------------------------------*/
        /*Skill Data Add*/
        [HttpPost]
        public IActionResult SkillDataAdd(string SkillName)
        {

            if (SkillName != null)
            {
                int statusvalue = 0;
                var skillexsist = _skill.GetSkillData().FirstOrDefault(skillname => skillname.SkillName == SkillName);
                if (skillexsist == null)
                {
                    var skilldata = new Skill();
                    skilldata.SkillName = SkillName;
                    skilldata.Status = (byte)statusvalue;

                    _db.Skills.Add(skilldata);
                    _db.SaveChanges();
                }
                else
                {
                    TempData["error"] = "This Skill Already Exsist In Data Base!";
                }

            }

            Admintables admintables = new Admintables();
            admintables.SkillList = _skill.GetSkillData();

            return PartialView("_AdminSkills", admintables);
        }

        /*Skill Status Check*/
        public IActionResult SkillStatusCheck(int status, long id)
        {
            var data = _db.Skills.FirstOrDefault(ids => ids.SkillId == id);
            if(status == 1)
            {
                data.Status = (byte)status;
                data.DeletedAt = null;
                _db.Skills.Update(data);


                var userskilldata = _db.UserSkills.Where(ids => ids.SkillId == id);
                if (userskilldata.Any())
                {
                    foreach(var user in userskilldata)
                    {
                        user.DeletedAt = null;
                        _db.UserSkills.Update(user);
                    }
                }

                var MissionSkilldata = _db.MissionSkills.Where(ids => ids.SkillId == id);
                if (MissionSkilldata != null)
                {
                    foreach (var skill in MissionSkilldata)
                    {
                        skill.DeletedAt = null;
                        _db.MissionSkills.Update(skill);
                    }
                }
                _db.SaveChanges();
            }
            else
            {
                data.Status = (byte)status;
                data.DeletedAt = DateTime.Now;
                _db.Skills.Update(data);

                var userskilldata = _db.UserSkills.Where(ids => ids.SkillId == id);
                if (userskilldata.Any())
                {
                    foreach (var user in userskilldata)
                    {
                        user.DeletedAt = DateTime.Now;
                        _db.UserSkills.Update(user);
                    }
                }

                var MissionSkilldata = _db.MissionSkills.Where(ids => ids.SkillId == id);
                if (MissionSkilldata != null)
                {
                    foreach (var skill in MissionSkilldata)
                    {
                        skill.DeletedAt = DateTime.Now;
                        _db.MissionSkills.Update(skill);
                    }
                }
                _db.SaveChanges();
            }
            

            Admintables admintables = new Admintables();
            admintables.SkillList = _skill.GetSkillData();

            return PartialView("_AdminSkills", admintables);
        }



        /*-----------------------------------------------------------Theme Page-----------------------------------------------------------*/
        [HttpPost]
        public IActionResult ThemeDataAdd(string ThemeName)
        {

            if (ThemeName != null)
            {
                int statusvalue = 0;
                var Themeexsist = _theme.GetThemeData().FirstOrDefault(themename => themename.Title == ThemeName);
                if (Themeexsist == null)
                {
                    var themedata = new MissionTheme();
                    themedata.Title = ThemeName;
                    themedata.Status = (byte)statusvalue;

                    _db.MissionThemes.Add(themedata);
                    _db.SaveChanges();
                }

            }

            Admintables admintables = new Admintables();
            admintables.MissionThemeList = _theme.GetThemeData();

            return PartialView("_Admintheme", admintables);
        }


        public IActionResult ThemeStatusCheck(int status, long id)
        {
            var data = _theme.GetThemeData().FirstOrDefault(ids => ids.MissionThemeId == id);


            if(status == 1)
            {
                data.Status = (byte)status;
                data.DeletedAt = null;
                _db.MissionThemes.Update(data);
                _db.SaveChanges();
            }
            else
            {
                data.Status = (byte)status;
                data.DeletedAt = DateTime.Now;
                _db.MissionThemes.Update(data);
                _db.SaveChanges();
            }
            

            Admintables admintables = new Admintables();
            admintables.MissionThemeList = _theme.GetThemeData();

            return PartialView("_Admintheme", admintables);
        }



        /*-----------------------------------------------------------Story Status Check-----------------------------------------------------------*/

        public IActionResult StoryStatusCheck(string status, long StoryId)
        {
            var StoryData = _admin.StoryData().FirstOrDefault(storyids => storyids.StoryId == StoryId);
            StoryData.Status = status;
            _db.Stories.Update(StoryData);
            _db.SaveChanges();

            Admintables admintables = new Admintables();
            admintables.StoryList = _admin.StoryData();
            admintables.MissionList = _mission.GetMissionData();
            admintables.UserList = _admin.GetUserData();

            return PartialView("_AdminStory", admintables);
        }

        public IActionResult StoryDetailPage(long Storyid)
        {
            Admintables admintables = new Admintables();
            admintables.StoryDetail = _admin.StoryData().FirstOrDefault(id => id.StoryId == Storyid);
            admintables.MissionList = _mission.GetMissionData();
            admintables.UserList = _admin.GetUserData();
            admintables.storyMediaList = _admin.GetStoryMedia().Where(id => id.StoryId == Storyid).ToList();

            return PartialView("_StoryDetail",admintables);
        }


        /*-----------------------------------------------------------Deleted Items For All-----------------------------------------------------------*/
        public IActionResult DeletedItems(long Itemid, string Table)
        {
            if (Table == "Story")
            {
                var StoryData = _admin.StoryData().FirstOrDefault(storyids => storyids.StoryId == Itemid);
                StoryData.DeletedAt = DateTime.Now;
                _db.Stories.Update(StoryData);
                _db.SaveChanges();


                Admintables admintables = new Admintables();
                admintables.StoryList = _admin.StoryData();
                admintables.MissionList = _mission.GetMissionData();
                admintables.UserList = _admin.GetUserData();
                admintables.TableName = Table;

                return PartialView("_AdminStory", admintables);
            }
            else if (Table == "User")
            {
                var UserData = _admin.GetUserData().FirstOrDefault(user => user.UserId == Itemid);
                UserData.DeletedAt = DateTime.Now;
                UserData.Status = "0";
                var userid = UserData.UserId;
                _db.Users.Update(UserData);

                var Commentdata = _db.Comments.Where(id => id.UserId == userid);
                var FavMissiondata = _db.FavoriteMissions.Where(id => id.UserId == userid);
                var MissionApplicationData = _db.MissionApplications.Where(id => id.UserId == userid);
                var missionInvite = _db.MissionInvites.Where(id => id.FromUserId == userid);
                var MissionRating = _db.MissionRatings.Where(id => id.UserId == userid);
                var Story = _db.Stories.Where(id => id.UserId == userid);
                var Storyinvite = _db.StoryInvites.Where(id => id.FromUserId == userid);
                var Timesheets = _db.Timesheets.Where(id => id.UserId == userid);
                var Userskill = _db.UserSkills.Where(id => id.UserId == userid);



                if(Commentdata != null)
                {
                    foreach(var comment in Commentdata)
                    {
                        comment.DeletedAt = DateTime.Now;
                        _db.Comments.Update(comment);
                    }
                }

                if(MissionApplicationData != null)
                {
                    foreach (var mission in MissionApplicationData)
                    {
                        mission.DeletedAt = DateTime.Now;
                        _db.MissionApplications.Update(mission);
                    }
                }

                if(FavMissiondata != null)
                {
                    foreach(var fav in FavMissiondata)
                    {
                        fav.DeletedAt = DateTime.Now;   
                        _db.FavoriteMissions.Update(fav);
                    }
                }

                if(missionInvite != null)
                {
                    foreach(var invite in missionInvite)
                    {
                        invite.DeletedAt = DateTime.Now;
                        _db.MissionInvites.Update(invite);
                    }
                }

                if(MissionRating != null)
                {
                    foreach(var rating in MissionRating)
                    {
                        rating.DeletedAt = DateTime.Now;    
                        _db.MissionRatings.Update(rating);
                    }
                }

                if(Story != null)
                {
                    foreach(var story in Story)
                    {
                        story.DeletedAt = DateTime.Now;
                        _db.Stories.Update(story);
                    }
                }

                if(Storyinvite != null)
                {
                    foreach(var storyinvite in Storyinvite)
                    {
                        storyinvite.DeletedAt= DateTime.Now;

                        _db.StoryInvites.Update(storyinvite);
                    }
                }

                if(Timesheets != null)
                {
                    foreach( var timesheet in Timesheets)
                    {
                        timesheet.DeletedAt = DateTime.Now; 
                        _db.Timesheets.Update(timesheet);
                    }
                }

                if(Userskill != null)
                {
                    foreach(var userkill in Userskill)
                    {
                        userkill.DeletedAt = DateTime.Now;
                        _db.UserSkills.Update(userkill);
                    }
                }

                _db.SaveChanges();

                Admintables admintables = new Admintables();
                admintables.UserList = _admin.GetUserData().Where(user => user.DeletedAt == null);
                admintables.CountriesList = _country.GetCountryData();
                admintables.CityList = _city.GetCityData();
                admintables.TableName = Table;

                return PartialView("_AdminUser", admintables);
            }
            else if (Table == "CMS")
            {
                var Cmsdata = _admin.CMSdata().FirstOrDefault(cmsid => cmsid.CmsPageId == Itemid);
                Cmsdata.DeletedAt = DateTime.Now;

                _db.CmsPages.Update(Cmsdata);
                _db.SaveChanges();

                Admintables admintables = new Admintables();
                admintables.CmsPageList = _admin.CMSdata().Where(user => user.DeletedAt == null);

                return PartialView("_AdminCMS", admintables);
            }
            else if (Table == "Mission")
            {
                var MissionData = _mission.GetMissionData().FirstOrDefault(mission => mission.MissionId == Itemid);
                MissionData.DeletedAt = DateTime.Now;
                MissionData.Status = "0";

                _db.Missions.Update(MissionData);

                /*Now We Deleted All The Missions Ref From Other Table Also*/
                CommentDel(Itemid);
                FavMissionDel(Itemid);
                GoalMissionDel(Itemid);
                MissionApplicationDel(Itemid);
                MissionDocDel(Itemid);
                MissionInvDel(Itemid);
                MissionmediaDel(Itemid);
                MissionRatingDel(Itemid);
                MissionSkillsDel(Itemid);
                MissionStoryDel(Itemid);
                MissionTimeSheetDel(Itemid);


                /*After Changes We Save In Data Base */
                _db.SaveChanges();


                Admintables admintables = new Admintables();
                admintables.MissionList = _mission.GetMissionData().Where(user => user.DeletedAt == null);

                return PartialView("_AdminMission", admintables);
            }
            else if (Table == "Banner")
            {
                var bannerdata = _admin.Bannerdata().FirstOrDefault(id => id.BannerId == Itemid);
                bannerdata.DeletedAt = DateTime.Now;
                
                _db.Banners.Update(bannerdata);
                _db.SaveChanges();

                Admintables admintables = new Admintables();
                admintables.BannerList = _admin.Bannerdata().Where(user => user.DeletedAt == null).ToList();

                return PartialView("_Adminbanner", admintables);
            }

            return RedirectToAction("AdminMission", "Admin");
        }


        /*-----------------------------------------------------------Mission Ref Table DeleteAt Time And Status Update-----------------------------------------------------------*/


        public void CommentDel(long id)
        {
            var Commentdata = _db.Comments.Where(ids => ids.MissionId == id);
            foreach (var comment in Commentdata)
            {
                comment.DeletedAt = DateTime.Now;
                _db.Comments.Update(comment);
            }
        }

        public void FavMissionDel(long ids)
        {
            var FavMissiondata = _db.FavoriteMissions.Where(id => id.MissionId == ids);
            if (FavMissiondata != null)
            {
                foreach (var fav in FavMissiondata)
                {
                    fav.DeletedAt = DateTime.Now;
                    _db.FavoriteMissions.Update(fav);
                }
            }
        }

        public void GoalMissionDel(long ids)
        {
            var Missiongoaldata = _admin.goalMissions().Where(id => id.MissionId == ids);
            if(Missiongoaldata != null)
            {
                foreach(var goal in Missiongoaldata)
                {
                    goal.DeletedAt = DateTime.Now;
                    _db.GoalMissions.Update(goal);
                }
            }
        }

        public void MissionApplicationDel(long ids)
        {
            var MissionApplicationData = _db.MissionApplications.Where(id => id.UserId == ids);
            if (MissionApplicationData != null)
            {
                foreach (var mission in MissionApplicationData)
                {
                    mission.DeletedAt = DateTime.Now;
                    _db.MissionApplications.Update(mission);
                }
            }
        }

        public void MissionDocDel(long ids)
        {
            var Missiondocdata = _admin.GetMissionDocuments().Where(id => id.MissionId == ids);
            if(Missiondocdata != null)
            {
                foreach( var mission in Missiondocdata)
                {
                    mission.DeletedAt = DateTime.Now;
                    _db.MissionDocuments.Update(mission);
                }
            }
        }

        public void MissionInvDel(long ids)
        {
            var missionInvite = _db.MissionInvites.Where(id => id.FromUserId == ids);
            if (missionInvite != null)
            {
                foreach (var invite in missionInvite)
                {
                    invite.DeletedAt = DateTime.Now;
                    _db.MissionInvites.Update(invite);
                }
            }

        }

        public void MissionmediaDel(long ids)
        {
            var Missionmedia = _db.MissionMedia.Where(id => id.MissionId == ids);
            if(Missionmedia != null)
            {
                foreach(var media in Missionmedia)
                {
                    media.DeletedAt = DateTime.Now;
                    _db.MissionMedia.Update(media);
                }
            }
        }

        public void MissionRatingDel(long ids)
        {
            var MissionRating = _db.MissionRatings.Where(id => id.UserId == ids);
            if (MissionRating != null)
            {
                foreach (var rating in MissionRating)
                {
                    rating.DeletedAt = DateTime.Now;
                    _db.MissionRatings.Update(rating);
                }
            }
        }

        public void MissionSkillsDel(long ids)
        {
            var MissionSkilldata = _db.MissionSkills.Where(id => id.MissionId == ids);
            if(MissionSkilldata != null)
            {
                foreach( var skill in MissionSkilldata)
                {
                    skill.DeletedAt = DateTime.Now;
                    _db.MissionSkills.Update(skill);
                }
            }
        }

        public void MissionStoryDel(long ids)
        {
            var Story = _db.Stories.Where(id => id.UserId == ids);
            if (Story != null)
            {
                foreach (var story in Story)
                {
                    story.DeletedAt = DateTime.Now;
                    _db.Stories.Update(story);
                }
            }
        }

        public void MissionTimeSheetDel(long ids)
        {
            var Timesheets = _db.Timesheets.Where(id => id.UserId == ids);
            if (Timesheets != null)
            {
                foreach (var timesheet in Timesheets)
                {
                    timesheet.DeletedAt = DateTime.Now;
                    _db.Timesheets.Update(timesheet);
                }
            }
        }





        /*-----------------------------------------------------------CMS Page-----------------------------------------------------------*/

        [HttpPost]
        public IActionResult PartialViewForCms(long? id)
        {
            if (id == null)
            {
                Admintables admintables = new Admintables();
                admintables.CmsPageList = _admin.CMSdata();

                return PartialView("_AddEditCMS", admintables);
            }
            else
            {
                var data = _admin.CMSdata().FirstOrDefault(cms => cms.CmsPageId == id && cms.DeletedAt == null);
                if (data != null)
                {
                    var cms = new Admintables
                    {
                        CmsPageId = data.CmsPageId,
                        Title = data.Title,
                        Description = data.Description,
                        Slug = data.Slug,
                        Status = data.Status,
                    };
                    return PartialView("_AddEditCMS", cms);
                }
                else
                {
                    return PartialView("_AddEditCMS");
                }

            }

        }


        [HttpPost]
        public IActionResult AddEditCmsData(string title, string discription, string slug, string status, long cmsId)
        {
            if (cmsId == 0)
            {
                var slugvalue = _admin.CMSdata().FirstOrDefault(slugcheck => slugcheck.Slug == slug);
                if (slugvalue == null)
                {
                    var data = new CmsPage
                    {
                        Title = title,
                        Description = discription,
                        Slug = slug,
                        Status = status
                    };

                    _db.CmsPages.Add(data);
                    _db.SaveChanges();
                }
                else
                {
                    return null;
                }

            }
            else
            {
                var cmsdata = _admin.CMSdata().FirstOrDefault(cmsid => cmsid.CmsPageId == cmsId);
                cmsdata.Title = title;
                cmsdata.Description = discription;
                cmsdata.Slug = slug;
                cmsdata.Status = status;

                _db.CmsPages.Update(cmsdata);
                _db.SaveChanges();

                TempData["Success"] = "CMS Data Change Successfully!";
            }


            Admintables admintables = new Admintables();
            admintables.CmsPageList = _admin.CMSdata().Where(user => user.DeletedAt == null);

            return PartialView("_AdminCMS", admintables);
        }

        public IActionResult PartialViewForAdminCms()
        {
            Admintables admintables = new Admintables();
            admintables.CmsPageList = _admin.CMSdata().Where(user => user.DeletedAt == null);

            return PartialView("_AdminCMS", admintables);
        }





        /*-----------------------------------------------------------Mission Page-----------------------------------------------------------*/

        public IActionResult PartialViewForMission()
        {
            Admintables admintables = new Admintables();
            admintables.MissionList = _mission.GetMissionData();
            admintables.CountriesList = _country.GetCountryData();
            admintables.CityList = _city.GetCityData();
            admintables.SkillList = _skill.GetSkillData();
            admintables.MissionThemeList = _theme.GetThemeData();

            return PartialView("_AddEditMission", admintables);
        }

        public IActionResult PartialViewForAdminMission()
        {
            Admintables admintables = new Admintables();
            admintables.MissionList = _mission.GetMissionData();
            admintables.CountriesList = _country.GetCountryData();
            admintables.CityList = _city.GetCityData();
            admintables.SkillList = _skill.GetSkillData();
            admintables.MissionThemeList = _theme.GetThemeData();

            return PartialView("_AdminMission", admintables);
        }




        /*******************************************************************Add And Edit Mission Data*******************************************************************/

        public IActionResult AddEditMissionData(Admintables model)
        {
            var arr = model.SkillIdArr?.Split(",");

            var Img = model.Imagedraft?.Split(",");

            var urlarr = model.VideoUrlString?.Split(",");


            if (model.MissionId == 0)
            {

                if (model.MissionType == "TIME")
                {
                    var Missiondata = new Mission
                    {
                        Title = model.Title,
                        ShortDescription = model.ShortDescription,
                        Description = model.Description,
                        OrganizationName = model.OrganizationName,
                        OrganizationDetail = model.OrganizationDetail,
                        Status = model.Status,
                        MissionType = model.MissionType,
                        CountryId = model.CountryId,
                        CityId = model.CityID,
                        ThemeId = model.ThemeId,
                        StartDate = model.StartDate,
                        EndDate = model.EndDate,
                        TotalSheet = model.TotalSheet,
                        Deadline = model.deadLine,
                        Availability = model.Availability,
                    };
                    _db.Missions.Add(Missiondata);
                    _db.SaveChanges();

                    if (urlarr != null && urlarr.Length > 0)
                    {
                        for (int i = 0; i < urlarr.Length; i++)
                        {
                            if (urlarr[i] != "")
                            {
                                var MissionMidea = new MissionMedium
                                {

                                    MissionId = Missiondata.MissionId,
                                    MediaPath = urlarr[i],
                                    MediaType = "URL",
                                };
                                _db.MissionMedia.Add(MissionMidea);
                            }
                        }
                    }

                    if (arr != null && arr.Length > 0)
                    {
                        for (int i = 0; i < arr.Length; i++)
                        {
                            var MissionSKills = new MissionSkill
                            {

                                MissionId = Missiondata.MissionId,
                                SkillId = int.Parse(arr[i]),
                            };
                            _db.MissionSkills.Add(MissionSKills);
                        }
                    }


                    if (model.Images != null)
                    {
                        // for file save in folder
                        var filePaths = new List<string>();
                        foreach (var formFile in model.Images)
                        {
                            MissionMedium mediaobj = new MissionMedium();

                            if (formFile.Length > 0)
                            {
                                // full path to file in temp location
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/MissionImg", formFile.FileName);
                                filePaths.Add(filePath);
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    formFile.CopyTo(stream);
                                }
                            }

                            mediaobj.MissionId = Missiondata.MissionId;
                            mediaobj.MediaPath = "/MissionImg/" + formFile.FileName;
                            mediaobj.MediaType = "PNG";
                            _db.MissionMedia.Add(mediaobj);

                        }


                    }

                    if (model.Document != null)
                    {
                        // for file save in folder
                        var filePaths = new List<string>();
                        foreach (var formFile in model.Document)
                        {
                            MissionDocument mediaobj = new MissionDocument();

                            if (formFile.Length > 0)
                            {
                                // full path to file in temp location
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Document", formFile.FileName);
                                filePaths.Add(filePath);
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    formFile.CopyTo(stream);
                                }
                            }

                            mediaobj.MissionId = Missiondata.MissionId;
                            mediaobj.DocumentPath = "/Document/" + formFile.FileName;
                            mediaobj.DocumentType = formFile.ContentType;
                            mediaobj.DocumentName = formFile.FileName;
                            _db.MissionDocuments.Add(mediaobj);

                        }
                    }



                    _db.SaveChanges();

                }
                else
                {
                    var Missiondata = new Mission
                    {
                        Title = model.Title,
                        ShortDescription = model.ShortDescription,
                        Description = model.Description,
                        OrganizationName = model.OrganizationName,
                        OrganizationDetail = model.OrganizationDetail,
                        Status = model.Status,
                        MissionType = model.MissionType,
                        CountryId = model.CountryId,
                        CityId = model.CityID,
                        ThemeId = model.ThemeId,
                        StartDate = model.StartDate,
                        EndDate = model.EndDate,
                        Availability = model.Availability,
                    };
                    _db.Missions.Add(Missiondata);
                    _db.SaveChanges();



                    if (urlarr != null && urlarr.Length > 0)
                    {
                        for (int i = 0; i < urlarr.Length; i++)
                        {
                            if (urlarr[i] != "")
                            {
                                var MissionMidea = new MissionMedium
                                {

                                    MissionId = Missiondata.MissionId,
                                    MediaPath = urlarr[i],
                                    MediaType = "URL",
                                };
                                _db.MissionMedia.Add(MissionMidea);
                            }

                        }
                    }

                    if (arr != null && arr.Length > 0)
                    {
                        for (int i = 0; i < arr.Length; i++)
                        {
                            var MissionSKills = new MissionSkill
                            {

                                MissionId = Missiondata.MissionId,
                                SkillId = int.Parse(arr[i]),
                            };
                            _db.MissionSkills.Add(MissionSKills);
                        }
                    }

                    var GoalMissionData = new GoalMission
                    {
                        MissionId = Missiondata.MissionId,
                        GoalValue = model.GoalValue,
                        GoalObjectiveText = model.GoalObjectiveText,
                    };
                    _db.GoalMissions.Add(GoalMissionData);




                    if (model.Images != null)
                    {
                        // for file save in folder
                        var filePaths = new List<string>();
                        foreach (var formFile in model.Images)
                        {
                            MissionMedium mediaobj = new MissionMedium();

                            if (formFile.Length > 0)
                            {
                                // full path to file in temp location
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/MissionImg", formFile.FileName);
                                filePaths.Add(filePath);
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    formFile.CopyTo(stream);
                                }
                            }

                            mediaobj.MissionId = Missiondata.MissionId;
                            mediaobj.MediaPath = "/MissionImg/" + formFile.FileName;
                            mediaobj.MediaType = "PNG";
                            _db.MissionMedia.Add(mediaobj);

                        }
                    }



                    if (model.Document != null)
                    {
                        // for file save in folder
                        var filePaths = new List<string>();
                        foreach (var formFile in model.Document)
                        {
                            MissionDocument mediaobj = new MissionDocument();

                            if (formFile.Length > 0)
                            {
                                // full path to file in temp location
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Document", formFile.FileName);
                                filePaths.Add(filePath);
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    formFile.CopyTo(stream);
                                }
                            }

                            mediaobj.MissionId = Missiondata.MissionId;
                            mediaobj.DocumentPath = "/Document/" + formFile.FileName;
                            mediaobj.DocumentType = formFile.ContentType;
                            mediaobj.DocumentName = formFile.FileName;
                            _db.MissionDocuments.Add(mediaobj);

                        }
                    }




                    _db.SaveChanges();

                }


                Admintables admintables = new Admintables();
                admintables.MissionList = _mission.GetMissionData();
                admintables.missionApplications = _mission.GetMissionApplications();
                admintables.SkillList = _skill.GetSkillData();
                admintables.MissionThemeList = _theme.GetThemeData();
                admintables.CountriesList = _country.GetCountryData();
                admintables.CityList = _city.GetCityData();

                return PartialView("_AdminMission", admintables);
            }
            else
            {



                var data = _mission.GetMissionData().FirstOrDefault(missionid => missionid.MissionId == model.MissionId);
                if (data != null)
                {
                    if (data.MissionType == "TIME")
                    {
                        data.Title = model.Title;
                        data.ShortDescription = model.ShortDescription;
                        data.Description = model.Description;
                        data.OrganizationName = model.OrganizationName;
                        data.OrganizationDetail = model.OrganizationDetail;
                        data.Status = model.Status;
                        data.MissionType = model.MissionType;
                        data.CountryId = model.CountryId;
                        data.CityId = model.CityID;
                        data.ThemeId = model.ThemeId;
                        data.StartDate = model.StartDate;
                        data.EndDate = model.EndDate;
                        data.TotalSheet = model.TotalSheet;
                        data.Deadline = model.deadLine;
                        data.Availability = model.Availability;
                        _db.Missions.Update(data);
                        _db.SaveChanges();
                    }
                    else
                    {
                        data.Title = model.Title;
                        data.ShortDescription = model.ShortDescription;
                        data.Description = model.Description;
                        data.OrganizationName = model.OrganizationName;
                        data.OrganizationDetail = model.OrganizationDetail;
                        data.Status = model.Status;
                        data.MissionType = model.MissionType;
                        data.CountryId = model.CountryId;
                        data.CityId = model.CityID;
                        data.ThemeId = model.ThemeId;
                        data.StartDate = model.StartDate;
                        data.EndDate = model.EndDate;
                        data.Availability = model.Availability;
                        _db.Missions.Update(data);
                        _db.SaveChanges();



                        var GoalMissionData = _admin.goalMissions().FirstOrDefault(id => id.MissionId == data.MissionId);
                        GoalMissionData.GoalValue = model.GoalValue;
                        GoalMissionData.GoalObjectiveText = model.GoalObjectiveText;
                        _db.GoalMissions.Update(GoalMissionData);
                        _db.SaveChanges();
                    }

                    var urlolddata = _admin.GetMissionMediumData().Where(id => id.MissionId == data.MissionId).ToList();
                    var Skillolddata = _skill.GetMissionSkillData().Where(id => id.MissionId == data.MissionId).ToList();
                    if(urlolddata != null)
                    {
                        _db.MissionMedia.RemoveRange(urlolddata);
                        _db.SaveChanges();

                    }
                    if (Skillolddata != null)
                    {
                        _db.MissionSkills.RemoveRange(Skillolddata);
                        _db.SaveChanges();

                    }

             

                    if (urlarr != null && urlarr.Length > 0)
                    {
                        for (int i = 0; i < urlarr.Length; i++)
                        {
                            if (urlarr[i] != "")
                            {
                                var MissionMidea = new MissionMedium
                                {

                                    MissionId = data.MissionId,
                                    MediaPath = urlarr[i],
                                    MediaType = "URL",
                                };
                                _db.MissionMedia.Add(MissionMidea);
                            }

                        }
                    }

                    if (arr != null && arr.Length > 0)
                    {
                        for (int i = 0; i < arr.Length; i++)
                        {
                            var MissionSKills = new MissionSkill
                            {

                                MissionId = data.MissionId,
                                SkillId = int.Parse(arr[i]),
                            };
                            _db.MissionSkills.Add(MissionSKills);
                        }
                    }

                    if (model.Images != null)
                    {
                        // for file save in folder
                        var filePaths = new List<string>();

                        foreach (var formFile in model.Images)
                        {
                            MissionMedium mediaobj = new MissionMedium();

                            if (formFile.Length > 0)
                            {
                                // full path to file in temp location
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/MissionImg", formFile.FileName);
                                filePaths.Add(filePath);
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    formFile.CopyTo(stream);
                                }
                            }

                            mediaobj.MissionId = data.MissionId;
                            mediaobj.MediaPath = "/MissionImg/" + formFile.FileName;
                            mediaobj.MediaType = "PNG";
                            _db.MissionMedia.Add(mediaobj);

                        }
                    }

                    _db.SaveChanges();

                    if (Img != null && Img.Length > 0)
                    {
                        for (int i = 0; i < Img.Length; i++)
                        {
                            var imagepath = _admin.GetMissionMediumData().FirstOrDefault(path => path.MediaPath == Img[i] && path.MissionId == model.MissionId);

                            if (imagepath == null || imagepath.MediaPath != Img[i])
                            {
                                var MissionMidea = new MissionMedium
                                {

                                    MissionId = data.MissionId,
                                    MediaPath = Img[i],
                                    MediaType = "PNG",
                                };
                                _db.MissionMedia.Add(MissionMidea);
                            }

                        }
                    }
                    _db.SaveChanges();


                    if (model.Document != null)
                    {
                        var documentdetail = _admin.GetMissionDocuments().Where(id => id.MissionId == model.MissionId);
                        _db.MissionDocuments.RemoveRange(documentdetail);

                        // for file save in folder
                        var filePaths = new List<string>();
                        foreach (var formFile in model.Document)
                        {
                            MissionDocument mediaobj = new MissionDocument();

                            if (formFile.Length > 0)
                            {
                                // full path to file in temp location
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Document", formFile.FileName);
                                filePaths.Add(filePath);
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    formFile.CopyTo(stream);
                                }
                            }

                            mediaobj.MissionId = model.MissionId;
                            mediaobj.DocumentPath = "/Document/" + formFile.FileName;
                            mediaobj.DocumentType = formFile.ContentType;
                            mediaobj.DocumentName = formFile.FileName;
                            _db.MissionDocuments.Add(mediaobj);

                        }
                    }

                    _db.SaveChanges();


                }



                Admintables admintables = new Admintables();
                admintables.MissionList = _mission.GetMissionData();
                admintables.missionApplications = _mission.GetMissionApplications();
                admintables.SkillList = _skill.GetSkillData();
                admintables.MissionThemeList = _theme.GetThemeData();
                admintables.CountriesList = _country.GetCountryData();
                admintables.CityList = _city.GetCityData();

                return PartialView("_AdminMission", admintables);
            }


        }


        public IActionResult ImageAndDocumentGet(long MissionId)
        {
            var ImageAndDocumentData = _admin.GetMissionMediumData().Where(missionId => missionId.MissionId == MissionId);
            var Img_details = new List<object>();
            if (ImageAndDocumentData != null)
            {
                var Images = ImageAndDocumentData.Where(type => type.MediaType == "PNG").Select(path => path.MediaPath);
                Img_details.Add(Images);
            }
            return new JsonResult(Img_details);
        }

        public IActionResult GetMissionSkillForAddEditMissionPage(long MissionId)
        {
            var Skills = _skill.GetMissionSkillData().Where(mid => mid.MissionId == MissionId).Select(s => s.SkillId);

            return Json(Skills);
        }


        public IActionResult GetMissionDataForEdit(long MissionId)
        {
            if (MissionId != 0)
            {
                var data = _mission.GetMissionData().FirstOrDefault(missionid => missionid.MissionId == MissionId);
                var sdate = data.StartDate;

                if(data.MissionType == "TIME")
                {
                    var GetData = new Admintables
                    {
                        MissionId = data.MissionId,
                        Title = data.Title,
                        ShortDescription = data.ShortDescription,
                        Description = data.Description,
                        OrganizationName = data.OrganizationName,
                        OrganizationDetail = data.OrganizationDetail,
                        StartDate = (DateTime)data.StartDate,
                        EndDate = (DateTime)data.EndDate,
                        Status = data.Status,
                        MissionType = data.MissionType,
                        Availability = data.Availability,
                        CountryId = data.CountryId,
                        CityID = data.CityId,
                        ThemeId = data.ThemeId,
                        TotalSheet = (long)data.TotalSheet,
                        deadLine = (DateTime)data.Deadline,
                    };
                    GetData.SkillList = _skill.GetSkillData();
                    GetData.MissionThemeList = _theme.GetThemeData();
                    GetData.CountriesList = _country.GetCountryData();
                    GetData.CityList = _city.GetCityData();
                    GetData.MissionMediumList = _admin.GetMissionMediumData().Where(mission => mission.MissionId == MissionId).ToList();
                    GetData.MissionDocumentList = _admin.GetMissionDocuments();
                    return PartialView("_AddEditMission", GetData);
                }
                else
                {
                    var GetData = new Admintables
                    {
                        MissionId = data.MissionId,
                        Title = data.Title,
                        ShortDescription = data.ShortDescription,
                        Description = data.Description,
                        OrganizationName = data.OrganizationName,
                        OrganizationDetail = data.OrganizationDetail,
                        StartDate = (DateTime)data.StartDate,
                        EndDate = (DateTime)data.EndDate,
                        Status = data.Status,
                        MissionType = data.MissionType,
                        Availability = data.Availability,
                        CountryId = data.CountryId,
                        CityID = data.CityId,
                        ThemeId = data.ThemeId,
                        GoalValue = _admin.goalMissions().FirstOrDefault(id => id.MissionId == data.MissionId).GoalValue,
                        GoalObjectiveText = _admin.goalMissions().FirstOrDefault(id => id.MissionId == data.MissionId).GoalObjectiveText,
                    };
                    GetData.SkillList = _skill.GetSkillData();
                    GetData.MissionThemeList = _theme.GetThemeData();
                    GetData.CountriesList = _country.GetCountryData();
                    GetData.CityList = _city.GetCityData();
                    GetData.MissionMediumList = _admin.GetMissionMediumData().Where(mission => mission.MissionId == MissionId).ToList();
                    GetData.MissionDocumentList = _admin.GetMissionDocuments();
                    return PartialView("_AddEditMission", GetData);
                }

                
            }



            return PartialView("_AddEditMission");
        }




        public IActionResult GetBannerData(long BannerId)
        {
            var data = _admin.Bannerdata().FirstOrDefault(id => id.BannerId == BannerId);

            Admintables admintables = new Admintables();
            admintables.Text = data.Text;
            admintables.SortOrder = data.SortOrder;
            admintables.BannerImg = data.Image;
            admintables.BannerId = data.BannerId;

            
            return PartialView("_AddEditBanner", admintables);
        }



        public IActionResult AddEditViewOpen()
        {
            Admintables admintables = new Admintables();
            admintables.BannerList = _admin.Bannerdata();

            return PartialView("_AddEditBanner",admintables);
        }

        public IActionResult ReturnBanner()
        {

            Admintables admintables = new Admintables();
            admintables.BannerList = _admin.Bannerdata();

            return PartialView("_Adminbanner", admintables);
        }

        public IActionResult AddEditBannerData(Admintables model)
        {

            if (model.BannerId == 0)
            {
                var BannerData = new Banner()
                {
                    Text = model.Text,
                    SortOrder = model.SortOrder,
                    Image = "",
                };
                _db.Banners.Add(BannerData);
                _db.SaveChanges();

                


                if (model.BannerImg1 != null)
                {
                    // for file save in folder
                    var filePaths = new List<string>();

                    foreach (var formFile in model.BannerImg1)
                    {
                        Banner mediaobj = _admin.Bannerdata().FirstOrDefault(id => id.BannerId == BannerData.BannerId);

                        if (formFile.Length > 0)
                        {
                            // full path to file in temp location
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/BannerImg", formFile.FileName);
                            filePaths.Add(filePath);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                formFile.CopyTo(stream);
                            }
                        }

                        mediaobj.Image = "/BannerImg/" + formFile.FileName;
                        _db.Banners.Update(mediaobj);
                        _db.SaveChanges();
                    }
                }
            }
            else
            {
                var Olddata = _admin.Bannerdata().FirstOrDefault(bannerid => bannerid.BannerId == model.BannerId);
                Olddata.Text = model.Text;
                Olddata.SortOrder = model.SortOrder;
                Olddata.Image = "";

                _db.Banners.Update(Olddata);
                _db.SaveChanges();



                if (model.BannerImg1 != null)
                {
                    // for file save in folder
                    var filePaths = new List<string>();

                    foreach (var formFile in model.BannerImg1)
                    {
                        var mediaobj = _admin.Bannerdata().FirstOrDefault(id => id.BannerId == Olddata.BannerId);

                        if (formFile.Length > 0)
                        {
                            // full path to file in temp location
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/BannerImg", formFile.FileName);
                            filePaths.Add(filePath);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                formFile.CopyTo(stream);
                            }
                        }

                        mediaobj.Image = "/BannerImg/" + formFile.FileName;
                        _db.Banners.Update(mediaobj);
                        _db.SaveChanges();

                    }
                }
            }



            Admintables admintables = new Admintables();
            admintables.BannerList = _admin.Bannerdata();

            return PartialView("_Adminbanner", admintables);
        }




    }
}
