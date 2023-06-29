using CIPlatform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.AdminModel
{
    public class MissionApplicationAdminModel
    {
        [Key]

        public List<User> Users { get; set; } = new List<User>();

        public List<MissionApplication> MissionApplications { get; set; } = new List<MissionApplication>();

        public List<Mission> missions { get; set; } = new List<Mission>();

    }
}
