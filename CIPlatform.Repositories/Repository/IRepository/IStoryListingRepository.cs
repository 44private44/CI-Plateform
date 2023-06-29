using CIPlatform.Entities.MissionModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repositories.Repository.IRepository
{
    public interface IStoryListingRepository : IRepository<StorylistingModel>
    {
        public StorylistingModel GetAllStoryDetails(int UserId, int papageNumber, int pageSize);

    }
}
