using CIPlatform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.MissionModel
{
    public class SPMissionModel
    {
        [Key]

        public List<Mission> Missions { get; set; } = new List<Mission>();

        public string CityName { get; set; }

        public string CountryName { get; set; }

        public string? Title { get; set; }

        public string MediaPath { get; set; }

        public string? GoalText { get; set; } = string.Empty;
        public int? GoalValue { get; set; } = 0;

        public string ApproveStatus { get; set; } = string.Empty;

        public long? ApplicationId { get; set; } = 0;
        public int? Rating { get; set; } = 0;

        public int? AverageRating { get; set; } = 0;

    }
}
