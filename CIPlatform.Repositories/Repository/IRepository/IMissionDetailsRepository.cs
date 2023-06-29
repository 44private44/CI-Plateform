using CIPlatform.Entities.MissionModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repositories.Repository.IRepository
{
    public interface IMissionDetailsRepository : IRepository<MissiondetailsModel>
    {
        public MissiondetailsModel GetAllMissionDetails(int id, int UserId);
    }
}
