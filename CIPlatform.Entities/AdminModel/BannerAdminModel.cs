using CIPlatform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.AdminModel
{
    public class BannerAdminModel
    {
        [Key]

        public List<User> Users { get; set; } = new List<User>();

        public List<Banner> banners { get; set; } = new List<Banner>();

        public string? Text { get; set; } = string.Empty;

        public int? SortOrder { get; set; }
    }
}
