using CIPlatform.Entities.AdminModel;
using CIPlatform.Entities.DataModels;
using CIPlatform.Repositories.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repositories.Repository
{
    public class UserAdminRepository : Repository<Entities.AdminModel.UserAdminModel>, IUserAdminRepository
    {
        private readonly CiplatformContext _context;
        public UserAdminRepository(CiplatformContext context) : base(context)
        {
            _context = context;
        }
        public UserAdminModel GetAllUserAdminData(int skipRows, int pageSize)
        {
            List<User> users = _context.Users.ToList();
            List<City> cities = _context.Cities.ToList();
            List<Country> countries = _context.Countries.ToList();
            var allUserAdminData = new UserAdminModel
            {
                Users = users,
                cities = cities,
                country = countries,
            };

            var allUserData = _context.Users.Where(user => user.Flag == 1).
                OrderByDescending(user => user.CreatedAt)
                                 .Skip(skipRows)
                                 .Take(pageSize).ToList();
            allUserAdminData.Users = allUserData;


            var allcities = _context.Cities.ToList();
            allUserAdminData.cities = allcities;

            var allcountries = _context.Countries.ToList();
            allUserAdminData.country = allcountries;

            return allUserAdminData;
        }

        // email validate
        public bool VlidateUserEmail(string email)
        {
            var validateUserEmail = _context.Users.Where(userEmail => userEmail.Email == email).FirstOrDefault();

            if (validateUserEmail == null)
            {

                return true;
            }
            else
            {
                return false;
            }
        }

        // Add new User
        public bool AdminNewUserData(UserAdminModel data)
        {

            var adduser = new User
            {
                FirstName = data.FirstName,
                LastName = data.LastName,
                Email = data.Email,
                Password = data.Password,
                EmployeeId = data.EmployeeId,
                Avatar = "IMAGES/CI Platform Assets/user1.png",
                Manager = data.Manager,
                Title = data.Title,
                Department = data.Department,
                ProfileText = data.ProfileText,
                WhyIVolunteer = data.WhyIVolunteer,
                CityId = data.CityId,
                CountryId = data.CountryId,
                Availablity = data.Availablity,
                LinkedInUrl = data.LinkedInUrl,
                Status = data.Status,
            };
            _context.Users.Add(adduser);
            _context.SaveChanges();
            return true;

        }

        // Update user data
        public bool AdminUpdateUserData(UserAdminModel data, long Userid)
        {
            var userdata = _context.Users.Find(Userid);

            if (userdata != null)
            {
                userdata.FirstName = data.FirstName;
                userdata.LastName = data.LastName;
                userdata.EmployeeId = data.EmployeeId;
                userdata.WhyIVolunteer = data.WhyIVolunteer;
                userdata.Department = data.Department;
                userdata.ProfileText = data.ProfileText;
                userdata.CityId = data.CityId;
                userdata.CountryId = data.CountryId;
                userdata.Title = data.Title;
                userdata.Manager = data.Manager;
                userdata.Availablity = data.Availablity;
                userdata.LinkedInUrl = data.LinkedInUrl;
                userdata.Status = data.Status;

                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        // Soft delete data from database.
        public bool DeleteUserRecords(long deleteUserid)
        {
            var userdeletedata = _context.Users.Find(deleteUserid);

            if (userdeletedata != null)
            {
                userdeletedata.Flag = 0;
                userdeletedata.DeletedAt = DateTime.Now;
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
