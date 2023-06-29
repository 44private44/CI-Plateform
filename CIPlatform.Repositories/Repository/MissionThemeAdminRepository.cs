using CIPlatform.Entities.AdminModel;
using CIPlatform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repositories.Repository.IRepository
{
    public class MissionThemeAdminRepository : Repository<Entities.AdminModel.MissionThemeAdminModel>, IMissionThemeAdminRepository
    {
        private readonly CiplatformContext _context;
        public MissionThemeAdminRepository(CiplatformContext context) : base(context)
        {
            _context = context;
        }
        public MissionThemeAdminModel GetAllMissionThemeAdminData(int skipRows, int pageSize)
        {
            List<User> users = _context.Users.ToList();

            List<MissionTheme> missionThemes = _context.MissionThemes.ToList();

            var allmissionthemeAdminData = new MissionThemeAdminModel
            {
                Users = users,
                MissionThemes = missionThemes
            };

            var allThemesData = _context.MissionThemes.Where(theme => theme.Flag == 1).
                OrderByDescending(theme => theme.CreatedAt)
                                 .Skip(skipRows)
                                 .Take(pageSize).ToList();
            allmissionthemeAdminData.MissionThemes = allThemesData;

            return allmissionthemeAdminData;
        }

        // Theme validate
        public bool VlidateUserTheme(string Themename)
        {
            var validatetheme = _context.MissionThemes.Where(name => name.Title == Themename).FirstOrDefault();

            if (validatetheme == null)
            {

                return true;
            }
            else
            {
                return false;
            }
        }

        // Add new Theme
        public bool AdminNewThemeData(MissionThemeAdminModel data)
        {

            var addtheme = new MissionTheme
            {
                Title = data.Title,
                Status = data.Status,
            };
            _context.MissionThemes.Add(addtheme);
            _context.SaveChanges();

            addtheme.Status = data.Status;
            _context.MissionThemes.Update(addtheme);
            _context.SaveChanges();
            return true;

        }

        // Update Theme data
        public bool AdminUpdateThemeData(MissionThemeAdminModel data, long Themeid)
        {
            var themedata = _context.MissionThemes.Find(Themeid);

            if (themedata != null)
            {
                themedata.Title = data.Title;
                themedata.Status = data.Status;

                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        // Soft delete data from database.
        public bool DeleteThemeRecords(long deleteThemeid)
        {
            var themedeletedata = _context.MissionThemes.Find(deleteThemeid);

            if (themedeletedata != null)
            {
                themedeletedata.Flag = 0;
                themedeletedata.DeletedAt = DateTime.Now;
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
