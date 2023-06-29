using CIPlatform.Entities.DataModels;
using CIPlatform.Entities.UserRecordsModel;
using CIPlatform.Repositories.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repositories.Repository
{
    public class UserEditProfileRepository : Repository<Entities.UserRecordsModel.UserEditProfileModel>, IUserEditProfileRepository
    {
        private readonly CiplatformContext _context;
        public UserEditProfileRepository(CiplatformContext context) : base(context)
        {
            _context = context;
        }
        public UserEditProfileModel GetAllUserRecords(int UserId)
        {

            List<User> users = _context.Users.ToList();
            List<Country> countries = _context.Countries.ToList();
            List<City> cities = _context.Cities.ToList();
            List<Skill> skills = _context.Skills.ToList();
            List<UserSkill> userskills = _context.UserSkills.ToList();
            List<CmsPage> cmsPages = _context.CmsPages.ToList();    
            var userprofiledata = new UserEditProfileModel
            {
                Users = users,
                Country = countries,
                City = cities,
                Skill = skills,
                userSkills = userskills,
                CmsPage = cmsPages,
            };

            var userdata = _context.Users.Where(u => u.UserId == UserId).ToList();
            userprofiledata.Users = userdata;

            var allcountry = _context.Countries.ToList();
            userprofiledata.Country = allcountry;

            var allcities = _context.Cities.ToList();
            userprofiledata.City = allcities;

            var alluskills = _context.Skills.ToList();
            userprofiledata.Skill = alluskills;

            var alluserskills = _context.UserSkills.Where(skills => skills.UserId == UserId).ToList();
            userprofiledata.userSkills = alluserskills;

            return userprofiledata;

        }

        // contactus save data
        public void contactussavedata(int UserId, String subject, String message)
        {
            var contactusdata = new Contactu
            {
                UserId = UserId,
                Subject = subject,
                Message = message,
            };

            _context.Add(contactusdata);
            _context.SaveChanges();
        }

    }
}
