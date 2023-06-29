using CIPlatform.Entities.DataModels;
using CIPlatform.Entities.MissionModel;
using CIPlatform.Repositories.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repositories.Repository
{
    public class StoryDetailsRepository : Repository<Entities.MissionModel.StorydetailsModel>, IStoryDetailsRepository
    {
        private readonly CiplatformContext _context;
        public StoryDetailsRepository(CiplatformContext context) : base(context)
        {
            _context = context;
        }

        public StorydetailsModel GetOneStoryData(int UserId, int id)
        {
            List<User> users = _context.Users.ToList();
            List<Story> stories = new List<Story>();
            List<StoryMedium> storyMedia = new List<StoryMedium>();
            List<StoryInvite> storyInvites = new List<StoryInvite>();
            var storydata = new StorydetailsModel
            {
                // populate the properties of the StorydetailsModel object
                // with data from the User objects
                Users = users,
                stories = stories,
                storiesMedium = storyMedia,
                AllUsers = new List<AllUserdataModel>(),
                storiesInvite = storyInvites
            };

            var userdata = _context.Users.Where(u => u.UserId == UserId).ToList();
            storydata.Users = userdata;

            var storydetailsdata = _context.Stories
                                .Include(s => s.StoryMedia)
                                .Where(s => s.StoryId == id)
                                .ToList();
            storydata.stories = storydetailsdata;

            if (storydetailsdata.Count > 0)
            {
                var mediaPaths = storydetailsdata[0].StoryMedia.Select(m => m.Path).ToList();
                storydata.MediaPaths = mediaPaths;
            }

            var story = storydata.stories.FirstOrDefault(s => s.StoryId == id);
            if (story != null)
            {
                story.Views++;
                _context.SaveChanges();
            }

            var allUsers = _context.Users.ToList();
            foreach (var user in allUsers)
            {
                storydata.AllUsers.Add(new AllUserdataModel { User = user });
            }

            var storyinvitedata = _context.StoryInvites.ToList();
            storydata.storiesInvite = storyinvitedata;


            return storydata;
        }
    }

}
