//using CI_Platform.CIPlatform.Entities;
//using CI_Platform.DataDB;
using CIPlatform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.MissionModel
{
    public class FilterMission
    {
        [Key]
        public Mission? Missions { get; set; }

        public MissionMedium? image { get; set; }
        public List<Country>? Country { get; set; }
        public List<City>? Cities { get; set; }
        public List<MissionTheme>? themes { get; set; }
        public List<Skill>? skills { get; set; }

        public GoalMission goal { get; set; }

        public MissionRating rating { get; set; }
        public bool isvalid { get; set; }

        public list <FavouriteMission> favmission { get; set; }

        public List<User> users { get; set; }

        public List<MissionApplication> missionApplications { get; set; }   

    }
}
