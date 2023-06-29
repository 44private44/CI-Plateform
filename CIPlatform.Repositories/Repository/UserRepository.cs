
using CIPlatform.Entities.DataModels;
using CIPlatform.Entities.UserAccountAuthModel;
using CIPlatform.Repositories.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repositories.Repository
{
    public class UserRepository : Repository<Entities.UserAccountAuthModel.LoginModel>, IUserRepository
    {
        private CiplatformContext _context;
        public UserRepository(CiplatformContext context) : base(context)
        {
            _context = context;
        }

        public LoginModel GetAllLoginData()
        {
            List<User> users = _context.Users.ToList();
            List<Banner> banners = _context.Banners.ToList();

            var allLoginData = new LoginModel
            {
                banners = banners,
            };

            var bannedata = _context.Banners.OrderBy(banner => banner.SortOrder).ToList();
            allLoginData.banners = bannedata;

            return allLoginData;
        }


        public bool IsRegistered(User user)
        {
            return _context.Users.Any(a => (a.Email == user.Email));
        }

        public bool IsPasswordmatch(User user)
        {
            return _context.Users.Any(a => (a.Email == user.Email) && (a.Password == user.Password));
        }
        public bool IsAdminPasswordMatch(Admin admin)
        {
            return _context.Admins.Any(admindata => (admindata.Email == admin.Email) && (admindata.Password == admin.Password));
        }

        public void Add(User entity)
        {
            _context.Add(entity);
        }

        public bool RegisterEmailMatch(string email)
        {
            var emailmatch = _context.Users.Where(user => user.Email == email).FirstOrDefault();

            if(emailmatch != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
