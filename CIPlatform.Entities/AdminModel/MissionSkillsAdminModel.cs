using CIPlatform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.AdminModel
{
    public class MissionSkillsAdminModel
    {
        [Key]

        public List<User> Users { get; set; } = new List<User>();

        public List<Skill> Skills { get; set; } = new List<Skill>();

        public string? SkillName { get; set; } = string.Empty;

        public byte Status { get; set; } = 1;
    }
}
