using CIPlatform.Entities.AdminModel;
using CIPlatform.Entities.DataModels;
using CIPlatform.Repositories.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repositories.Repository
{
    public class BannerAdminRepository : Repository<Entities.AdminModel.BannerAdminModel>, IBannerAdminRepository
    {
        private readonly CiplatformContext _context;
        public BannerAdminRepository(CiplatformContext context) : base(context)
        {
            _context = context;
        }
        public BannerAdminModel GetAllBannerAdminData(int skipRows, int pageSize)
        {
            List<User> users = _context.Users.ToList();
            List<Banner> banners = _context.Banners.ToList();

            var allBannerData = new BannerAdminModel
            {
                Users = users,
                banners = banners,
            };

            var BannerData = _context.Banners.Where(banner => banner.Flag == 1)
                                .OrderByDescending(banner => banner.CreatedAt)
                                .Skip(skipRows)
                                .Take(pageSize).ToList();
            allBannerData.banners = BannerData;

            return allBannerData;
        }


        // Add new Skill
        public bool AdminBannerAddData(BannerAdminModel data, string imageNames)
        {
            var addBanner = new Banner
            {
                Image = imageNames,
                Text = data.Text,
                SortOrder = data.SortOrder,
            };
            _context.Banners.Add(addBanner);
            _context.SaveChanges();

            return true;

        }

        // Update banner data
        public bool AdminUpdateBannerData(BannerAdminModel data, long Bannerid, string imagename)
        {
            var bannerdata = _context.Banners.Find(Bannerid);

            if (bannerdata != null)
            {
                bannerdata.Image = imagename;
                bannerdata.Text = data.Text;
                bannerdata.SortOrder = data.SortOrder;
                bannerdata.UpdatedAt = DateTime.Now;
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        // Soft delete data from database.
        public bool DeleteBannerRecords(long deleteBannerId)
        {
            var bannerdeletedata = _context.Banners.Find(deleteBannerId);

            if (bannerdeletedata != null)
            {
                bannerdeletedata.Flag = 0;
                bannerdeletedata.DeletedAt = DateTime.Now;
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
