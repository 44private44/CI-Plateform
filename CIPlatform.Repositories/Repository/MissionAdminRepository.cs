using CIPlatform.Entities.AdminModel;
using CIPlatform.Entities.DataModels;
using CIPlatform.Repositories.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace CIPlatform.Repositories.Repository
{
    public class MissionAdminRepository : Repository<Entities.AdminModel.MissionAdminModel>, IMissionAdminRepository
    {
        private readonly CiplatformContext _context;
        public MissionAdminRepository(CiplatformContext context) : base(context)
        {
            _context = context;
        }
        public MissionAdminModel GetAllMissionAdminData(int skipRows, int pageSize)
        {
            List<User> users = _context.Users.ToList();

            List<Mission> missions = _context.Missions.ToList();
            List<Country> countries = _context.Countries.ToList();
            List<MissionTheme> missionThemes = _context.MissionThemes.ToList();
            List<Skill> skills = _context.Skills.ToList();
            var allmissionAdminData = new MissionAdminModel
            {
                Users = users,
                Missions = missions,
                skills = skills,
            };

            var allMisionsData = _context.Missions.Where(mission => mission.Flag == 1).
               OrderByDescending(mission => mission.CreatedAt)
                                .Skip(skipRows)
                                .Take(pageSize).ToList();
            allmissionAdminData.Missions = allMisionsData;

            var allCountries = _context.Countries.ToList();
            allmissionAdminData.Country = allCountries;

            var allMissionThemes = _context.MissionThemes.ToList();
            allmissionAdminData.MissionThemes = allMissionThemes;

            return allmissionAdminData;
        }

        // Add new Mission
        public bool AddMissionData(MissionAdminModel data, string allSkills, string imageNames)
        {

            var imageNamesArray = imageNames.Split(",");

            var description = data.Description;
            var decodeDescription = HttpUtility.HtmlDecode(description); // convert to the string 
            var textDescription = Regex.Replace(decodeDescription, @"<.*?>", string.Empty); // regular expression remove tag which matches
            data.Description = textDescription;

            var orgadetails = data.OrganizationDetail;
            var decodedDetails = HttpUtility.HtmlDecode(orgadetails); // convert to the string 
            var textDetails = Regex.Replace(decodedDetails, @"<.*?>", string.Empty); // regular expression remove tag which matches
            data.OrganizationDetail = textDetails;

            var addmission = new Mission
            {
                Title = data.Title,
                ThemeId = data.MissionThemeId,
                CityId = data.CityId,
                CountryId = data.CountryId,
                ShortDescription = data.ShortDescription,
                Description = data.Description,
                OrganizationName = data.OrganizationName,
                OrganizationDetail = data.OrganizationDetail,
                MissionType = data.MissionType,
                StartDate = data.StartDate,
                EndDate = data.EndDate,
                SeatLeft = data.SeatLeft,
                Deadline = data.Deadline,
                Availability = data.Availability,
                Status = data.Status,
            };
            _context.Missions.Add(addmission);
            _context.SaveChanges();

            if (allSkills != null)
            {
                var allSkillsArray = allSkills.Split(",");
                // skill add
                for (int i = 0; i < allSkillsArray.Length; i++)
                {
                    var misionSkills = new MissionSkill
                    {
                        MissionId = addmission.MissionId,
                        SkillId = long.Parse(allSkillsArray[i])
                    };
                    _context.MissionSkills.Add(misionSkills);
                    _context.SaveChanges();
                }
            }


            //image add
            for (int i = 0; i < imageNamesArray.Length; i++)
            {
                var missonImages = new MissionMedium
                {
                    MissionId = addmission.MissionId,
                    MediaPath = $"IMAGES/CI Platform Assets/{imageNamesArray[i]}",

                };
                _context.MissionMedia.Add(missonImages);
                _context.SaveChanges();
            }

            if (data.MissionType == "Goal")
            {
                var goaldata = new GoalMission
                {
                    MissionId = addmission.MissionId,
                    GoalObjectiveText = data.GoalObjectiveText,
                    GoalValue = data.GoalValue,
                };
                _context.GoalMissions.Add(goaldata);
                _context.SaveChanges();
            }
            return true;

        }


        // Update mission data
        public bool AdminUpdateMissionData(MissionAdminModel data, long Missionid, string allSkills)
        {
            if (allSkills != null)
            {
                var allSkillsArray = allSkills.Split(",");
                var missionSkills = _context.MissionSkills.Where(skill => skill.MissionId == Missionid).ToList();

                //detete previous skills
                foreach (var skill in missionSkills)
                {
                    var missionSkillid = _context.MissionSkills.Find(skill.MissionSkillId);
                    if (missionSkillid != null)
                    {
                        _context.MissionSkills.Remove(missionSkillid);
                        _context.SaveChanges();
                    }
                }

                //add update skills
                for (int i = 0; i < allSkillsArray.Length; i++)
                {
                    var misionSkills = new MissionSkill
                    {
                        MissionId = Missionid,
                        SkillId = long.Parse(allSkillsArray[i])
                    };
                    _context.MissionSkills.Add(misionSkills);
                    _context.SaveChanges();
                }
            }
            else
            {
                var missionSkills = _context.MissionSkills.Where(skill => skill.MissionId == Missionid).ToList();

                //detete previous skills
                foreach (var skill in missionSkills)
                {
                    var missionSkillid = _context.MissionSkills.Find(skill.MissionSkillId);
                    if (missionSkillid != null)
                    {
                        _context.MissionSkills.Remove(missionSkillid);
                        _context.SaveChanges();
                    }
                }
            }
            var missiondata = _context.Missions.Find(Missionid);
            var goaldata = _context.GoalMissions.Where(goal => goal.MissionId == Missionid).FirstOrDefault();

            var description = data.Description;
            var decodeDescription = HttpUtility.HtmlDecode(description); // convert to the string 
            var textDescription = Regex.Replace(decodeDescription, @"<.*?>", string.Empty); // regular expression remove tag which matches
            data.Description = textDescription;

            var orgadetails = data.OrganizationDetail;
            var decodedDetails = HttpUtility.HtmlDecode(orgadetails); // convert to the string 
            var textDetails = Regex.Replace(decodedDetails, @"<.*?>", string.Empty); // regular expression remove tag which matches
            data.OrganizationDetail = textDetails;

            if (data.MissionType == "Goal")
            {
                goaldata.GoalObjectiveText = data.GoalObjectiveText;
                goaldata.GoalValue = data.GoalValue;
                _context.SaveChanges();
            }

            if (missiondata != null)
            {
                missiondata.Title = data.Title;
                missiondata.ShortDescription = data.ShortDescription;
                missiondata.Description = data.Description;
                if (data.CityId != 0 && data.CountryId != 0)
                {
                    missiondata.CityId = data.CityId;
                    missiondata.CountryId = data.CountryId;
                }
                missiondata.OrganizationName = data.OrganizationName;
                missiondata.OrganizationDetail = data.OrganizationDetail;
                missiondata.MissionType = data.MissionType;
                missiondata.Deadline = data.Deadline;
                missiondata.StartDate = data.StartDate;
                missiondata.EndDate = data.EndDate;
                missiondata.SeatLeft = data.SeatLeft;
                if (data.MissionThemeId != 0)
                {
                    missiondata.ThemeId = data.MissionThemeId;
                }
                if (data.Availability != "default")
                {
                    missiondata.Availability = data.Availability;
                }
                missiondata.Status = data.Status;
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }


        // Soft delete data from database.
        public bool DeleteMissionRecords(long Missionid)
        {
            var missiondeletedata = _context.Missions.Find(Missionid);

            if (missiondeletedata != null)
            {
                missiondeletedata.Flag = 0;
                missiondeletedata.DeletedAt = DateTime.Now;
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
