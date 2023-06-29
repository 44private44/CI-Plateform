using CIPlatform.Entities.DataModels;
using CIPlatform.Entities.UserAccountAuthModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repositories.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        public LoginModel GetAllLoginData();
        public bool IsRegistered(User user);

        public bool IsPasswordmatch(User user);

        public bool IsAdminPasswordMatch(Admin admin);

        public bool RegisterEmailMatch(string email);

    }
}
