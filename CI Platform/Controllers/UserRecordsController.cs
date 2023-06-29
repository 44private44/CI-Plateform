using CIPlatform.Entities.DataModels;
using CIPlatform.Entities.UserRecordsModel;
using CIPlatform.Repositories.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CI_Platform.Controllers
{
    public class UserRecordsController : Controller
    {

        private CiplatformContext _context;
        private readonly ILogger<StoryController> _logger;
        private readonly IUserEditProfileRepository _userEditProfileRepository;
        public UserRecordsController(CiplatformContext context, ILogger<StoryController> logger, IUserEditProfileRepository userEditProfileRepository)
        {
            _context = context;
            _userEditProfileRepository = userEditProfileRepository;
        }

        //main view page
        public IActionResult UserEditProfile()
        {
            if (HttpContext.Session.GetString("Token") == "null")
            {
                return RedirectToAction("Login", "UserAuth");
            }

            string Id = HttpContext.Session.GetString("userId");
            int UserId = int.Parse(Id);

            UserEditProfileModel userprofilerecords = _userEditProfileRepository.GetAllUserRecords(UserId);
            return View(userprofilerecords);
        }

        //draft data show
        [HttpPost]
        public IActionResult Draftdatashow()
        {
            string Id = HttpContext.Session.GetString("userId");
            if (!string.IsNullOrEmpty(Id))
            {
                int UserId = int.Parse(Id);
                var draftuserdata = _context.Users.FirstOrDefault(userdata => userdata.UserId == UserId);

                if (draftuserdata != null)
                {
                    string cityname = "select city";
                    var city = _context.Cities.FirstOrDefault(c => c.CityId == draftuserdata.CityId);
                    if (city != null)
                    {
                        cityname = city.Name;
                    }

                    string countryname = "select country";
                    var country = _context.Countries.FirstOrDefault(country => country.CountryId == draftuserdata.CountryId);
                    if (country != null)
                    {
                        countryname = country.Name;
                    }

                    var data = new
                    {
                        firstname = draftuserdata.FirstName,
                        lastname = draftuserdata.LastName,
                        empid = draftuserdata.EmployeeId,
                        title = draftuserdata.Title,
                        managerdetails = draftuserdata.Manager,
                        department = draftuserdata.Department,
                        profiletext = draftuserdata.ProfileText,
                        whyivol = draftuserdata.WhyIVolunteer,
                        cityname = cityname,
                        countryname = countryname,
                        cityidvalue = draftuserdata.CityId,
                        countryidvalue = draftuserdata.CountryId,
                        availability = draftuserdata.Availablity,
                        linkedin = draftuserdata.LinkedInUrl,
                    };
                    return Json(new { success = true, userdata = data });
                }
            }

            return Json(new { success = false });

        }

        // user image upload
        [HttpPost]
        public IActionResult Updateuerimage(IFormFile file)
        {
            string Id = HttpContext.Session.GetString("userId");
            if (!string.IsNullOrEmpty(Id))
            {
                int UserId = int.Parse(Id);
                var validuserdata = _context.Users.FirstOrDefault(userdata => userdata.UserId == UserId);
                var oldImage = validuserdata.Avatar;
                //var guid = Guid.NewGuid().ToString().Substring(0, 8);
                var fileName = $"{UserId}_{file.FileName}"; // getting filename
                if (validuserdata != null)
                {
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "IMAGES", "CI Platform Assets", fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    validuserdata.Avatar = $"IMAGES/CI Platform Assets/{fileName}";
                    _context.SaveChanges();

                    var useravatar = _context.Users.Where(user => user.UserId == UserId).Select(user => user.Avatar).FirstOrDefault();
                    HttpContext.Session.SetString("useravatar", useravatar);

                    if (oldImage != null)
                    {
                        System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldImage)); // deleteOldImage
                    }
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });
        }

        //--------------------------------------------------------------

        //change password
        [HttpPost]
        public IActionResult changepassword(string currentpassword, string newpassword)
        {
            string Id = HttpContext.Session.GetString("userId");
            int UserId = int.Parse(Id);

            var updatepassword = _context.Users.FirstOrDefault(user => user.UserId == UserId);

            if (updatepassword != null)
            {
                if (currentpassword != newpassword)
                {
                    if (updatepassword.Password == currentpassword)
                    {
                        updatepassword.Password = newpassword;
                        updatepassword.UpdatedAt = DateTime.Now;
                        _context.SaveChanges();
                        return Json(new { success = true });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Your current password is missing or incorrect !" });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Your new password cannot be the same as your current password !" });
                }
            }
            else
            {
                return Json(new { error = true });

            }
        }

        public JsonResult GetCitiesByCountry(int countryId)
        {
            var cities = _context.Cities.Where(c => c.CountryId == countryId)
                                         .Select(c => new { cityId = c.CityId, cityName = c.Name })
                                         .ToList();
            return Json(cities);
        }

        //add skills
        [HttpPost]

        public IActionResult addskills(int skillid)
        {
            string Id = HttpContext.Session.GetString("userId");
            int UserId = int.Parse(Id);

            var skillsdata = _context.UserSkills.Where(skills => skills.UserId == UserId && skills.SkillId == skillid).FirstOrDefault();

            if (skillsdata == null)
            {
                var newuserskillls = new UserSkill
                {
                    UserId = UserId,
                    SkillId = skillid,
                };

                //add new skills in the table
                _context.UserSkills.Add(newuserskillls);
                _context.SaveChanges();

                var dataid = newuserskillls.UserSkillId;
                var dataname = _context.Skills.Where(s => s.SkillId == skillid).Select(s => s.SkillName).FirstOrDefault();

                return Json(new { success = true, dataid = dataid, dataname = dataname });
            }
            else
            {
                return Json(new { success = false });

            }
        }

        //remove skills
        [HttpPost]

        public IActionResult removeskills(int userskillid)
        {
            if (userskillid != null)
            {
                var removeskills = new UserSkill
                {
                    UserSkillId = userskillid,
                };

                //remove skills in the table
                _context.UserSkills.Remove(removeskills);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });

        }

        // Update the skills
        [HttpGet]
        public ActionResult GetUserSkills()
        {
            string Id = HttpContext.Session.GetString("userId");
            int UserId = int.Parse(Id);

            var userSkills = _context.UserSkills
                           .Where(us => us.UserId == UserId)
                           .Select(us => us.Skill.SkillName) // select only the skill name
                           .ToList();
            return Json(userSkills);
        }


        // update the user profile
        [HttpPost]
        public IActionResult UpdateUserProfile(UserEditProfileModel model)
        {
            string Id = HttpContext.Session.GetString("userId");
            long UserId = long.Parse(Id);

            var user = _context.Users.Find(UserId);

            if (user == null)
            {
                return NotFound(); // Return a 404 error if the user is not found
            }

            // Update the user data with the data from the model
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.EmployeeId = model.EmployeeId;
            user.Manager = model.Manager;
            user.Title = model.Title;
            user.Department = model.Department;
            user.ProfileText = model.ProfileText;
            user.WhyIVolunteer = model.WhyIVolunteer;
            user.CityId = model.CityId;
            user.CountryId = model.CountryId;
            user.Availablity = model.Availablity;
            user.LinkedInUrl = model.LinkedInUrl;
            user.UpdatedAt = DateTime.Now;

            _context.SaveChanges();

            TempData["success8"] = "update successful";
            return RedirectToAction("UserEditProfile", "UserRecords");
        }

        //contact us data
        [HttpPost]

        public IActionResult Contactusdata(String subject, String message)
        {
            string Id = HttpContext.Session.GetString("userId");
            int UserId = int.Parse(Id);

            _userEditProfileRepository.contactussavedata(UserId, subject, message);

            return Json(new { success = true });
        }

        //Privacy and Policy

        public IActionResult Policy()
        {
            if (HttpContext.Session.GetString("Token") == "null")
            {
                return RedirectToAction("Login", "UserAuth");
            }

            string Id = HttpContext.Session.GetString("userId");
            int UserId = int.Parse(Id);

            UserEditProfileModel userprofilerecords = _userEditProfileRepository.GetAllUserRecords(UserId);
            return View(userprofilerecords);
        }

        public IActionResult UserPrivacy()
        {
            UserEditProfileModel userprofilerecords = _userEditProfileRepository.GetAllUserRecords(13);
            return View(userprofilerecords);
        }

    }
}
