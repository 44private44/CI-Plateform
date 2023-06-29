
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
    public class MissionDetailsRepository : Repository<Entities.MissionModel.MissiondetailsModel>, IMissionDetailsRepository
    {
        private readonly CiplatformContext _context;
        public MissionDetailsRepository(CiplatformContext context) : base(context)
        {
            _context = context;
        }

        public MissiondetailsModel GetAllMissionDetails(int id, int UserId)
        {
            List<Mission> mission = _context.Missions.ToList();
            List<MissionMedium> image = _context.MissionMedia.ToList();
            List<GoalMission> goal = _context.GoalMissions.ToList();
            List<MissionTheme> theme = _context.MissionThemes.ToList();
            List<Country> countries = _context.Countries.ToList();
            List<City> city = _context.Cities.ToList();
            List<Skill> skills = _context.Skills.ToList();
            List<MissionRating> rating = _context.MissionRatings.ToList();
            List<Comment> comment = _context.Comments.ToList();
            List<FavouriteMission> FavouriteMissions = _context.FavouriteMissions.ToList();
            List<User> userdata = _context.Users.ToList();
            List<MissionInvite> missionInvites = _context.MissionInvites.ToList();
            List<MissionApplication> missionApplications = _context.MissionApplications.ToList();
            List<MissionMedium> missionMedia = _context.MissionMedia.ToList();
            List<MissionRating> missionRatings = _context.MissionRatings.ToList();
            List<MissionDocument> missionDocuments = _context.MissionDocuments.ToList();
            var Missionsdetails = (from m in mission
                                   where m.MissionId.Equals(id)
                                   join g in goal on m.MissionId equals g.MissionId into data1
                                   from g in data1.DefaultIfEmpty().Take(1)

                                   join i in image on m.MissionId equals i.MissionId into data
                                   from i in data.DefaultIfEmpty().Take(1)

                                   join r in rating on m.MissionId equals r.MissionId into data2
                                   from r in data2.DefaultIfEmpty().Take(1)

                                   join c in comment on m.MissionId equals c.MissionId into data3
                                   from c in data3.DefaultIfEmpty().Take(1)

                                       //join fm in FavouriteMissions on m.MissionId equals fm.MissionId into data3
                                       //from fm in data3.DefaultIfEmpty().Take(1)

                                   select new MissiondetailsModel { comment = c, rating = r, image = i, Missions = m, Country = countries, themes = theme, skills = skills, goal = g, isvalid = _context.FavouriteMissions.Any(fm => fm.UserId == UserId && fm.MissionId == id) }).First();

            var relatedMissions = _context.Missions
            .Where(m => (m.City.Name == Missionsdetails.Missions.City.Name || m.Theme.Title == Missionsdetails.Missions.Theme.Title) && m.MissionId != id)
            .Select(m => new
            {
                Mission = m,
                MissionMedia = m.MissionMedia.FirstOrDefault() // assuming there's only one mission media per mission
            })

            .ToList();

            Missionsdetails.RelatedMissions = relatedMissions.Select(x => x.Mission).ToList();
            //Missionsdetails.RelatedMissions = relatedMissions.ToList();


            var commentdetails = _context.Comments.Where(c => c.MissionId == id).ToList();
            Missionsdetails.commentdetails = commentdetails.ToList();

            var userdata3 = userdata.ToList();
            Missionsdetails.Users = userdata3;

            var invitealldata = missionInvites.ToList();
            Missionsdetails.missionInvites = invitealldata;

            var recentvolunteers = _context.MissionApplications.Where(rv => rv.MissionId == id && rv.ApprovalStatus == "APPROVE").ToList();
            Missionsdetails.missionApplications = recentvolunteers;

            var userrating = _context.MissionRatings.Where(rv => rv.MissionId == id && rv.UserId == UserId).ToList();
            Missionsdetails.missionRatings = userrating.ToList();

            // mission documents

            var alldocuments = _context.MissionDocuments.Where(doc => doc.MissionId == id).ToList();
            Missionsdetails.missiondocuments = alldocuments;

            return Missionsdetails;
        }


    }
}
