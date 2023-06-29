using CIPlatform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.MissionModel
{
    public class StorydetailsModel
    {
        public List<AllUserdataModel> AllUsers;

        [Key]

        public List<User> Users { get; set; }
        
        public List<Story> stories { get; set; }

        public List<StoryMedium> storiesMedium { get; set; }  

        public List<StoryInvite> storiesInvite { get; set; }

        public List<string> MediaPaths { get; set; }
    }
}
