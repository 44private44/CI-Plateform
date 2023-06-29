
using CIPlatform.Entities.DataModels;
using CIPlatform.Entities.MissionModel;
using CIPlatform.Repositories.Repository.IRepository;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;
using System.Net.Mail;

namespace CI_Platform.Controllers
{
    public class MissionController : Controller
    {
        private readonly ILogger<MissionController> _logger;
        private readonly IMissionRepository _MissionRepository;
        private readonly IMissionDetailsRepository _MissionDetailsRepository;
        private readonly ISPMissionDataRepository _SPMissionDataRepository;
        private CiplatformContext _context;
        public MissionController(CiplatformContext context, IMissionRepository missionRepository, IMissionDetailsRepository missiondetailsRepository, ISPMissionDataRepository SPMissionDataRepository, ILogger<MissionController> logger)
        {
            _context = context;
            _MissionRepository = missionRepository;
            _MissionDetailsRepository = missiondetailsRepository;
            _SPMissionDataRepository = SPMissionDataRepository;
            _logger = logger;
        }

        private readonly string connectionString = "Server=PCA172\\SQL2017;Database=CIPlatform;Trusted_Connection=True;MultipleActiveResultSets=true;User ID=sa;Password=Tatva@123;Integrated Security=False; TrustServerCertificate=True";
        [HttpGet]
        public async Task<IActionResult> Landing(int page) //int pageIndex
        {
            if (HttpContext.Session.GetString("Token") == "null")
            {
                return RedirectToAction("Login", "UserAuth");
            }

            // username

            ViewBag.usern = HttpContext.Session.GetString("username");

            string Id = HttpContext.Session.GetString("userId");
            int UserId = int.Parse(Id);

            ViewBag.UserId = UserId;

            //avatar 
            ViewBag.useravatar = HttpContext.Session.GetString("useravatar");

            string useravatar = HttpContext.Session.GetString("useravatar");

            //email

            string useremail = HttpContext.Session.GetString("useremail");
            ViewBag.useremail = useremail;

            //skills
            var skillList = _context.Skills.ToList();
            var selectedSkills = new SelectList(skillList, "SkillId", "SkillName");
            ViewBag.selectedskill = selectedSkills;

            var totalStories = _context.Missions.Count();

            var pageNumber = 0;

            if (page != 0)
            {

                pageNumber = page;
            }
            else
            {
                pageNumber = 1;
            }
            var pageSize = 3;
            ViewBag.TotalPages = Math.Ceiling((double)totalStories / pageSize);
            var skipRows = (pageNumber - 1) * pageSize;

            //filter mision
            List<FilterMission> FilterMission = _MissionRepository.GetAllMissions(UserId, skipRows, pageSize); //pageIndex
            ViewBag.PageNumber = pageNumber;
            ViewBag.FilterMisson = FilterMission;

            return View(FilterMission);

        }

        [HttpGet]
        public IActionResult GetCitiesByCountry(long CountryId)
        {
            var cities = _context.Cities.Where(c => c.CountryId == CountryId).ToList();
            return Json(cities);
        }

        [HttpPost]
        public IActionResult AddToFavorites(int missionId)
        {
            string Id = HttpContext.Session.GetString("userId");
            long userId = long.Parse(Id);

            // Check if the mission is already in favorites for the user
            if (_context.FavouriteMissions.Any(fm => fm.MissionId == missionId && fm.UserId == userId))
            {
                // Mission is already in favorites, return an error message or redirect back to the mission page
                var FavoriteMissionId = _context.FavouriteMissions.Where(fm => fm.MissionId == missionId && fm.UserId == userId).FirstOrDefault();
                _context.FavouriteMissions.Remove(FavoriteMissionId);
                _context.SaveChanges();
                return Ok();

                //return BadRequest("Mission is already in favorites.");
            }

            // Add the mission to favorites for the user
            var favoriteMission = new FavouriteMission { MissionId = missionId, UserId = userId };
            _context.FavouriteMissions.Add(favoriteMission);
            _context.SaveChanges();

            return Ok();
        }

