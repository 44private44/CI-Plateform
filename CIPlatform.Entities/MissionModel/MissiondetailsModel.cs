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
    public class MissiondetailsModel
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

        public List<Mission> RelatedMissions = new List<Mission>();
        public List<Comment> commentdetails;

        public FavouriteMission FavouriteMissions { get; set; }   

        public List<FilterMission> FilterMissions { get; set; }

            public Comment comment { get; set; }

        public List<User> Users { get; set; }

        public List<MissionInvite> missionInvites { get; set; }

        public List<MissionApplication> missionApplications { get; set; }

        public List<MissionMedium> missionmediums { get; set;}
        public List<MissionRating> missionRatings { get; set;}

        public List<MissionDocument> missiondocuments { get; set; }
    }
}
