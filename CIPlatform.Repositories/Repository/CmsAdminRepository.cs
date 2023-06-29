using CIPlatform.Entities.AdminModel;
using CIPlatform.Entities.DataModels;
using CIPlatform.Repositories.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace CIPlatform.Repositories.Repository
{
    public class CmsAdminRepository : Repository<Entities.AdminModel.CmsAdminModel>, ICmsAdminRepository
    {
        private readonly CiplatformContext _context;
        public CmsAdminRepository(CiplatformContext context) : base(context)
        {
            _context = context;
        }
        public CmsAdminModel GetAllCmsAdminData(int skipRows, int pageSize)
        {
            List<User> users = _context.Users.ToList();

            List<CmsPage> cmsPages = _context.CmsPages.ToList();

            var allcmsAdminData = new CmsAdminModel
            {
                Users = users,
                cmsPages = cmsPages,
            };

            var allCmsData = _context.CmsPages.Where(cms => cms.Flag == 1).
               OrderByDescending(user => user.CreatedAt)
                                .Skip(skipRows)
                                .Take(pageSize).ToList();
            allcmsAdminData.cmsPages = allCmsData;

            return allcmsAdminData;
        }

        // Cms Title validate
        public bool VlidateCmsTitle(string title)
        {
            var validateCmsTitle = _context.CmsPages.Where(cms => cms.Title == title).FirstOrDefault();

            if (validateCmsTitle == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Add new User
        public bool AdminNewCmsData(CmsAdminModel data)
        {
            var htmlString = data.Description;
            var decodedString = HttpUtility.HtmlDecode(htmlString); // convert to the string 
            var textContent = Regex.Replace(decodedString, @"<.*?>", string.Empty); // regular expression remove tag which matches
            data.Description = textContent;

            var addCms = new CmsPage
            {
                Title = data.Title,
                Description = data.Description,
                Slug = data.Slug,
                Status = data.Status,
            };
            _context.CmsPages.Add(addCms);
            _context.SaveChanges();
            return true;

        }

        // Update user data
        public bool AdminUpdateCmsData(CmsAdminModel data, long Cmsid)
        {
            var cmsdata = _context.CmsPages.Find(Cmsid);

            var htmlString = data.Description;
            var decodedString = HttpUtility.HtmlDecode(htmlString); // convert to the string 
            var textContent = Regex.Replace(decodedString, @"<.*?>", string.Empty); // regular expression remove tag which matches
            data.Description = textContent;

            if (cmsdata != null)
            {
                cmsdata.Title = data.Title;
                cmsdata.Description = data.Description;
                cmsdata.Slug = data.Slug;
                cmsdata.Status = data.Status;
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }


        // Soft delete data from database.
        public bool DeleteCmsRecords(long Cmsid)
        {
            var cmsdeletedata = _context.CmsPages.Find(Cmsid);

            if (cmsdeletedata != null)
            {
                cmsdeletedata.Flag = 0;
                cmsdeletedata.DeletedAt = DateTime.Now;
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
