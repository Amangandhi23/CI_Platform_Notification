using CI_Platform.Entities.Data;
using CI_Platform.Entities.Models;
using CI_Platform.Entities.ViewModel;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace CI_Platform.Controllers
{
    public class StoryController : Controller
    {
        private readonly CiPlatformContext _db;
        private IStoryRepository _story;


        public StoryController(CiPlatformContext db, IStoryRepository story)
        {
            _db = db;
            _story = story;
        }

        public IActionResult Story()
        {
            var user_session = HttpContext.Session.GetString("Login");
            var userid = _db.Users.FirstOrDefault(m => m.Email == user_session).UserId;

            StoryModel model = new StoryModel();
            model.Stories = _story.GetStoryData().OrderBy(m => m.Status);
            model.MissionTheme = _db.MissionThemes;
            model.storymedia = _db.StoryMedia;
            model.UserId = userid;
            model.User = _db.Users;

            return View(model);
        }

        public JsonResult GetStory(long MissionId)
        {
            var user_session = HttpContext.Session.GetString("Login");
            var userid = _db.Users.FirstOrDefault(m => m.Email == user_session).UserId;
            var story = _db.Stories.Where(m => m.MissionId == MissionId && m.UserId == userid).Include(m => m.StoryMedia);
            var story_details = new List<object>();
            if (story != null)
            {
                foreach(var item in story)
                {
                    if(item.Status == "DRAFT")
                    {
                        var media_paths = item.StoryMedia.Where(m => m.Type == "URL").Select(m => m.Path).ToList();
                        var image_path = item.StoryMedia.Where(m => m.Type == "PNG" || m.Type == "JPG" || m.Type == "JPEG").Select(m => m.Path).ToList();

                        var detail = new
                        {
                            Title = item.Title,
                            Description = item.Description,
                            MediaPaths = media_paths,
                            ImagePath = image_path,
                            Date = item.Date
                        };
                        story_details.Add(detail);
                    }
                }
                return new JsonResult(story_details);
            }
            else
            {
                return Json("Story not availble");
            }
            
        }

        public IActionResult Storylisting(long? storyid)
        {
            var user_session = HttpContext.Session.GetString("Login");
            var userid = _db.Users.FirstOrDefault(m => m.Email == user_session).UserId;

            StoryModel sm = new StoryModel();
            sm.Stories = _story.GetStoryData();
            sm.MissionApplication = _db.MissionApplications.Include(m => m.Mission).ToList();
            sm.StoryMedium = _db.StoryMedia.ToList();
            sm.UserId = userid;
            sm.User = _db.Users;
            if(storyid != null)
            {
                Story story = _story.GetStoryData().FirstOrDefault(m => m.StoryId == storyid);
                var storymedia = _db.StoryMedia.Where(m => m.StoryId == storyid).ToList();
                sm.storymedia = storymedia;
                sm.Storydata = story;
            }
            

            return View(sm);
        }


        [HttpPost]
        public IActionResult Storylisting(StoryModel storymodel, List<IFormFile> files,string url)
        {

            var user_session = HttpContext.Session.GetString("Login");
            var userid = _db.Users.FirstOrDefault(m => m.Email == user_session).UserId;

            ModelState.Remove("Stories");
            ModelState.Remove("MissionTheme");
            ModelState.Remove("missionMedia");
            ModelState.Remove("Mission");
            ModelState.Remove("User");
            ModelState.Remove("storyuserid");
            ModelState.Remove("StoryId");
            ModelState.Remove("MissionApplication");
            ModelState.Remove("Storydata.User");
            ModelState.Remove("Storydata.Status");
            ModelState.Remove("StoryMedium");
            ModelState.Remove("videoUrl");
            ModelState.Remove("Storydata.Mission");
            ModelState.Remove("StoryIdsForPriview");
            ModelState.Remove("storymedia");
            
            if (ModelState.IsValid)
            {
                var exsiting = _db.Stories.FirstOrDefault(m => m.MissionId == storymodel.Storydata.MissionId && m.UserId == userid);
                var recObj = new Story()
                {
                    UserId = userid,
                    MissionId = storymodel.Storydata.MissionId,
                    Title = storymodel.Title,
                    Description = storymodel.Description,
                    Status = "PENDING"
                };


                if (exsiting == null)
                {
                    if (storymodel != null)
                    {
                        _db.Stories.Add(recObj);
                        _db.SaveChanges();

                    }
                }
                else
                {
                    exsiting.Title = storymodel.Title;
                    exsiting.Description = storymodel.Description;
                    exsiting.Status = "PENDING";
                    exsiting.UpdatedAt = DateTime.Now;
                    _db.Stories.Update(exsiting);
                    _db.SaveChanges();
                }



                var storyIds = _db.Stories.FirstOrDefault(m => m.MissionId == storymodel.Storydata.MissionId && m.UserId == userid).StoryId;
                var existingMedia = _db.StoryMedia.Where(m => m.StoryId == storyIds && m.Type == "URL").ToList();

                //delete Dublicate element from data base 
                if (existingMedia.Count > 0)
                {
                    // check if there are any media files or URLs associated with the story ID
                    var hasUrls = existingMedia.Any(m => m.Type == "URL");
                    if (hasUrls)
                    {
                        // delete existing media records associated with the story ID
                        _db.StoryMedia.RemoveRange(existingMedia);
                        _db.SaveChanges();

                    }
                }


                /*var UrlPath = new List<string>();*/
                var youtubeurl = url.ToString().Split("\r\n");
                foreach (var formFile in youtubeurl)
                {
                    if(formFile.ToString() != " ")
                    {
                        StoryMedium mediaobj = new StoryMedium();
                        if (exsiting != null)
                        {
                            mediaobj.StoryId = exsiting.StoryId;
                        }
                        else
                        {
                            mediaobj.StoryId = recObj.StoryId;
                        }
                        mediaobj.Path = formFile.ToString();
                        mediaobj.Type = "URL";
                        _db.StoryMedia.Add(mediaobj);
                        _db.SaveChanges();
                    }
                    
                }
                return RedirectToAction("Story", "Story");
            }
            else
            {
                StoryModel sa = new StoryModel();
                sa.MissionApplication = _db.MissionApplications.Include(m => m.Mission).ToList();
                sa.User = _db.Users;
                sa.UserId = userid;
                return View(sa);
            }    
            
            

        }

        [HttpPost]
        public long SaveStory(IFormFileCollection files, string story_details)
        {
            var user_session = HttpContext.Session.GetString("Login");
            var userid = _db.Users.FirstOrDefault(m => m.Email == user_session).UserId;
            JObject parseObject = JObject.Parse(story_details);
            var missionid = parseObject.Value<long>("MissionId");
            var title = parseObject.Value<string>("Title");
            var storydesc = parseObject.Value<string>("Storydesc");
            JArray array = (JArray)parseObject["Videourl"];
            JArray ImgArr = (JArray)parseObject["Image"];
            var date = parseObject.Value<DateTime>("Date");
            long ids;


            var stories = _story.GetStoryData();

            var recObj = new Story()
            {
                UserId = userid,
                MissionId = missionid,
                Title = title,
                Description = storydesc,
                Date = date,
                Status = "DRAFT"
            };
            var exsiting = _db.Stories.FirstOrDefault(m => m.MissionId == missionid && m.UserId == userid);

            if(exsiting == null)
            {
                if (recObj != null)
                {
                    _db.Stories.Add(recObj);
                    _db.SaveChanges();
                 
                }
            }
            else
            {
                exsiting.Title = title;
                exsiting.Description = storydesc;
                exsiting.Status = "DRAFT";
                exsiting.Date = date;
                exsiting.UpdatedAt = DateTime.Now;
                _db.Stories.Update(exsiting);
                _db.SaveChanges();
            }

            if(exsiting != null)
            {
                var existingMedia = _db.StoryMedia.Where(m => m.StoryId == exsiting.StoryId && m.Type != "URL").ToList();
                ids = exsiting.StoryId;


                //delete Dublicate element from data base 

                if (existingMedia.Count > 0 && existingMedia.Count > ImgArr.Count)
                {
                    var hasMedia = existingMedia.Any(m => m.Type == "PNG");
                    if (hasMedia)
                    {
                        _db.StoryMedia.RemoveRange(existingMedia);
                        _db.SaveChanges();
                    }
                    foreach (var item in ImgArr)
                    {
                        StoryMedium mediaobj = new StoryMedium();
                        mediaobj.StoryId = existingMedia[0].StoryId;
                        mediaobj.Type = "PNG";
                        mediaobj.Path = item.ToString();
                        _db.StoryMedia.Add(mediaobj);
                    }
                    _db.SaveChanges();

                }

            }
            else
            {
                ids = recObj.StoryId;
            }





            var filePaths = new List<string>();
            foreach (var formFile in files)
            {
                    StoryMedium mediaobj = new StoryMedium();

                    if (formFile.Length > 0)
                    {
                        // full path to file in temp location
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/StoryImg", formFile.FileName);
                        filePaths.Add(filePath);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            formFile.CopyTo(stream);
                        }
                    }
                    if (exsiting != null)
                    {
                        mediaobj.StoryId = exsiting.StoryId;
                        mediaobj.Path = "/StoryImg/" + formFile.FileName;
                        mediaobj.Type = "PNG";
                        _db.StoryMedia.Add(mediaobj);
                        _db.SaveChanges();
                    }
                    else
                    {
                        mediaobj.StoryId = recObj.StoryId;
                        mediaobj.Path = "/StoryImg/" + formFile.FileName;
                        mediaobj.Type = "PNG";
                        _db.StoryMedia.Add(mediaobj);
                        _db.SaveChanges();

                    }
           
                
            }

            if(exsiting != null)
            {
                var VideoUrl = _db.StoryMedia.Where(m => m.StoryId == exsiting.StoryId && m.Type == "URL").ToList();
                if (VideoUrl.Count > 0 && VideoUrl != null)
                {
                    _db.StoryMedia.RemoveRange(VideoUrl);
                }
            }
            

            // for Urlpath save in folder
            foreach (var formFile in array)
            {
                if(formFile.ToString() != "")
                {
                    StoryMedium mediaobj = new StoryMedium();
                    if (exsiting != null)
                    {
                        mediaobj.StoryId = exsiting.StoryId;
                        mediaobj.Path = formFile.ToString();
                        mediaobj.Type = "URL";
                        _db.StoryMedia.Add(mediaobj);
                        _db.SaveChanges();
                    }
                    else
                    {
                        mediaobj.StoryId = recObj.StoryId;
                        mediaobj.Path = formFile.ToString();
                        mediaobj.Type = "URL";
                        _db.StoryMedia.Add(mediaobj);
                        _db.SaveChanges();
                    }
                }
                
            }

            return ids;

        }

        public IActionResult Storydetail(long storyid)
        {
            var user_session = HttpContext.Session.GetString("Login");
            var userid = _db.Users.FirstOrDefault(m => m.Email == user_session).UserId;

           
            StoryModel storydetail = new StoryModel();
            storydetail.Stories = _story.GetStoryData();
            storydetail.MissionApplication = _db.MissionApplications.Include(m => m.Mission).ToList();
            storydetail.storyuserid = _story.GetStoryData().FirstOrDefault(m => m.StoryId == storyid).UserId;
            storydetail.User = _db.Users;
            storydetail.StoryId = storyid;
            storydetail.UserId = userid;
            storydetail.Storydata = _db.Stories.Where(m => m.StoryId == storyid).FirstOrDefault();
            storydetail.StoryMedium = _db.StoryMedia.ToList();

            var url = _db.StoryMedia.Where(m => m.StoryId != storyid && m.Type == "URL").ToList();
            var urllink = new List<string>();
            if(url != null)
            {
                for (int i = 0; i <url.Count(); i++)
                {
                    urllink.Add(url[i].Path);
                }

                for (int i = 0; i < urllink.Count(); i++)
                {
                    urllink[i] = urllink[i].Substring(urllink[i].LastIndexOf("/") + 1);
                }
            }
            
            storydetail.videoUrl = urllink.ToList();

            if (storydetail.Storydata.Views == null)
            {
                storydetail.Storydata.Views = 1;
            }
            else
            {
                storydetail.Storydata.Views = storydetail.Storydata.Views + 1;
            }
            

            _db.Stories.Update(storydetail.Storydata);
            _db.SaveChanges();

            return View(storydetail);
        }


        public void Recommandation(string recommend)
        {


            var parseObject = JObject.Parse(recommend);
            var userstoryid = parseObject.Value<long>("StoryId");
            var UId = parseObject.Value<long>("UserId");
            var loggeduserid = parseObject.Value<long>("FromUserId");
            var Email = parseObject.Value<string>("UserEmail");

            var recObj = new StoryInvite()
            {
                StoryId = userstoryid,
                FromUserId = loggeduserid,
                ToUserId = UId,
            };


            /*logged_in_user_email = Email*/
            ;

            if (Email != null)
            {
                _db.StoryInvites.Add(recObj);
                _db.SaveChanges();
                SendmailtoFriends(Email, userstoryid);
            }
        }


        public void SendmailtoFriends(string email, long id)
        {

            var passwordresetlink = Url.Action("Storydetail", "Story", new { storyid = id }, Request.Scheme);
            var link = Url.Action("Index", "Home", new { url = passwordresetlink }, Request.Scheme);
            TempData["link"] = passwordresetlink;
            var emailfrom = new MailAddress("amangandhi0523@gmail.com");
            var frompwd = "ifuempfdxibysfbg";
            var toEmail = new MailAddress(email);

            String body = "Here is Story link <br>" + link;

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

    }
}


