using CIPlatform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.AdminModel
{
    public class MissionAdminModel
    {
        [Key]

        public List<User> Users { get; set; } = new List<User>();

        public List<Mission> Missions { get; set; } = new List<Mission>();

        public List<City> Citys { get; set; } = new List<City>();
        public List<Country> Country { get; set; } = new List<Country>();

        public List<GoalMission> Goals { get; set; } = new List<GoalMission>();

        public List<MissionMedium> MissionMediums { get; set;} = new List<MissionMedium>();

        public List<MissionSkill> MissionSkills { get; set;} = new List<MissionSkill>();
        public List<MissionTheme> MissionThemes { get; set;} = new List<MissionTheme>();

        public List<Skill> skills { get; set; } = new List<Skill>();

        public string Title { get; set; } = null!;

        public string? ShortDescription { get; set; }

        public string? Description { get; set; }
        public long CityId { get; set; }
        public long CountryId { get; set; }
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string MissionType { get; set; } = null!;

        public string Status { get; set; } = null!;

        public string? OrganizationName { get; set; }

        public string? OrganizationDetail { get; set; }

        public string Availability { get; set; } = null!;
        public string? SeatLeft { get; set; }

        public DateTime? Deadline { get; set; }

        public string? GoalObjectiveText { get; set; }

        public int GoalValue { get; set; }
        public long MissionThemeId { get; set; }

    }
}
