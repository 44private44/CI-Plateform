using CIPlatform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.AdminModel
{
    public class UserAdminModel
    {
        [Key]

        public List<User> Users { get; set; }   
        public List<Admin> admins { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? WhyIVolunteer { get; set; }

        public string? EmployeeId { get; set; }

        public string? Department { get; set; }

        public long CityId { get; set; }

        public long CountryId { get; set; }

        public string? ProfileText { get; set; }

        public string? LinkedInUrl { get; set; }

        public int? Status { get; set; }

        public string? Title { get; set; }

        public string? Manager { get; set; }

        public string? Availablity { get; set; }

        public List<City> cities { get; set; } = null;

        public List<Country> country { get; set; } = null;
    }
}
