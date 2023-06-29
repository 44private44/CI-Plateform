using CIPlatform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.MissionModel
{
    public class StorylistingModel
    {
        [Key]

        public List<User> Users { get; set; }

        public List<Country> Country { get; set; }

        public List<City> City { get; set; }
        
        public List<MissionTheme> MissionThemes { get; set; }
        public List<Skill> Skill { get; set; }
        public List<Mission>missions { get; set; }

        public List<Story> stories { get; set; }

        public List<StoryMedium> storyMedia { get; set; }

    }
}
