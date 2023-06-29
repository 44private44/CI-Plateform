using CIPlatform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.UserRecordsModel
{
    public class UserEditProfileModel
    {
        [Key]

        public List<User> Users { get; set; }

        public List<Country> Country { get; set; }

        public List<City> City { get; set; }
        public List<Skill> Skill { get; set; }

        public List<CmsPage> CmsPage { get; set; }
        public List<UserSkill> userSkills { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? WhyIVolunteer { get; set; }

        public string? EmployeeId { get; set; }

        public string? Department { get; set; }

        public long CityId { get; set; }

        public long CountryId { get; set; }

        public string? ProfileText { get; set; }

        public string? LinkedInUrl { get; set; }

        public string? Title { get; set; }

        public string? Manager { get; set; }

        public string? Availablity { get; set; }
    }
}
