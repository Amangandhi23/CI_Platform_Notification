using CI_Platform.Entities.Data;
using CI_Platform.Entities.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform.Controllers
{
    public class PolicyController : Controller
    {
        private readonly CiPlatformContext _db;
        public PolicyController(CiPlatformContext db)
        {
            _db = db;
        }

        public IActionResult PolicyPage()
        {
            var user_session = HttpContext.Session.GetString("Login");
            var userid = _db.Users.FirstOrDefault(m => m.Email == user_session).UserId;

            Profile modelDetail = new Profile();
            modelDetail.LoginUser = _db.Users.FirstOrDefault(u => u.UserId == userid);
            modelDetail.Policy = _db.CmsPages.Where(id => id.DeletedAt == null).ToList();

            return View(modelDetail);
        }

        public IActionResult CmsPagePolicy()
        {
            var Policy = _db.CmsPages.Where(id => id.DeletedAt == null).ToList();

            return Json(Policy);
        }

    }
}
