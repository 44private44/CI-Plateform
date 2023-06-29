using CIPlatform.Entities.DataModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.MissionModel
{
    public class SharestoryModel
    {
        [Key]

        public List<User> Users { get; set; }

        public List<MissionApplication> missionApplications { get; set; }

        public List<Mission> missions { get; set; }
        public List<Story> stories { get; set; }

        public int Id { get; set; }

        public int MissionId { get; set; }

        public string? StoryTitle { get; set; }

        public DateTime StoryDate { get; set; } 

        public string? Description { get; set; }

        public string? VideoURLs { get; set; }

        public string Type { get; set; } = null!;

        public List<IFormFile>? Images { get; set; }
    }
}