        public IActionResult Missionnotfound()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Volunteering(int id)
        {
            if (HttpContext.Session.GetString("Token") == "null")
            {
                return RedirectToAction("Login", "UserAuth");
            }
            // username

            ViewBag.usern = HttpContext.Session.GetString("username");

            string Id = HttpContext.Session.GetString("userId");
            int UserId = int.Parse(Id);

            ViewBag.userId = UserId;
            //avatar 
            ViewBag.useravatar = HttpContext.Session.GetString("useravatar");

            string useravatar = HttpContext.Session.GetString("useravatar");

            //email

            string useremail = HttpContext.Session.GetString("useremail");
            ViewBag.useremail = useremail;

            //skills
            var skillList = _context.Skills.ToList();
            var selectedSkills = new SelectList(skillList, "SkillId", "SkillName");
            ViewBag.selectedskill = selectedSkills;

            //filter mision
            MissiondetailsModel missiondetails = _MissionDetailsRepository.GetAllMissionDetails(id, UserId);
            //ViewBag.FilterMisson = missiondetails;

            return View(missiondetails);

        }
        public IActionResult postcomment(int missionId, string commenttext, string commenuname)
        {
            string Id = HttpContext.Session.GetString("userId");
            long userId = long.Parse(Id);

            var commtxt = new Comment { Comment1 = commenttext, MissionId = missionId, UserId = userId, Username = commenuname };
            _context.Comments.Add(commtxt);
            _context.SaveChanges();
            return Ok();
            //return Redirect("Mission/Landing");

        }

        [HttpPost]
        public IActionResult recommendedcoworkers(string recoemail, int recommissionid)
        {
            string tempid = HttpContext.Session.GetString("userId");
            int UserId2 = int.Parse(tempid);

            if (ModelState.IsValid)
            {

                var obj2 = _context.Users.Where(a => a.Email.Equals(recoemail)).FirstOrDefault();
                var getTouserid = _context.Users.Where(user => user.Email == recoemail).Select(user => user.UserId).FirstOrDefault();
                var validatedata = _context.MissionInvites.Where(recommended => recommended.MissionId == recommissionid &&
                                    recommended.FromUserId == UserId2 && recommended.ToUserId == getTouserid).Select(recommended => recommended.MissionInviteId).FirstOrDefault();
                if (obj2 != null)
                {
                    if (validatedata == 0)
                    {
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
                        var PasswordResetLink = Url.Action("Login", "UserAuth", new { Email = recoemail, Token = token }, Request.Scheme);

                        // retrieve the user_id based on recoemail parameter
                        string Id = HttpContext.Session.GetString("userId");
                        int UserId = int.Parse(Id);

                        long toUserId = _context.Users.Where(a => a.Email.Equals(recoemail)).Select(a => a.UserId).FirstOrDefault();

                        //store data into table 
                        var ResetPasswordInfo = new MissionInvite()
                        {
                            FromUserId = UserId,
                            MissionId = recommissionid,
                            ToUserId = toUserId

                        };
                        _context.MissionInvites.Add(ResetPasswordInfo);
                        _context.SaveChanges();


                        //email sent
                        var fromEmail = new MailAddress("sohammodi124421@gmail.com");
                        var toEmail = new MailAddress(recoemail);
                        var fromEmailPassword = "whwuvzwoegqezftj";
                        string subject = "Recommende Co-worker";
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
                        return Json(new { success = true });
                    }
                    else
                    {
                        return Json(new { success = false });
                    }

                }
                else
                {
                    TempData["error11"] = "User is not Registered";
                    ModelState.AddModelError("Email", "Email is not Registered");
                }

            }
            return View(recoemail);
        }

        [HttpPost]
        public IActionResult Addrating(int MissionId, int rating)
        {

            if (HttpContext.Session.GetString("username") != null)
            {
                ViewBag.user1 = HttpContext.Session.GetString("username");
                ViewBag.UserId = HttpContext.Session.GetString("userId");
            }

            string Id = HttpContext.Session.GetString("userId");
            int UserId = int.Parse(Id);

            var existingrating = _context.MissionRatings.SingleOrDefault(fm => fm.UserId == UserId && fm.MissionId == MissionId);

            if (existingrating == null)
            {
                var newrating = new MissionRating
                {
                    UserId = UserId,
                    MissionId = MissionId,
                    Rating = rating
                };

                _context.MissionRatings.Add(newrating);
                _context.SaveChanges();
            }
            else
            {
                existingrating.Rating = rating;
                _context.SaveChangesAsync();
            }

            return Json(new { success = true });
        }

