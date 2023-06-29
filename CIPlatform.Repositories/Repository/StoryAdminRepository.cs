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
    public class StoryAdminRepository : Repository<Entities.AdminModel.StoryAdminModel>, IStoryAdminRepository
    {
        private readonly CiplatformContext _context;
        public StoryAdminRepository(CiplatformContext context) : base(context)
        {
            _context = context;
        }
        public StoryAdminModel GetAllStoryAdminData(int skipRows, int pageSize)
        {
            List<User> users = _context.Users.ToList();
            List<Story> stories = _context.Stories.ToList();
            List<Mission> missions = _context.Missions.ToList();

            var allcmsAdminData = new StoryAdminModel
            {
                Users = users,
                Stories = stories,
                Missions = missions,
            };

            var StoryData = _context.Stories.Where(story => story.Flag == 1).
               OrderByDescending(story => story.CreatedAt)
                                .Skip(skipRows)
                                .Take(pageSize).ToList();
            allcmsAdminData.Stories = StoryData;

            return allcmsAdminData;
        }
        // Soft delete data from database.
        public bool DeleteStoryRecords(long deleteStoryid)
        {
            var storydeletedata = _context.Stories.Find(deleteStoryid);

            if (storydeletedata != null)
            {
                storydeletedata.Flag = 0;
                storydeletedata.DeletedAt = DateTime.Now;
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
