using CIPlatform.Entities.DataModels;
using CIPlatform.Entities.TimesheetModel;
using CIPlatform.Repositories.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CI_Platform.Controllers
{
    public class TimesheetController : Controller
    {
        private CiplatformContext _context;
        private readonly ITimesheetRepository _timesheetRepository;
        public TimesheetController(CiplatformContext context, ITimesheetRepository timesheetRepository)
        {
            _context = context;
            _timesheetRepository = timesheetRepository;
        }

        public IActionResult VolunteeringTimesheet()
        {
            if (HttpContext.Session.GetString("Token") == "null")
            {
                return RedirectToAction("Login", "UserAuth");
            }

            string Id = HttpContext.Session.GetString("userId");
            int UserId = int.Parse(Id);

            VolunteeringTimesheetModel userprofilerecords = _timesheetRepository.GetAllTimesheetData(UserId);
            return View(userprofilerecords);

        }


        // submit time data into database

        [HttpPost]

        public IActionResult timemissionsubmit(int missionid, DateTime date, int hours, int minute, string message)
        {
            string Id = HttpContext.Session.GetString("userId");
            int UserId = int.Parse(Id);

            var hourresponse = _timesheetRepository.GetAllHourData(missionid, UserId, date, hours, minute, message);

            if (hourresponse == true)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { error = true });

            }

        }

        //Draft time data

        [HttpPost]

        public IActionResult Drafttimedata(int missionid)
        {
            string Id = HttpContext.Session.GetString("userId");
            int UserId = int.Parse(Id);

            var getstartdate = _context.Missions.Where(mission => mission.MissionId == missionid).Select(mission => mission.StartDate).FirstOrDefault();
            var timedata = _context.Timesheets.Include(mission => mission.Mission).SingleOrDefault(userdata => userdata.UserId == UserId && userdata.MissionId == missionid);

            if (timedata != null)
            {
                string timeString = timedata.Time.ToString();
                string[] timeComponents = timeString.Split(':');

                var draftdata = new
                {
                    date = timedata.DateVolunteered,
                    hour = timeComponents[0],
                    minute = timeComponents[1],
                    message = timedata.Notes,
                    missionid = timedata.MissionId,
                    startdate = timedata.Mission.StartDate,
                    missiontitle = timedata.Mission.Title,
                    action = timedata.Action,
                };
                return Json(new { success = true, data = draftdata });
            }
            else
            {
               
                return Json(new { success = false, data = getstartdate });
            }
        }

        // Delete Timesheet

        [HttpPost]

        public IActionResult DeleteTimesheetRecords(int timesheetid)
        {

            var timesheeetdata = _timesheetRepository.DeleteTimesheedData(timesheetid);

            if (timesheeetdata == true)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        // submit goal data into database

        [HttpPost]

        public IActionResult goalmissionsubmit(int missionid, DateTime date, int action, string message)
        {
            string Id = HttpContext.Session.GetString("userId");
            int UserId = int.Parse(Id);

            var goalresponse = _timesheetRepository.GetAllGoalData(missionid, UserId, date, action, message);

            if (goalresponse == true)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { error = true });

            }

        }


    }
}