        [HttpPost]

        public IActionResult applieduser(int appliedmissionid, int applieduserid)
        {
            var existingstatus = _context.MissionApplications.SingleOrDefault(fm => fm.UserId == applieduserid && fm.MissionId == appliedmissionid);

            if (existingstatus == null)
            {
                var newstatus = new MissionApplication
                {
                    UserId = applieduserid,
                    MissionId = appliedmissionid,
                    ApprovalStatus = "APPROVE"
                };

                _context.MissionApplications.Add(newstatus);
                _context.SaveChanges();
            }
            return Json("Applied");
        }

        //SP ADO.NET get the mission Data
        [HttpGet]
        public IActionResult MissionData()
        {
            try
            {
                // mkae connection 
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand("allMissionData", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    // open connection
                    connection.Open();
                    // read the result thet returned by the SP
                    SqlDataReader reader = command.ExecuteReader();

                    List<SPMissionModel> missionDataList = new List<SPMissionModel>();
                    // returned by the SP read of the each row one by one 
                    while (reader.Read())
                    {
                        SPMissionModel missionData = new SPMissionModel();
                        // new mission is creted for each rows 
                        Mission mission = new Mission();
                        mission.MissionId = reader.GetInt64(reader.GetOrdinal("mission_id"));
                        mission.ThemeId = reader.GetInt64(reader.GetOrdinal("theme_id"));
                        mission.CityId = reader.GetInt64(reader.GetOrdinal("city_id"));
                        mission.CountryId = reader.GetInt64(reader.GetOrdinal("country_id"));
                        mission.Title = reader.GetString(reader.GetOrdinal("title"));
                        mission.ShortDescription = reader.GetString(reader.GetOrdinal("short_description"));
                        mission.Description = reader.GetString(reader.GetOrdinal("description"));
                        mission.StartDate = reader.GetDateTime(reader.GetOrdinal("start_date")).Date;
                        mission.EndDate = reader.GetDateTime(reader.GetOrdinal("end_date")).Date;
                        mission.MissionType = reader.GetString(reader.GetOrdinal("mission_type"));
                        mission.Status = reader.GetString(reader.GetOrdinal("status"));
                        mission.OrganizationName = reader.GetString(reader.GetOrdinal("organization_name"));
                        mission.OrganizationDetail = reader.GetString(reader.GetOrdinal("organization_detail"));
                        mission.Availability = reader.GetString(reader.GetOrdinal("availability"));
                        mission.Deadline = reader.GetDateTime(reader.GetOrdinal("deadline")).Date;
                        mission.SeatLeft = reader.GetString(reader.GetOrdinal("seat_left"));
                        missionData.Missions.Add(mission);

                        missionData.CityName = reader.GetString(reader.GetOrdinal("city_name"));
                        missionData.CountryName = reader.GetString(reader.GetOrdinal("country_name"));

                        missionData.Title = reader.GetString(reader.GetOrdinal("theme_name"));
                        if (mission.MissionType == "Goal")
                        {
                            missionData.GoalText = reader.GetString(reader.GetOrdinal("goal_text"));
                            missionData.GoalValue = reader.GetInt32(reader.GetOrdinal("goal_value"));
                        }

                        string approvalStatus = reader.IsDBNull(reader.GetOrdinal("approval_status")) ? null : reader.GetString(reader.GetOrdinal("approval_status"));
                        if (approvalStatus != null)
                        {
                            missionData.ApproveStatus = reader.GetString(reader.GetOrdinal("approval_status"));
                        }

                        int missionRating = reader.IsDBNull(reader.GetOrdinal("average_rating")) ? 0 : reader.GetInt32(reader.GetOrdinal("average_rating"));
                        if (missionRating != 0)
                        {
                            missionData.AverageRating = reader.GetInt32(reader.GetOrdinal("average_rating"));
                        }
                        missionData.ApplicationId = reader.IsDBNull(reader.GetOrdinal("mission_application_id")) ? 0 : reader.GetInt64(reader.GetOrdinal("mission_application_id"));


                        missionData.MediaPath = reader.GetString(reader.GetOrdinal("media_path"));

                        missionDataList.Add(missionData);
                    }

                    connection.Close();

                    // Return the data to the view
                    return View(missionDataList);
                }
            }
            catch (Exception ex)
            {
                // Return an error view or message
                return View("Error");
            }
        }


