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
    public class MissionSkillsAdminRepository : Repository<Entities.AdminModel.MissionSkillsAdminModel>, IMissionSkillsAdminRepository
    {
        private readonly CiplatformContext _context;
        public MissionSkillsAdminRepository(CiplatformContext context) : base(context)
        {
            _context = context;
        }
        public MissionSkillsAdminModel GetAllMissionSkillsAdminData(int skipRows, int pageSize)
        {
            List<User> users = _context.Users.ToList();

            List<Skill> skills = _context.Skills.ToList();

            var SkillsAdminData = new MissionSkillsAdminModel
            {
                Users = users,
                Skills = skills,
            };

            var allSkillsData = _context.Skills.Where(skill => skill.Flag == 1).
                OrderByDescending(skill => skill.CreatedAt)
                                 .Skip(skipRows)
                                 .Take(pageSize).ToList();
            SkillsAdminData.Skills = allSkillsData;

            return SkillsAdminData;
        }

        // Skills validate
        public bool VlidateUserSkill(string skillname)
        {
            var validateskill = _context.Skills.Where(name => name.SkillName == skillname).FirstOrDefault();

            if (validateskill == null)
            {

                return true;
            }
            else
            {
                return false;
            }
        }

        // Add new Skill
        public bool AdminNewSkillData(MissionSkillsAdminModel data)
        {

            var addskill = new Skill
            {
                SkillName = data.SkillName,
                Status = data.Status,
            };
            _context.Skills.Add(addskill);
            _context.SaveChanges();

            addskill.Status = data.Status;
            _context.Skills.Update(addskill);
            _context.SaveChanges();
            return true;

        }

        // Update Skill data
        public bool AdminUpdateSkillData(MissionSkillsAdminModel data, long Skillid)
        {
            var skilldata = _context.Skills.Find(Skillid);

            if (skilldata != null)
            {
                skilldata.SkillName = data.SkillName;
                skilldata.Status = data.Status;

                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }


        // Soft delete data from database.
        public bool DeleteSkillRecords(long deleteSkillid)
        {
            var skilldeletedata = _context.Skills.Find(deleteSkillid);

            if (skilldeletedata != null)
            {
                skilldeletedata.Flag = 0;
                skilldeletedata.DeletedAt = DateTime.Now;
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
