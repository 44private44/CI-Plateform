using CIPlatform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.TimesheetModel
{
    public class VolunteeringTimesheetModel
    {
        [Key]

        public List<User> Users { get; set; }

        public List<Mission> missions { get; set; }

        public List<MissionApplication> MissionApplications { get; set; }

        public List<Timesheet> timesheets { get; set; }
    }
}
