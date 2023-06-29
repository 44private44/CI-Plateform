using CIPlatform.Entities.MissionModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repositories.Repository.IRepository
{
    public interface IStoryDetailsRepository : IRepository<StorydetailsModel>
    {
        public StorydetailsModel GetOneStoryData(int UserId, int storyid);
    }
}
