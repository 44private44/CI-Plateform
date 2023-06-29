using CIPlatform.Entities.UserRecordsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repositories.Repository.IRepository
{
    public interface IUserEditProfileRepository : IRepository<UserEditProfileModel>
    {
        public UserEditProfileModel GetAllUserRecords(int UserId);

        public void contactussavedata(int UserId, String subject, String message);
    }
}
