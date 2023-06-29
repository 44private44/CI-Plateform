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
    public class StoryListingRepository : Repository<Entities.MissionModel.StorylistingModel>, IStoryListingRepository
    {
        private readonly CiplatformContext _context;
        public StoryListingRepository(CiplatformContext context) : base(context)
        {
            _context = context;
        }

        public StorylistingModel GetAllStoryDetails(int UserId, int skipRows, int pageSize)
        {
            List<User> users = _context.Users.ToList();
            List<Country> countries = _context.Countries.ToList();
            List<City> cities = _context.Cities.ToList();
            List<MissionTheme> missionThemes = _context.MissionThemes.ToList();
            List<Skill> skills = _context.Skills.ToList();
            List<Mission> missions = _context.Missions.ToList();
            List<Story> stories = _context.Stories
                                 .Where(storystatus => storystatus.Status == "PUBLISHED" && storystatus.Flag == 1)
                                 .OrderByDescending(story => story.CreatedAt)
                                 .Skip(skipRows)
                                 .Take(pageSize)
                                 .ToList();

            List<StoryMedium> storyMedia = _context.StoryMedia.ToList();

            var storydata = new StorylistingModel
            {
                Users = users,
                Country = countries,
                City = cities,
                MissionThemes = missionThemes,
                Skill = skills,
                stories = stories,
            };
            var userdata = _context.Users.Where(u => u.UserId == UserId).ToList();
            storydata.Users = userdata;

            var countrydata = _context.Countries.ToList();
            storydata.Country = countrydata;

            var citiesdata = _context.Cities.ToList();
            storydata.City = citiesdata;

            var themedata = _context.MissionThemes.ToList();
            storydata.MissionThemes = themedata;

            var skillsdata = _context.Skills.ToList();
            storydata.Skill = skillsdata;

            return storydata;
        }
    }
}
























//var storiesdata = _context.Stories
//                   .Skip((pageNumber - 1) * pageSize)
//                   .Take(pageSize)
//                   .ToList();
//storydata.stories = storiesdata;


//storydata.stories = storiesdata;
//var storycard = _context.Stories.ToList();
//storydata.stories = storycard;