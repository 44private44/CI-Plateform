using CIPlatform.Entities.MissionModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repositories.Repository.IRepository
{
    public interface ISPMissionDataRepository : IRepository<SPMissionModel>
    {
        public SPMissionModel GetAllMissionData();

        public List<SPDapperMissionData> GetAllMissionDataByDapper();

        public SPDapperOneMissionData OneMissionDataByDappaer(long MisionID);

        public List<DragDropMissionModel> GetAllmissionByDragDrop();

        public int UpdateFriendOrder(int id, int order);
    }
}
