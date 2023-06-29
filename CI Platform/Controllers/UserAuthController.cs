//using CI_Platform.CIPlatform.Entities;
//using CI_Platform.DataDB;
//using CI_Platform.DataModels;
using CIPlatform.Entities.DataModels;
using CIPlatform.Entities.UserAccountAuthModel;
using CIPlatform.Repositories.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace CI_Platform.Controllers
{
    public class UserAuthController : Controller
    {
        private CiplatformContext _context2;
        private IUserRepository _context;
        private CiplatformContext _db;
        private readonly IUserRepository _userRepository;
        public UserAuthController(IUserRepository context, CiplatformContext db, IUserRepository userRepository, CiplatformContext context2)
        {
            _context = context;
            _context2 = context2;
            _db = db;
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Login
        public IActionResult Login()
        {

            LoginModel userdetails = _userRepository.GetAllLoginData();
            return View(userdetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(User obj)
        {
            if (ModelState.IsValid)
            {
                var uservalidate = _db.Users.Where(user => user.Email == obj.Email).FirstOrDefault();

                if (uservalidate != null && _context.IsPasswordmatch(obj))
                {
                    HttpContext.Session.SetString("Token", uservalidate.Email);

                    var user = _db.Users.Where(x => x.Email == obj.Email).FirstOrDefault();
                    var username = user.FirstName + " " + user.LastName;

                    HttpContext.Session.SetString("username", username);

                    var userId = (user.UserId).ToString();
                    HttpContext.Session.SetString("userId", userId);

                    var useravatar = user.Avatar;
                    HttpContext.Session.SetString("useravatar", useravatar);

                    var useremail = user.Email;
                    HttpContext.Session.SetString("useremail", useremail);

                    TempData["success"] = "Login successful";
                    return RedirectToAction("Landing", "Mission");
                }
                else
                {
                    var adminvalidate = _db.Admins.Where(x => x.Email == obj.Email).FirstOrDefault();
                    if (adminvalidate != null)
                    {
                        var admin = new Admin
                        {
                            Email = obj.Email,
                            Password = obj.Password
                        };
                        if (_context.IsAdminPasswordMatch(admin))
                        {
                            HttpContext.Session.SetString("TokenAdmin", admin.Email);

                            var Adminid = (adminvalidate.AdminId).ToString();
                            HttpContext.Session.SetString("AdminId", Adminid);

                            var adminname = adminvalidate.FirstName + " " + adminvalidate.LastName;
                            HttpContext.Session.SetString("AdminName", adminname);

                            TempData["success12"] = "Login successful";
                            return RedirectToAction("UserAdmin", "Admin");
                        }
                    }
                }

            }

            TempData["error2"] = "Email not registered or incorrect password !";
            LoginModel userdetails = _userRepository.GetAllLoginData();
            return View(userdetails);

        }

        // Registration 
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registration(User obj, RegisterModel model) // show error message for that reason add model.
        {
            if (ModelState.IsValid)
            {
                if (!_context.IsRegistered(obj))
                {
                    var userdata = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        PhoneNumber = 0,
                        Avatar = "IMAGES/CI Platform Assets/user1.png",
                        Password = model.Password,
                        ConPassword = model.ConPassword,
                        Mobileno = model.Mobileno,
                    };
                    _context2.Add(userdata);
                    TempData["success"] = "Registration successful";
                    _context2.SaveChanges();
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["error"] = "Email already registered !";
                    return View();
                }
            }
            TempData["error"] = "Fill Proper data !";
            return View();

        }

        //Email Validate
        [HttpPost]

        public IActionResult UserEmailValidateRegister(string email)
        {

            var emailValidateResult = _userRepository.RegisterEmailMatch(email);

            if (emailValidateResult)
            {
                return Json(new { Success = true });
            }
            else
            {
                return Json(new { Success = false });
            }
        }

        //Email Validate login
        [HttpPost]

        public IActionResult UserEmailValidateLogin(string email)
        {

            var loginemailvalidate = _db.Users.Where(user => user.Email == email).FirstOrDefault();
            var adminvalidate = _db.Admins.Where(admin => admin.Email == email).FirstOrDefault();

            if (loginemailvalidate != null || adminvalidate != null)
            {
                return Json(new { Success = true });
            }
            else
            {
                return Json(new { Success = false });
            }
        }

        //Forgot Passsword
        public IActionResult Forgot()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Forgot(ForgotModel obj)
        {
            if (ModelState.IsValid)
            {
                var obj2 = _db.Users.Where(a => a.Email.Equals(obj.Email)).FirstOrDefault();
                if (obj2 != null)
                {

                    TempData["success5"] = "Email Successfully sent";

                    //random token generate
                    Random random = new Random();

                    //ascii value
                    int capitalCharCode = random.Next(65, 91);
                    char randomCapitalChar = (char)capitalCharCode;


                    int randomint = random.Next();


                    int SmallcharCode = random.Next(97, 123);
                    char randomChar = (char)SmallcharCode;

                    String token = "";
                    token += randomCapitalChar.ToString();
                    token += randomint.ToString();
                    token += randomChar.ToString();

                    //link generate
                    var PasswordResetLink = Url.Action("Reset", "UserAuth", new { Email = obj.Email, Token = token }, Request.Scheme);

                    //store data into table 
                    var ResetPasswordInfo = new PasswordReset()
                    {
                        Email = obj.Email,
                        Token = token,
                    };
                    _db.PasswordResets.Add(ResetPasswordInfo);
                    _db.SaveChanges();

                    //email sent
                    var fromEmail = new MailAddress("sohammodi124421@gmail.com");
                    var toEmail = new MailAddress(obj.Email);
                    var fromEmailPassword = "whwuvzwoegqezftj";
                    string subject = "Reset Password";
                    string body = PasswordResetLink;

                    var smtp = new SmtpClient
                    {

                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
                    };

                    MailMessage message = new MailMessage(fromEmail, toEmail);
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;
                    message.IsBodyHtml = true;
                    smtp.Send(message);

                }
                else
                {
                    TempData["error3"] = "User is not Registered !";
                    ModelState.AddModelError("Email", "Email is not Registered !");
                }

            }
            return View(obj);
        }

        //Reset password
        public IActionResult Reset(string email, string token)
        {
            var activatecheck = _db.PasswordResets.Any(activate => activate.Email == email && activate.Token == token && activate.Linkactivate == 1);
            var alink = _db.PasswordResets.Where(activate => activate.Email == email && activate.Token == token).FirstOrDefault();

            if (activatecheck)
            {
                alink.Linkactivate = 0;
                _db.SaveChanges();
                return View(new ResetModel
                {
                    Email = email,
                    Token = token
                });
            }
            else
            {
                TempData["errorlink"] = "Link Expired !";
                return RedirectToAction("Forgot");
            }
        }

        [HttpPost]
        public IActionResult Reset(ResetModel model)
        {
            var ResetPasswordData = _db.PasswordResets.Any(e => e.Email == model.Email && e.Token == model.Token);

            if (ResetPasswordData)
            {
                var x = _db.Users.FirstOrDefault(e => e.Email == model.Email);

                x.Password = model.Password;

                _db.Users.Update(x);
                _db.SaveChanges();
                TempData["success3"] = "Password changed successful";
                return RedirectToAction("Login");
            }
            else
            {
                ModelState.AddModelError("Token", "Reset Passwordword Link is Invalid");
            }
            return View(model);
        }


    }
}
