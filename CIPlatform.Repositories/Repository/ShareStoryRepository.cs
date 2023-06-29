using CIPlatform.Entities.DataModels;
using CIPlatform.Entities.MissionModel;
using CIPlatform.Repositories.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repositories.Repository
{
    public class ShareStoryRepository : Repository<Entities.MissionModel.SharestoryModel>, IShareStoryRepository
    {
        private readonly CiplatformContext _context;
        public ShareStoryRepository(CiplatformContext context) : base(context)
        {
            _context = context;
        }


        public SharestoryModel GetAllShareStoryData(int UserId)
        {
            List<User> users = _context.Users.ToList();
            List<MissionApplication> missionApplications = _context.MissionApplications.ToList();
            List<Mission> missions = _context.Missions.ToList();
            List<Story> stories = _context.Stories.ToList();
            var sharestorydata = new SharestoryModel
            {
                Users = users,
                missionApplications = missionApplications,
                missions = missions,
                stories = stories
            };
            var userdata = _context.Users.Where(u => u.UserId == UserId).ToList();
            sharestorydata.Users = userdata;

            var selectedstorymission = _context.MissionApplications.Where(m => m.ApprovalStatus == "APPROVE" && m.UserId == UserId).ToList();
            sharestorydata.missionApplications = selectedstorymission;

            var draftallstorydata = _context.Stories.Where(st => st.UserId == UserId).ToList();
            sharestorydata.stories = draftallstorydata;

            return sharestorydata;
        }


    }
}
