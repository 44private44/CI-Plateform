using CIPlatform.Entities.AdminModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repositories.Repository.IRepository
{
    public interface IMissionAdminRepository : IRepository<MissionAdminModel>
    {
        public MissionAdminModel GetAllMissionAdminData(int skipRows, int pageSize);

        public bool AddMissionData(MissionAdminModel data, string allSkills, string imageNames);

        public bool AdminUpdateMissionData(MissionAdminModel data, long Missionid, string allSkills);

        public bool DeleteMissionRecords(long Missionid);
    }
}
