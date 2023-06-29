using CIPlatform.Entities.AdminModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repositories.Repository.IRepository
{
    public interface ICmsAdminRepository : IRepository<CmsAdminModel>
    {
        public CmsAdminModel GetAllCmsAdminData(int skipRows, int pageSize);

        public bool VlidateCmsTitle(string title);
        public bool AdminNewCmsData(CmsAdminModel data);

        public bool AdminUpdateCmsData(CmsAdminModel data, long Cmsid);

        public bool DeleteCmsRecords(long Cmsid);
    }
}
