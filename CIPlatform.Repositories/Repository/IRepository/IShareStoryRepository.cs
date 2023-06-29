using CIPlatform.Entities.MissionModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repositories.Repository.IRepository
{
    public interface IShareStoryRepository : IRepository<SharestoryModel>
    {
        public SharestoryModel GetAllShareStoryData(int UserId);

    }
}