        // SP ADO.NET Insert the Mission Data

        public IActionResult SPMissionInsert()
        {
            SPFormViewModel viewModel = new SPFormViewModel();

            List<Country> countries = _context.Countries.ToList();
            List<City> cities = _context.Cities.ToList();

            viewModel.Cities = cities;
            viewModel.Countries = countries;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult SubmitForm(SPFormViewModel viewModel, List<IFormFile> files)
        {

            // Create a DataTable to hold the image data
            DataTable multiMissionMediaDataTable = new DataTable("MultiMissionMediaData");
            multiMissionMediaDataTable.Columns.Add("media_name", typeof(string));
            multiMissionMediaDataTable.Columns.Add("media_type", typeof(string));
            multiMissionMediaDataTable.Columns.Add("media_path", typeof(string));

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var imageNames = file.FileName;
                    var randomGenerate = Guid.NewGuid().ToString().Substring(0, 8);
                    var fileName = $"{randomGenerate}_{imageNames}";

                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "IMAGES", "CI Platform Assets", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    multiMissionMediaDataTable.Rows.Add("Mission1-Photo1 ", "jpeg", "IMAGES/CI Platform Assets/" + fileName);
                }
            }

            // Access the form data through the view model
            var submission = viewModel.Submission;

            // Create a connection to the SQL database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Create a command object for the stored procedure
                using (SqlCommand command = new SqlCommand("InsertMissionData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Set parameter values based on the form data
                    command.Parameters.AddWithValue("@themeId", 4);
                    command.Parameters.AddWithValue("@cityId", submission.CityId);
                    command.Parameters.AddWithValue("@countryId", submission.CountryId);
                    command.Parameters.AddWithValue("@title", submission.Title);
                    command.Parameters.AddWithValue("@shortDescription", submission.ShortDescription);
                    command.Parameters.AddWithValue("@description", "details");
                    command.Parameters.AddWithValue("@startDate", submission.StartDate);
                    command.Parameters.AddWithValue("@endDate", submission.EndDate);
                    command.Parameters.AddWithValue("@missionType", submission.MissionType);
                    command.Parameters.AddWithValue("@status", submission.Status);
                    command.Parameters.AddWithValue("@organizationName", submission.OrganizationName);
                    command.Parameters.AddWithValue("@organizationDetail", "details");
                    command.Parameters.AddWithValue("@availability", submission.Availability);
                    command.Parameters.AddWithValue("@seatleft", 200);
                    command.Parameters.AddWithValue("@userId", 13);
                    command.Parameters.AddWithValue("@deadline", submission.Deadline);
                    command.Parameters.AddWithValue("@multiMissionMediaData", multiMissionMediaDataTable);

                    if (submission.MissionType == "Goal")
                    {
                        command.Parameters.AddWithValue("@goalText", "Plan 10,000 trees");
                        command.Parameters.AddWithValue("@goalValue", 5000);

                    }

                    SqlParameter missionIdOutParameter = new SqlParameter("@missionIdOut", SqlDbType.BigInt);
                    missionIdOutParameter.Direction = ParameterDirection.Output;
                    command.Parameters.Add(missionIdOutParameter);

                    try
                    {
                        // Execute the stored procedure
                        command.ExecuteNonQuery();

                        // Retrieve the output parameter value
                        long missionId = (long)command.Parameters["@missionIdOut"].Value;

                    }
                    catch (SqlException ex)
                    {
                        // Handle the SQL exception
                        Console.WriteLine("Error inserting mission: " + ex.Message);
                        return Json(new { success = false });
                    }
                }

                connection.Close();
            }
            return Json(new { success = true });
        }

        // SP ADO.NET Mission Approve
        [HttpPost]

