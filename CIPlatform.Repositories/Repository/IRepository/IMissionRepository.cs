using CIPlatform.Entities.MissionModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repositories.Repository.IRepository
{
    public interface IMissionRepository : IRepository<FilterMission>
    {
        public List<FilterMission> GetAllMissions(int UserId, int papageNumber, int pageSize); //int pageIndex
    }
}
