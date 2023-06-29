using CIPlatform.Entities.AdminModel;
using CIPlatform.Entities.DataModels;
using CIPlatform.Entities.MissionModel;
using CIPlatform.Repositories.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform.Controllers
{
    public class AdminController : Controller
    {
        private CiplatformContext _context;
        private readonly IUserAdminRepository _userAdminRepository;
        private readonly ICmsAdminRepository _cmsAdminRepository;
        private readonly IMissionAdminRepository _missionAdminRepository;
        private readonly IMissionThemeAdminRepository _missionThemeAdminRepository;
        private readonly IMissionSkillsAdminRepository _missionSkillsAdminRepository;
        private readonly IMissionApplicationRepository _missionApplicationRepository;
        private readonly IStoryAdminRepository _storyAdminRepository;
        private readonly IStoryDetailsRepository _storyDetailsRepository;
        private readonly IBannerAdminRepository _bannerAdminRepository;
        public AdminController(CiplatformContext context, ILogger<StoryController> logger, IUserAdminRepository userAdminRepository, ICmsAdminRepository cmsAdminRepository, IMissionAdminRepository missionAdminRepository,
                                IMissionThemeAdminRepository missionThemeAdminRepository, IMissionSkillsAdminRepository missionSkillsAdminRepository, IMissionApplicationRepository missionApplicationRepository, IStoryAdminRepository storyAdminRepository, IStoryDetailsRepository storyDetailsRepository, IBannerAdminRepository bannerAdminRepository)
        {
            _context = context;
            _userAdminRepository = userAdminRepository;
            _cmsAdminRepository = cmsAdminRepository;
            _missionAdminRepository = missionAdminRepository;
            _missionThemeAdminRepository = missionThemeAdminRepository;
            _missionSkillsAdminRepository = missionSkillsAdminRepository;
            _missionApplicationRepository = missionApplicationRepository;
            _storyAdminRepository = storyAdminRepository;
            _storyDetailsRepository = storyDetailsRepository;
            _bannerAdminRepository = bannerAdminRepository;
        }

        public IActionResult UserAdmin(int page)
        {
            if (HttpContext.Session.GetString("TokenAdmin") == "null")
            {
                return RedirectToAction("Login", "UserAuth");
            }

            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");

            var totalUsers = _context.Users.Where(user => user.Flag == 1).Count();

            var pageNumber = 0;

            if (page == 0)
            {
                pageNumber = 1;
            }
            else
            {
                pageNumber = page;
            }
            var pageSize = 8;
            ViewBag.TotalPages = Math.Ceiling((double)totalUsers / pageSize);
            var skipRows = (pageNumber - 1) * pageSize;
            UserAdminModel userdetails = _userAdminRepository.GetAllUserAdminData(skipRows, pageSize);

            ViewBag.PageNumber = pageNumber;

            return View(userdetails);
        }

        //Email Validate
        [HttpPost]

        public IActionResult UserEmailValidate(string email)
        {

            var emailValidateResult = _userAdminRepository.VlidateUserEmail(email);

            if (emailValidateResult)
            {

                return Json(new { Success = true });
            }
            else
            {
                return Json(new { Success = false });
            }
        }

        // add new user by admin

        [HttpPost]
        public IActionResult AddUserAdminProfile(UserAdminModel data)
        {
            // Check if email already exists
            if (!_userAdminRepository.VlidateUserEmail(data.Email))
            {
                ModelState.AddModelError("Email", "Email already exist");
                return Json(new { success = false, message = "Email already exists" });
            }

            // Add new user
            if (_userAdminRepository.AdminNewUserData(data))
            {
                TempData["success13"] = "Great, successfully added new user!";
                return Json(new { success = true, message = "Successfully add new user" });
            }
            else
            {
                TempData["error13"] = "Something went wrong!";
                return Json(new { success = false, message = "Something went wrong" });
            }
        }


        //draft data show
        [HttpPost]
        public IActionResult DraftUserAdminShow(int Userid)
        {

            var draftuserdata = _context.Users.FirstOrDefault(userdata => userdata.UserId == Userid);

            if (draftuserdata != null)
            {
                string cityname = "select city";
                long cityid = 0;
                var city = _context.Cities.FirstOrDefault(c => c.CityId == draftuserdata.CityId);
                if (city != null)
                {
                    cityname = city.Name;
                    cityid = city.CityId;
                }

                string countryname = "select country";
                long countryid = 0;
                var country = _context.Countries.FirstOrDefault(country => country.CountryId == draftuserdata.CountryId);
                if (country != null)
                {
                    countryname = country.Name;
                    countryid = country.CountryId;
                }

                var data = new
                {
                    firstname = draftuserdata.FirstName,
                    lastname = draftuserdata.LastName,
                    email = draftuserdata.Email,
                    password = draftuserdata.Password,
                    empid = draftuserdata.EmployeeId,
                    title = draftuserdata.Title,
                    managerdetails = draftuserdata.Manager,
                    department = draftuserdata.Department,
                    profiletext = draftuserdata.ProfileText,
                    whyivol = draftuserdata.WhyIVolunteer,
                    cityname = cityname,
                    countryname = countryname,
                    countryidvalue = countryid,
                    cityidvalue = cityid,
                    availability = draftuserdata.Availablity,
                    linkedin = draftuserdata.LinkedInUrl,
                    status = draftuserdata.Status,
                };
                return Json(new { success = true, userdata = data });
            }

            return Json(new { success = false });

        }


        // Update user Data by admin

        [HttpPost]
        public IActionResult UpdateUserAdminProfile(UserAdminModel data, long Userid)
        {

            // Update user data
            if (_userAdminRepository.AdminUpdateUserData(data, Userid))
            {
                TempData["success15"] = "Great, successfully changed data!";
                return Json(new { success = true, message = "Successfully save user data" });
            }
            else
            {
                TempData["error13"] = "Something went wrong!";
                return Json(new { success = false, message = "Something went wrong" });
            }
        }

        // Soft Delete User Records 

        [HttpPost]

        public IActionResult DeleteUserRecords(long deleteUserid)
        {

            var deletedata = _userAdminRepository.DeleteUserRecords(deleteUserid);

            if (deletedata == true)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }

            return Json(new { success = true });

        }

        //------------------------ Cms admin---------------------------------
        // Cms admin

        public IActionResult CmsAdmin(int page)
        {
            if (HttpContext.Session.GetString("TokenAdmin") == "null")
            {
                return RedirectToAction("Login", "UserAuth");
            }
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");

            var totalCmsPages = _context.CmsPages.Where(cms => cms.Flag == 1).Count();

            var pageNumber = 0;

            if (page == 0)
            {
                pageNumber = 1;
            }
            else
            {
                pageNumber = page;
            }
            var pageSize = 8;
            ViewBag.TotalPages = Math.Ceiling((double)totalCmsPages / pageSize);
            var skipRows = (pageNumber - 1) * pageSize;

            CmsAdminModel cmsdetails = _cmsAdminRepository.GetAllCmsAdminData(skipRows, pageSize);

            ViewBag.PageNumber = pageNumber;
            return View(cmsdetails);
        }

        // add new Cms by admin

        [HttpPost]
        public IActionResult AddCmsAdmin(CmsAdminModel data)
        {
            // Check if title already exists or not
            if (!_cmsAdminRepository.VlidateCmsTitle(data.Title))
            {
                ModelState.AddModelError("Title", " Title already exists");
                return Json(new { success = false, message = "Title already exists" });
            }

            // Add new user
            if (_cmsAdminRepository.AdminNewCmsData(data))
            {
                TempData["successcmsadd"] = "Great, successfully added !";
                return Json(new { success = true, message = "Successfully added" });
            }
            else
            {
                TempData["errorcms"] = "Something went wrong!";
                return Json(new { success = false, message = "Something went wrong" });
            }
        }

        //draft data show
        [HttpPost]
        public IActionResult DraftCmsAdminShow(int Cmsid)
        {

            var draftcmsdata = _context.CmsPages.FirstOrDefault(cmsdata => cmsdata.CmsPageId == Cmsid);

            if (draftcmsdata != null)
            {
                var data = new
                {
                    title = draftcmsdata.Title,
                    details = draftcmsdata.Description,
                    slug = draftcmsdata.Slug,
                    status = draftcmsdata.Status,
                };
                return Json(new { success = true, cmsdata = data });
            }

            return Json(new { success = false });

        }

        // Update Cms Data 

        [HttpPost]
        public IActionResult UpdateCmsAdmin(CmsAdminModel data, long Cmsid)
        {
            var cmsTitleName = _context.CmsPages.Where(cms => cms.CmsPageId == Cmsid && cms.Title == data.Title).FirstOrDefault();
            if (cmsTitleName == null)
            {
                // Check Title exists
                if (!_cmsAdminRepository.VlidateCmsTitle(data.Title))
                {
                    ModelState.AddModelError("Title", "Title already exists");
                    return Json(new { success = false, message = "Title already exists" });
                }
            }

            // Update user data
            if (_cmsAdminRepository.AdminUpdateCmsData(data, Cmsid))
            {
                TempData["successcmsadd"] = "Great, successfully changed data!";
                return Json(new { success = true, message = "Successfully save data" });
            }
            else
            {
                TempData["errorcms"] = "Something went wrong!";
                return Json(new { success = false, message = "Something went wrong" });
            }
        }

        // Soft Delete Cms Records 

        [HttpPost]

        public IActionResult DeleteCmsRecords(long Cmsid)
        {

            var deletedata = _cmsAdminRepository.DeleteCmsRecords(Cmsid);

            if (deletedata == true)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }

        }

        //------------------------ Mission admin---------------------------------


        public IActionResult MissionAdmin(int page)
        {
            if (HttpContext.Session.GetString("TokenAdmin") == "null")
            {
                return RedirectToAction("Login", "UserAuth");
            }

            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");

            var totalmission = _context.Missions.Count();

            var pageNumber = 0;

            if (page == 0)
            {
                pageNumber = 1;
            }
            else
            {
                pageNumber = page;
            }
            var pageSize = 8;
            ViewBag.TotalPages = Math.Ceiling((double)totalmission / pageSize);
            var skipRows = (pageNumber - 1) * pageSize;

            MissionAdminModel Missiondetails = _missionAdminRepository.GetAllMissionAdminData(skipRows, pageSize);

            ViewBag.PageNumber = pageNumber;
            return View(Missiondetails);
        }

        //add new Mission by admin

        [HttpPost]
        public IActionResult AddMissionAdmin(MissionAdminModel data, string allSkills, string imageNames)
        {

            // Add new Mission
            if (_missionAdminRepository.AddMissionData(data, allSkills, imageNames))
            {
                TempData["successmissionadd"] = "Great, successfully Mission added !";
                return Json(new { success = true, message = "Successfully Mission added " });
            }
            else
            {
                TempData["errormission"] = "Something went wrong!";
                return Json(new { success = false, message = "Something went wrong" });
            }
        }

        //draft Mission data show
        [HttpPost]
        public IActionResult DraftMissionAdminShow(int Missionid)
        {

            var draftmissiondata = _context.Missions.FirstOrDefault(mission => mission.MissionId == Missionid);

            if (draftmissiondata != null)
            {
                var city = _context.Cities.FirstOrDefault(c => c.CityId == draftmissiondata.CityId);
                var country = _context.Countries.FirstOrDefault(country => country.CountryId == draftmissiondata.CountryId);
                var theme = _context.MissionThemes.FirstOrDefault(theme => theme.MissionThemeId == draftmissiondata.ThemeId);
                var goal = _context.GoalMissions.FirstOrDefault(goal => goal.MissionId == Missionid);
                var missionSkills = _context.MissionSkills.Where(skill => skill.MissionId == Missionid).Select(skill => skill.SkillId).ToList();
                var goalvalue = 0;
                var goaltext = "";
                if (draftmissiondata.MissionType == "Goal")
                {
                    goalvalue = goal.GoalValue;
                    goaltext = goal.GoalObjectiveText;
                }
                var data = new
                {
                    title = draftmissiondata.Title,
                    shortdes = draftmissiondata.ShortDescription,
                    description = draftmissiondata.Description,
                    orgname = draftmissiondata.OrganizationName,
                    orgdetails = draftmissiondata.OrganizationDetail,
                    cityname = city.Name,
                    countryname = country.Name,
                    countryidvalue = country.CountryId,
                    cityidvalue = city.CityId,
                    themename = theme.Title,
                    themeidvalue = theme.MissionThemeId,
                    stardate = draftmissiondata.StartDate,
                    enddate = draftmissiondata.EndDate,
                    deadline = draftmissiondata.Deadline,
                    goalvalue = goalvalue,
                    goaltext = goaltext,
                    seats = draftmissiondata.SeatLeft,
                    availability = draftmissiondata.Availability,
                    type = draftmissiondata.MissionType,
                    status = draftmissiondata.Status,
                    missionskills = missionSkills
                };
                return Json(new { success = true, missiondata = data });
            }

            return Json(new { success = false });

        }

        // Update Mission Data 

        [HttpPost]
        public IActionResult UpdateMissionAdmin(MissionAdminModel data, long Missionid, string allSkills)
        {

            // Update Mission data
            if (_missionAdminRepository.AdminUpdateMissionData(data, Missionid, allSkills))
            {
                TempData["successmissionadd"] = "Great, successfully changed data!";
                return Json(new { success = true, message = "Successfully save data" });
            }
            else
            {
                TempData["errormission"] = "Something went wrong!";
                return Json(new { success = false, message = "Something went wrong" });
            }
        }

        // Soft Delete Mission Records 

        [HttpPost]

        public IActionResult DeleteMissionRecords(long Missionid)
        {
            var deletedata = _missionAdminRepository.DeleteMissionRecords(Missionid);

            if (deletedata == true)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }

        }

        //--------------------------------Mission Theme admin--------------------------------

        public IActionResult MissionThemeAdmin(int page)
        {
            if (HttpContext.Session.GetString("TokenAdmin") == "null")
            {
                return RedirectToAction("Login", "UserAuth");
            }
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");

            var totaltheme = _context.MissionThemes.Where(theme => theme.Flag == 1).Count();

            var pageNumber = 0;

            if (page == 0)
            {
                pageNumber = 1;
            }
            else
            {
                pageNumber = page;
            }
            var pageSize = 8;
            ViewBag.TotalPages = Math.Ceiling((double)totaltheme / pageSize);
            var skipRows = (pageNumber - 1) * pageSize;

            MissionThemeAdminModel MissionThemeDetails = _missionThemeAdminRepository.GetAllMissionThemeAdminData(skipRows, pageSize);

            ViewBag.PageNumber = pageNumber;

            return View(MissionThemeDetails);
        }

        //add new Theme by admin

        [HttpPost]
        public IActionResult AddThemeAdmin(MissionThemeAdminModel data)
        {
            // Check Skills exists
            if (!_missionThemeAdminRepository.VlidateUserTheme(data.Title))
            {
                ModelState.AddModelError("Email", "Theme already exists");
                return Json(new { success = false, message = "Theme already exists" });
            }

            // Add new Theme
            if (_missionThemeAdminRepository.AdminNewThemeData(data))
            {
                TempData["skillsuccess"] = "Great, successfully added new Theme!";
                return Json(new { success = true, message = "Successfully add new Theme" });
            }
            else
            {
                TempData["skillerror"] = "Something went wrong!";
                return Json(new { success = false, message = "Something went wrong" });
            }

        }

        //draft Theme data show
        [HttpPost]
        public IActionResult DraftThemeAdminShow(long Themeid)
        {

            var draftthemedata = _context.MissionThemes.Find(Themeid);

            if (draftthemedata != null)
            {
                var data = new
                {
                    themename = draftthemedata.Title,
                    status = draftthemedata.Status,
                };
                return Json(new { success = true, themedata = data });
            }

            return Json(new { success = false });

        }

        // Update Theme Data by admin

        [HttpPost]
        public IActionResult UpdateThemeAdmin(MissionThemeAdminModel data, long Themeid)
        {

            var themename = _context.MissionThemes.Where(theme => theme.MissionThemeId == Themeid && theme.Title == data.Title).FirstOrDefault();
            if (themename == null)
            {
                // Check Skills exists
                if (!_missionThemeAdminRepository.VlidateUserTheme(data.Title))
                {
                    ModelState.AddModelError("Email", "Theme already exists");
                    return Json(new { success = false, message = "Theme already exists" });
                }
            }
            // Update theme data
            if (_missionThemeAdminRepository.AdminUpdateThemeData(data, Themeid))
            {
                TempData["skillsuccess"] = "Great, successfully changed data!";
                return Json(new { success = true, message = "Successfully save Theme data" });
            }
            else
            {
                TempData["error13"] = "Something went wrong!";
                return Json(new { success = false, message = "Something went wrong" });
            }
        }

        // Soft Delete Theme Records 

        [HttpPost]

        public IActionResult DeleteThemeRecords(long deleteThemeid)
        {

            var deletedata = _missionThemeAdminRepository.DeleteThemeRecords(deleteThemeid);

            if (deletedata == true)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }

        }

        //-------------------------------- Mission Skills admin----------------------------------------

        public IActionResult MissionSkillsAdmin(int page)
        {
            if (HttpContext.Session.GetString("TokenAdmin") == "null")
            {
                return RedirectToAction("Login", "UserAuth");
            }
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");

            var totalskills = _context.Skills.Where(skill => skill.Flag == 1).Count();

            var pageNumber = 0;

            if (page == 0)
            {
                pageNumber = 1;
            }
            else
            {
                pageNumber = page;
            }
            var pageSize = 8;
            ViewBag.TotalPages = Math.Ceiling((double)totalskills / pageSize);
            var skipRows = (pageNumber - 1) * pageSize;

            MissionSkillsAdminModel MissionSkillsDetails = _missionSkillsAdminRepository.GetAllMissionSkillsAdminData(skipRows, pageSize);

            ViewBag.PageNumber = pageNumber;

            return View(MissionSkillsDetails);
        }

        //add new user by admin

        [HttpPost]
        public IActionResult AddSkillAdmin(MissionSkillsAdminModel data)
        {
            // Check Skills exists
            if (!_missionSkillsAdminRepository.VlidateUserSkill(data.SkillName))
            {
                ModelState.AddModelError("Email", "Skill already exists");
                return Json(new { success = false, message = "Skill already exists" });
            }

            // Add new skill
            if (_missionSkillsAdminRepository.AdminNewSkillData(data))
            {
                TempData["skillsuccess"] = "Great, successfully added new skill!";
                return Json(new { success = true, message = "Successfully add new skill" });
            }
            else
            {
                TempData["skillerror"] = "Something went wrong!";
                return Json(new { success = false, message = "Something went wrong" });
            }

        }

        //draft story data show
        [HttpPost]
        public IActionResult DraftskillAdminShow(long Skillid)
        {

            var draftskilldata = _context.Skills.Find(Skillid);

            if (draftskilldata != null)
            {

                var data = new
                {
                    skillname = draftskilldata.SkillName,
                    status = draftskilldata.Status,
                };
                return Json(new { success = true, skilldata = data });
            }

            return Json(new { success = false });

        }


        // Update skill Data by admin

        [HttpPost]
        public IActionResult UpdateSkillAdmin(MissionSkillsAdminModel data, long Skillid)
        {

            var skillname = _context.Skills.Where(skill => skill.SkillId == Skillid && skill.SkillName == data.SkillName).FirstOrDefault();
            if (skillname == null)
            {
                // Check Skills exists
                if (!_missionSkillsAdminRepository.VlidateUserSkill(data.SkillName))
                {
                    ModelState.AddModelError("Email", "Skill already exists");
                    return Json(new { success = false, message = "Skill already exists" });
                }
            }
            // Update user data
            if (_missionSkillsAdminRepository.AdminUpdateSkillData(data, Skillid))
            {
                TempData["skillsuccess"] = "Great, successfully changed data!";
                return Json(new { success = true, message = "Successfully save skill data" });
            }
            else
            {
                TempData["error13"] = "Something went wrong!";
                return Json(new { success = false, message = "Something went wrong" });
            }
        }

        // Soft Delete skill Records 

        [HttpPost]

        public IActionResult DeleteSkillRecords(long deleteSkillid)
        {

            var deletedata = _missionSkillsAdminRepository.DeleteSkillRecords(deleteSkillid);

            if (deletedata == true)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }

        }


        //----------------------------Mission Application admin-----------------------------------

        public IActionResult MissionApplicationAdmin(int page)
        {
            if (HttpContext.Session.GetString("TokenAdmin") == "null")
            {
                return RedirectToAction("Login", "UserAuth");
            }
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");

            var totalapplication = _context.MissionApplications.Count();

            var pageNumber = 0;

            if (page == 0)
            {
                pageNumber = 1;
            }
            else
            {
                pageNumber = page;
            }
            var pageSize = 8;
            ViewBag.TotalPages = Math.Ceiling((double)totalapplication / pageSize);
            var skipRows = (pageNumber - 1) * pageSize;

            MissionApplicationAdminModel MissionAppData = _missionApplicationRepository.GetAllMissionApplicationAdminData(skipRows, pageSize);

            ViewBag.PageNumber = pageNumber;

            return View(MissionAppData);

        }

        // Application Status approved data show
        [HttpPost]
        public IActionResult ApplicationStatusAdminShow(long Applicationid)
        {
            var Applicationdata = _context.MissionApplications.Find(Applicationid);

            if (Applicationdata != null)
            {
                if (Applicationdata.ApprovalStatus == "APPROVE")
                {
                    return Json(new { success = false });
                }
                else
                {
                    Applicationdata.ApprovalStatus = "APPROVE";
                    _context.MissionApplications.Update(Applicationdata);
                    _context.SaveChanges();
                    return Json(new { success = true });
                }

            }

            return Json(new { error = true });

        }

        // Application Status dclined data show
        [HttpPost]
        public IActionResult ApplicationStatusdeclinedAdminShow(long Applicationid)
        {
            var Applicationdata = _context.MissionApplications.Find(Applicationid);

            if (Applicationdata != null)
            {
                if (Applicationdata.ApprovalStatus == "DECLINE")
                {
                    return Json(new { success = false });
                }
                else
                {
                    Applicationdata.ApprovalStatus = "DECLINE";
                    _context.MissionApplications.Update(Applicationdata);
                    _context.SaveChanges();
                    return Json(new { success = true });
                }

            }

            return Json(new { error = true });

        }


        //------------------------- Story admin----------------------------------------

        public IActionResult StoryAdmin(int page)
        {
            if (HttpContext.Session.GetString("TokenAdmin") == "null")
            {
                return RedirectToAction("Login", "UserAuth");
            }
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");

            var totalstories = _context.Stories.Where(story => story.Flag == 1).Count();

            var pageNumber = 0;

            if (page == 0)
            {
                pageNumber = 1;
            }
            else
            {
                pageNumber = page;
            }
            var pageSize = 8;
            ViewBag.TotalPages = Math.Ceiling((double)totalstories / pageSize);
            var skipRows = (pageNumber - 1) * pageSize;

            StoryAdminModel StoryAdminData = _storyAdminRepository.GetAllStoryAdminData(skipRows, pageSize);

            ViewBag.PageNumber = pageNumber;
            return View(StoryAdminData);
        }

        // Story Status published data show
        [HttpPost]
        public IActionResult StoryAdminPublishedStatus(long Storyid)
        {
            var StoryData = _context.Stories.Find(Storyid);

            if (StoryData != null)
            {
                if (StoryData.Status == "PUBLISHED")
                {
                    return Json(new { success = false });
                }
                else
                {
                    StoryData.Status = "PUBLISHED";
                    _context.Stories.Update(StoryData);
                    _context.SaveChanges();
                    return Json(new { success = true });
                }

            }

            return Json(new { error = true });

        }

        // Story Status Declined data show
        [HttpPost]
        public IActionResult StoryAdminDeclinedStatus(long Storyid)
        {
            var StoryData = _context.Stories.Find(Storyid);

            if (StoryData != null)
            {
                if (StoryData.Status == "DECLINED")
                {
                    return Json(new { success = false });
                }
                else
                {
                    StoryData.Status = "DECLINED";
                    _context.Stories.Update(StoryData);
                    _context.SaveChanges();
                    return Json(new { success = true });
                }

            }

            return Json(new { error = true });

        }

        // Soft Delete story Records 

        [HttpPost]

        public IActionResult DeleteStoryRecords(long deleteStoryid)
        {

            var deletedata = _storyAdminRepository.DeleteStoryRecords(deleteStoryid);

            if (deletedata == true)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }

        }

        //------------------------- Story admin view----------------------------------------

        public IActionResult StoryAdminView(int id)
        {
            if (HttpContext.Session.GetString("TokenAdmin") == "null")
            {
                return RedirectToAction("Login", "UserAuth");
            }
            StorydetailsModel sharestorydetails = _storyDetailsRepository.GetOneStoryData(13, id);
            return View(sharestorydetails);
        }

        //-------------------------Banner admin view----------------------------------------

        public IActionResult BannerAdmin(int page)
        {
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");

            var totalbanner = _context.Banners.Where(banner =>banner.Flag == 1).Count();

            var pageNumber = 0;

            if (page == 0)
            {
                pageNumber = 1;
            }
            else
            {
                pageNumber = page;
            }
            var pageSize = 8;
            ViewBag.TotalPages = Math.Ceiling((double)totalbanner / pageSize);
            var skipRows = (pageNumber - 1) * pageSize;

            BannerAdminModel bannerdetails = _bannerAdminRepository.GetAllBannerAdminData(skipRows, pageSize);
            ViewBag.PageNumber = pageNumber;
            return View(bannerdetails);
        }

        [HttpPost]
        public IActionResult AddBannerAdmin(BannerAdminModel data, string imageNames)
        {
            // Add new Banner
            if (_bannerAdminRepository.AdminBannerAddData(data, imageNames))
            {
                TempData["bannersuccess"] = "Great, successfully added new Banner!";
                return Json(new { success = true, message = "Successfully add new banner" });
            }
            else
            {
                TempData["bannererror"] = "Something went wrong!";
                return Json(new { success = false, message = "Something went wrong" });
            }

        }

        //draft banner data show
        [HttpPost]
        public IActionResult DrafBannerAdminShow(long Bannerid)
        {

            var draftbannerdata = _context.Banners.Find(Bannerid);

            if (draftbannerdata != null)
            {
                var data = new
                {
                    description = draftbannerdata.Text,
                    order = draftbannerdata.SortOrder,
                };
                return Json(new { success = true, bannerdata = data });
            }

            return Json(new { success = false });

        }

        // Update banner Data by admin

        [HttpPost]
        public IActionResult UpdateBannerAdmin(BannerAdminModel data, long Bannerid , string imagename)
        {
            // Update banner data
            if (_bannerAdminRepository.AdminUpdateBannerData(data, Bannerid , imagename))
            {
                TempData["bannersuccess"] = "Great, successfully changed data!";
                return Json(new { success = true, message = "Successfully save skill data" });
            }
            else
            {
                TempData["bannererror"] = "Something went wrong!";
                return Json(new { success = false, message = "Something went wrong" });
            }
        }

        // Soft Delete Banner Records 

        [HttpPost]

        public IActionResult DeleteBannerRecords(long deleteBannerId)
        {
            var deletedata = _bannerAdminRepository.DeleteBannerRecords(deleteBannerId);

            if (deletedata == true)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }

        }


        [HttpPost]

        public IActionResult userLogout()
        {
            HttpContext.Session.SetString("Token", "null");
            return Json(new { success = true });

        }

        [HttpPost]
        public IActionResult adminLogout()
        {
            HttpContext.Session.SetString("TokenAdmin", "null");
            return Json(new { success = true });

        }

    }
}
