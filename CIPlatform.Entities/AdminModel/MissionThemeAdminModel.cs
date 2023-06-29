using CIPlatform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.AdminModel
{
    public class MissionThemeAdminModel
    {
        [Key]

        public List<User> Users { get; set; } = new List<User>();

        public List<MissionTheme> MissionThemes { get; set; } = new List<MissionTheme>();

        public string? Title { get; set; } = string.Empty;
        public byte Status { get; set; } = 1;

    }
}
