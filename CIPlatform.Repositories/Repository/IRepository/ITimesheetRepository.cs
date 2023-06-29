using CIPlatform.Entities.DataModels;
using CIPlatform.Entities.TimesheetModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repositories.Repository.IRepository
{
    public interface ITimesheetRepository : IRepository<VolunteeringTimesheetModel>
    {
        public VolunteeringTimesheetModel GetAllTimesheetData(int UserId);

        public bool GetAllHourData(int missionid, int UserId, DateTime date, int hours, int minute, String message);

        public bool DeleteTimesheedData(int timesheetid);

        public bool GetAllGoalData(int missionid, int UserId, DateTime date, int action, String message);


    }
}
