using CIPlatform.Entities.DataModels;
using CIPlatform.Entities.TimesheetModel;
using CIPlatform.Repositories.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repositories.Repository
{
    public class TimesheetRepository : Repository<Entities.TimesheetModel.VolunteeringTimesheetModel>, ITimesheetRepository
    {

        private readonly CiplatformContext _context;
        public TimesheetRepository(CiplatformContext context) : base(context)
        {
            _context = context;
        }

        public VolunteeringTimesheetModel GetAllTimesheetData(int UserID)
        {
            List<User> users = new List<User>();
            List<Mission> missions = new List<Mission>();
            List<MissionApplication> missionApplications = new List<MissionApplication>();
            List<Timesheet> timesheets = new List<Timesheet>();
            var alltimesheetdata = new VolunteeringTimesheetModel
            {
                Users = users,
                missions = missions,
                MissionApplications = missionApplications,
                timesheets = timesheets,
            };

            var userdata = _context.Users.Where(u => u.UserId == UserID).ToList();
            alltimesheetdata.Users = userdata;

            var allmissionapplied = _context.MissionApplications.Include(m => m.Mission).Where(mission => mission.UserId == UserID && mission.ApprovalStatus == "APPROVE").ToList();
            alltimesheetdata.MissionApplications = allmissionapplied;

            var alltabledata = _context.Timesheets.Where(hdata => hdata.UserId == UserID).ToList();
            alltimesheetdata.timesheets = alltabledata;

            return alltimesheetdata;
        }


        // store new hour data into database.
        public bool GetAllHourData(int missionid, int UserId, DateTime date, int hours, int minute, String message)
        {
            TimeSpan time = new TimeSpan(hours, minute, 0);

            var timedata = _context.Timesheets.SingleOrDefault(userdata => userdata.UserId == UserId && userdata.MissionId == missionid);

            if (timedata == null)
            {
                //add new data
                var newtimemissionadd = new Timesheet
                {
                    MissionId = missionid,
                    UserId = UserId,
                    Time = time,
                    DateVolunteered = date,
                    Notes = message,
                    Action = 0,
                    Status = "APPROVED",
                };

                _context.Timesheets.Add(newtimemissionadd);
                _context.SaveChanges();
                return true;
            }
            else
            {
                //Update data
                timedata.Time = time;
                timedata.DateVolunteered = date;
                timedata.Notes = message;

                _context.SaveChanges();
                return true;
            }

        }


        // delete data from database.
        public bool DeleteTimesheedData(int timesheetid)
        {
            var timesheeetdata = _context.Timesheets.Where(timesheet => timesheet.TimesheetId == timesheetid).FirstOrDefault();

            if (timesheeetdata != null)
            {
                _context.Timesheets.Remove(timesheeetdata);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        // store new goal data into database.
        public bool GetAllGoalData(int missionid, int UserId, DateTime date, int action, String message)
        {

            var timedata = _context.Timesheets.SingleOrDefault(userdata => userdata.UserId == UserId && userdata.MissionId == missionid);

            if (timedata == null)
            {
                //add new data
                var newtimemissionadd = new Timesheet
                {
                    MissionId = missionid,
                    UserId = UserId,
                    DateVolunteered = date,
                    Notes = message,
                    Action = action,
                    Time = TimeSpan.Zero,
                    Status = "APPROVED",
                };

                _context.Timesheets.Add(newtimemissionadd);
                _context.SaveChanges();
                return true;
            }
            else
            {
                //Update data
                timedata.DateVolunteered = date;
                timedata.Notes = message;
                timedata.Action = action;

                _context.SaveChanges();
                return true;
            }

        }

    }
}