        public IActionResult SPMissionStatus(long SPApplicationID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("UpdateMissionStatus", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@missionApplicationId", SPApplicationID);
                        command.Parameters.AddWithValue("@approvalStatus", "APPROVE");

                        int rowsAffected = command.ExecuteNonQuery();

                    }
                    connection.Close();
                    return Json(new { success = true });
                }
            }
            catch (Exception ex)
            {
                // Handle the exception, log the error, or perform necessary actions
                Console.WriteLine("An error occurred: " + ex.Message);
                return StatusCode(500, "An error occurred while updating the mission approval status.");
            }
        }

        // SP ADO.NET Mission Approve
        [HttpPost]

        public IActionResult SPMissionStatusDecline(long SPApplicationID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("UpdateMissionStatus", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@missionApplicationId", SPApplicationID);
                        command.Parameters.AddWithValue("@approvalStatus", "DECLINE");

                        int rowsAffected = command.ExecuteNonQuery();
                    }
                    connection.Close();
                    return Json(new { success = true });
                }
            }
            catch (Exception ex)
            {
                // Handle the exception, log the error, or perform necessary actions
                Console.WriteLine("An error occurred: " + ex.Message);
                return StatusCode(500, "An error occurred while updating the mission approval status.");
            }
        }

        // SP used Dapper..
        [HttpGet]
        public IActionResult MissionDataByDapper()
        {
            try
            {
                var allMissionData = _SPMissionDataRepository.GetAllMissionDataByDapper();
                return View(allMissionData);
            }
            catch (Exception ex)
            {
                // Return an error view or message
                Console.WriteLine(ex.Message);
                return View("Error");
            }
        }

        // Dapper with argument
        [HttpGet]

        public IActionResult OneMissionDataByDApper(long MisionID)
        {
            try
            {
                var oneMissionData = _SPMissionDataRepository.OneMissionDataByDappaer(MisionID);
                return View(oneMissionData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View("Error");
            }
        }


        // Drag Drop Mission Sort
        [HttpGet]
        public IActionResult DragDrop_Mission_Sort()
        {
            try
            {
                var allMissionData = _SPMissionDataRepository.GetAllmissionByDragDrop();
                return View(allMissionData);
            }
            catch (Exception ex)
            {
                // Return an error view or message
                Console.WriteLine(ex.Message);
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFriendOrder(int friendId, int newOrder)
        {
            var result = _SPMissionDataRepository.UpdateFriendOrder(friendId, newOrder);
            if (result == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { error = true });
            }
        }

        // Sortable js lib used for sorting 

        public IActionResult SortableJSDragDrop()
        {
            try
            {
                var allMissionData = _SPMissionDataRepository.GetAllmissionByDragDrop();
                return View(allMissionData);
            }
            catch (Exception ex)
            {
                // Return an error view or message
                Console.WriteLine(ex.Message);
                return View("Error");
            }
        }

        // update the sort order in the database 

        [HttpPost]
        public ActionResult UpdateSortOrderbylib(long missionId, int newIndex)
        {
            try
            {
                var missionData = _context.Missions.FirstOrDefault(m => m.MissionId == missionId);
                if (missionData != null)
                {
                    var oldIndex = missionData.SortOrder;
                    if (oldIndex != newIndex)
                    {
                        // Update the sort order of the dragged mission
                        missionData.SortOrder = newIndex;
                        _context.SaveChanges();

                        // Retrieve the missions that need to be updated
                        var missionsToUpdate = _context.Missions.Where(m => m.SortOrder >= Math.Min((byte)oldIndex, newIndex) && m.SortOrder <= Math.Max((byte)oldIndex, newIndex) && m.MissionId != missionId);

                        // Update the sort order of other missions
                        foreach (var mission in missionsToUpdate)
                        {
                            if (newIndex > oldIndex)
                            {
                                mission.SortOrder--; // Decrement the sort order
                            }
                            else if (newIndex < oldIndex)
                            {
                                mission.SortOrder++; // Increment the sort order
                            }
                        }
                        _context.SaveChanges();
                        return Json(new { success = true });
                    }
                    else
                    {
                        return Json(new { success = false });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Mission not found." });
                }
            }
            catch (Exception ex)
            {
                // Return an error response if there is an exception
                return Json(new { success = false, message = "An error occurred while updating the sort order." });
            }
            
        }
    }
}
