
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
    public class MissionRepository : Repository<Entities.MissionModel.FilterMission>, IMissionRepository
    {
        private readonly CiplatformContext _context;
        public MissionRepository(CiplatformContext context) : base(context)
        {
            _context = context;
        }

        public List<FilterMission> GetAllMissions(int UserId, int skipRows, int pageSize)//int pageIndex
        {
            List<Mission> mission = _context.Missions.ToList();
            List<MissionMedium> image = _context.MissionMedia.ToList();
            List<GoalMission> goal = _context.GoalMissions.ToList();
            List<MissionTheme> theme = _context.MissionThemes.ToList();
            List<Country> countries = _context.Countries.ToList();
            List<City> city = _context.Cities.ToList();
            List<Skill> skills = _context.Skills.ToList();
            List<MissionRating> rating = _context.MissionRatings.ToList();
            List<MissionRating> favmission = _context.MissionRatings.ToList();
            List<User> users = _context.Users.ToList();
            List<MissionApplication> missionApplications = _context.MissionApplications.ToList();
            var Missions = (from m in mission

                            join g in goal on m.MissionId equals g.MissionId into data1
                            from g in data1.DefaultIfEmpty().Take(1)

                            join i in image on m.MissionId equals i.MissionId into data
                            from i in data.DefaultIfEmpty().Take(1)

                            join r in rating on m.MissionId equals r.MissionId into data2
                            from r in data2.DefaultIfEmpty().Take(1)

                            select new FilterMission
                            {
                                rating = r,
                                image = i,
                                Missions = m,
                                Country = countries,
                                themes = theme,
                                skills = skills,
                                missionApplications = missionApplications,
                                goal = g,
                                isvalid = _context.FavouriteMissions.Any(fm => fm.UserId == UserId && fm.MissionId == m.MissionId)
                            }).OrderByDescending(m => m.Missions.CreatedAt).Skip(skipRows).Take(pageSize).ToList();

            return Missions;
        }
    }
}
