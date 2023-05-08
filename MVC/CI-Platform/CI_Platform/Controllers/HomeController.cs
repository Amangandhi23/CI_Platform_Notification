using CI_Platform.Entities.Data;
using CI_Platform.Entities.Models;
using CI_Platform.Models;
using CI_Platform.Repository.Interface;
using CI_Platform.Entities.ViewModel;

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace CI_PLATFORM.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CiPlatformContext _db;
        private readonly IConfiguration _config;
        public IAccountRepository _accountRepository;
        private string subject;
        private ICountryRepository _countryname;
        private IPasswordRepository _password;



        public int IsEmailConfirmedasync { get; private set; }

        public HomeController(ICountryRepository countryname, IPasswordRepository password, ILogger<HomeController> logger, CiPlatformContext db, IConfiguration config, IAccountRepository accountRepository, ICountryRepository userdataRepository)
        {
            _logger = logger;
            _db = db;
            _config = config;
            _accountRepository = accountRepository;
            _countryname = countryname;
            _password = password;
        }


        public IActionResult GetBannerData()
        {
            var bannerdata = _db.Banners.Where(id => id.DeletedAt == null).OrderBy(m => m.SortOrder).ToList();
            return Json(bannerdata);
        }


        public IActionResult Index(string? url)
        {
            var user_session = HttpContext.Session.GetString("Login");
            if (user_session != null && url != null)
            {
                return Redirect(url);
            }
            else if(user_session != null)
            {
                return RedirectToAction("Landing", "Mission");
            }
            else
            {
                return View();
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(CI_Platform.Entities.ViewModel.Login model, string? url)
        {

            if (ModelState.IsValid)
            {
                var encreyptedpass = _password.EncryptPassword(model.Password);
                var user_detail = _db.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == encreyptedpass);
                if (user_detail != null)
                {
                    HttpContext.Session.SetString("Login", model.Email);
                    var user = _db.Users.Where(e => e.Email == model.Email).FirstOrDefault();
                    var full_name = user.FirstName + " " + user.LastName;
                    HttpContext.Session.SetString("username", full_name);


                    if (user_detail.Role == "Admin")
                    {
                        return RedirectToAction("AdminMission", "Admin");
                    }
                    else
                    {
                        if(user_detail.DeletedAt == null || user_detail.Status == "1")
                        {
                            if (url != null)
                            {
                                return Redirect(url);
                            }
                            if (user_detail.CityId == 0 || user_detail.ProfileText == null)
                            {
                                TempData["success"] = "Please Fill Required Information!";
                                return RedirectToAction("UserProfile", "Profile");
                            }
                            else
                            {
                                TempData["success"] = "login Successfully";
                                return RedirectToAction("Landing", "Mission");
                            }
                        }
                        else
                        {
                            TempData["error"] = "You Are Not Authnticate user";
                            return View();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("email", "Invalid Email or Password !");
                    return View();
                }
            }
            return View();
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Login");
            return RedirectToAction("Index");
        }



        public IActionResult Forgot()
        {
            return View();
        }

        public IUrlHelper GetUrl()
        {
            return Url;
        }



        [HttpPost]
        public async Task<IActionResult> Forgot(string email)
        {
            if (ModelState.IsValid)
            {
                var user = _db.Users.FirstOrDefault(u => u.Email == email && u.DeletedAt == null && u.Status == "1");
                if (user != null)
                {
                    var token = GenerateToken(user);
                    var token2 = new JwtSecurityTokenHandler().WriteToken(token);

                    var obj = new CI_Platform.Entities.Models.PasswordReset()
                    {
                        Email = email,
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        CreateAt = DateTime.Now
                    };

                    _db.PasswordResets.Add(obj);
                    _db.SaveChanges();

                    var passwordresetlink = Url.Action("Reset", "Home", new { Email = email, token = token2 }, Request.Scheme);
                    TempData["link"] = passwordresetlink;
                    var emailfrom = new MailAddress("amangandhi0523@gmail.com");
                    var frompwd = "ifuempfdxibysfbg";
                    var toEmail = new MailAddress(email);

                    String body = "Here is your reset Password link <br>" + passwordresetlink;

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
                    message.Subject = "Password Reset Link";
                    message.Body = body;
                    message.IsBodyHtml = true;
                    smtp.Send(message);

                    TempData["success"] = "Email has been sent to your email account";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = "Email has not been registered";
                    ModelState.AddModelError("email", "Email not registered.");
                }
            }


            return View();
        }

        private JwtSecurityToken GenerateToken(CI_Platform.Entities.Models.User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Email,user.Email),
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);


            return token;

        }



        [HttpGet]
        public IActionResult Reset(string token)
        {
            if (token == null)
            {
                return RedirectToAction("Forgot");
            }
            var findToken = _db.PasswordResets.FirstOrDefault(x => x.Token == token);
            var tokenObject = new JwtSecurityTokenHandler().ReadJwtToken(token);

            if (tokenObject.ValidTo.CompareTo(DateTime.UtcNow) > 0)
            {
                var input_email = tokenObject.Payload.Claims.ToList()[0].Value;
                ViewBag.Email = new
                {
                    email = input_email,
                    token = token
                };
                return View();
            }
            return RedirectToAction("Forgot");

        }


        [HttpPost]
        public IActionResult Reset(Reset obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Password == obj.ConfirmPassword)
                {
                    var encryptedpass = _password.EncryptPassword(obj.Password);

                    var user = _db.Users.FirstOrDefault(e => e.Email == obj.Email);
                    if(encryptedpass != user.Password)
                    {
                        user.Password = encryptedpass;
                        _db.Users.Update(user);
                        _db.SaveChanges();
                    }
                    return RedirectToAction("Index");

                }
            }
            return View();

        }

        public IActionResult Registration()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registration(Registration model)
        {
            if (ModelState.IsValid)
            {
                var encryptedpassword = _password.EncryptPassword(model.Password);
                var user = new CI_Platform.Entities.Models.User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    Password = encryptedpassword,
                    Avatar = "/StoryImg/" + "user1.png",
                };


                var user_exsists = _db.Users.Any(m => m.Email == model.Email);
                var userdata = _db.Users.FirstOrDefault(user => user.Email == model.Email);
                if (userdata == null)
                {
                    _db.Users.Add(user);
                    _db.SaveChanges();

                    TempData["Success"] = "Registration Completed";

                    return RedirectToAction("Index");
                }
                else
                {
                    if(userdata.Status == "0")
                    {
                        if(userdata.DeletedAt != null)
                        {
                            ModelState.AddModelError("Email", "You Are Not Authnticate user");
                            return View();
                        }
                        else
                        {
                            ModelState.AddModelError("Email", "You Profile Locked By Admin");
                            return View();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "email already exsist");
                    }
                }
            }
            return View(model);


        }



        public IActionResult Privacy()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}