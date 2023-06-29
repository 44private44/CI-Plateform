using CIPlatform.Entities.AdminModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repositories.Repository.IRepository
{
    public interface IMissionApplicationRepository : IRepository<MissionApplicationAdminModel>
    {
        public MissionApplicationAdminModel GetAllMissionApplicationAdminData(int skipRows, int pageSize);
    }
}
