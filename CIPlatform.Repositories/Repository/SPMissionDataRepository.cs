using CIPlatform.Entities.DataModels;
using CIPlatform.Entities.MissionModel;
using CIPlatform.Repositories.Repository.IRepository;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repositories.Repository
{
    public class SPMissionDataRepository : Repository<Entities.MissionModel.SPMissionModel>, ISPMissionDataRepository
    {
        private readonly CiplatformContext _context;
        private readonly string connectionString = "Server=PCA172\\SQL2017;Database=CIPlatform;Trusted_Connection=True;MultipleActiveResultSets=true;User ID=sa;Password=Tatva@123;Integrated Security=False; TrustServerCertificate=True";

        public SPMissionDataRepository(CiplatformContext context) : base(context)
        {
            _context = context;
        }

        public SPMissionModel GetAllMissionData()
        {
            List<Mission> missions = _context.Missions.ToList();

            var allmissionAdminData = new SPMissionModel
            {
                Missions = missions,
            };

            return allmissionAdminData;
        }

        // Get all mission data by Dapper
        public List<SPDapperMissionData> GetAllMissionDataByDapper()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Execute the stored procedure and map the results to models using Dapper
                List<SPDapperMissionData> missionDataList = connection.Query<SPDapperMissionData>("MissionDataByDapper", commandType: CommandType.StoredProcedure).ToList();

                // Return the data to the view
                return missionDataList;
            }
        }

        // Dapper get one misison data by Dapper 
        SPDapperOneMissionData ISPMissionDataRepository.OneMissionDataByDappaer(long MisionID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new { missionID = MisionID }; // Create parameter object
                SPDapperOneMissionData onemissiondata = connection.QueryFirstOrDefault<SPDapperOneMissionData>("OneMissionDataByDapper", parameters, commandType: CommandType.StoredProcedure);
                return onemissiondata;
            }
        }

        // get all mission by drag and drop
 
        public List<DragDropMissionModel> GetAllmissionByDragDrop()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Execute the stored procedure and map the results to models using Dapper
                List<DragDropMissionModel> missionDataList = connection.Query<DragDropMissionModel>("DragdropSortMission", commandType: CommandType.StoredProcedure).ToList();

                // Return the data to the view
                return missionDataList;
            }
        }

        // Drag Drop Soft

        public int UpdateFriendOrder(int id, int order)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    var parameters = new
                    {
                        missionId = id,
                        newOrder = order
                    };
                    List<DragDropMissionModel> friendDataList = connection.Query<DragDropMissionModel>("UpdateMissionOrder", parameters, commandType: CommandType.StoredProcedure).ToList();
                }
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
