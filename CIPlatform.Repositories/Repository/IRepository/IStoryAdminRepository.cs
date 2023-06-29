using CIPlatform.Entities.AdminModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repositories.Repository.IRepository
{
    public interface IStoryAdminRepository : IRepository<StoryAdminModel>
    {
        public StoryAdminModel GetAllStoryAdminData(int skipRows, int pageSize);

        public bool DeleteStoryRecords(long deleteStoryid);
    }
}
