using CIPlatform.Entities.AdminModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repositories.Repository.IRepository
{
    public interface IBannerAdminRepository : IRepository<BannerAdminModel>
    {
        public BannerAdminModel GetAllBannerAdminData(int skipRows, int pageSize);

        public bool AdminBannerAddData(BannerAdminModel data, string imageNames);

        public bool AdminUpdateBannerData(BannerAdminModel data, long Bannerid, string imagename);

        public bool DeleteBannerRecords(long deleteBannerId);
    }
}
