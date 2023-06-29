using CIPlatform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.AdminModel
{
    public class StoryAdminModel
    {
        [Key]

        public List<User> Users { get; set; } = new List<User>();

        public List<Story> Stories { get; set; } = new List<Story>();

        public List<Mission> Missions { get; set; } = new List<Mission>();
    }
}
