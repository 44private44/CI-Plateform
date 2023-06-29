using CIPlatform.Entities.AdminModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repositories.Repository.IRepository
{
    public interface IMissionSkillsAdminRepository : IRepository<MissionSkillsAdminModel>
    {
        public MissionSkillsAdminModel GetAllMissionSkillsAdminData(int skipRows, int pageSize);

        public bool VlidateUserSkill(string skillname);

        public bool AdminNewSkillData(MissionSkillsAdminModel data);

        public bool AdminUpdateSkillData(MissionSkillsAdminModel data, long Skillid);
        public bool DeleteSkillRecords(long deleteSkillid);
    }
}
