using CIPlatform.Entities.AdminModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repositories.Repository.IRepository
{
    public interface IMissionThemeAdminRepository : IRepository<MissionThemeAdminModel>
    {
        public MissionThemeAdminModel GetAllMissionThemeAdminData(int skipRows, int pageSize);

        public bool VlidateUserTheme(string Themename);
        public bool AdminNewThemeData(MissionThemeAdminModel data);

        public bool AdminUpdateThemeData(MissionThemeAdminModel data, long Themeid);

        public bool DeleteThemeRecords(long deleteThemeid);
    }
}
