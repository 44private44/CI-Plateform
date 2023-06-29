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
    public class MissionApplicationRepository : Repository<Entities.AdminModel.MissionApplicationAdminModel>, IMissionApplicationRepository
    {
        private readonly CiplatformContext _context;
        public MissionApplicationRepository(CiplatformContext context) : base(context)
        {
            _context = context;
        }
        public MissionApplicationAdminModel GetAllMissionApplicationAdminData(int skipRows, int pageSize)
        {
            List<User> users = _context.Users.ToList();

            List<MissionApplication> missionApplications = _context.MissionApplications.ToList();

            List<Mission> missions = _context.Missions.ToList();
            var allmissionAdminData = new MissionApplicationAdminModel
            {
                Users = users,
                MissionApplications = missionApplications,
                missions = missions,
            };

            var allApplicationData = _context.MissionApplications.
                OrderByDescending(app => app.CreatedAt)
                                 .Skip(skipRows)
                                 .Take(pageSize).ToList();
            allmissionAdminData.MissionApplications = allApplicationData;

            return allmissionAdminData;
        }
    }
}
