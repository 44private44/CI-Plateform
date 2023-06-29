using CIPlatform.Entities.AdminModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repositories.Repository.IRepository
{
    public interface IUserAdminRepository : IRepository<UserAdminModel>
    {
        public UserAdminModel GetAllUserAdminData(int skipRows, int pageSize);

        public bool VlidateUserEmail(string email);
        public bool AdminNewUserData(UserAdminModel data);

        public bool AdminUpdateUserData(UserAdminModel data, long Userid);
        public bool DeleteUserRecords(long deleteUserid);
    }
}
