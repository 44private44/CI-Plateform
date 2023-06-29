using CIPlatform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.AdminModel
{
    public class CmsAdminModel
    {
        [Key]

        public List<User> Users { get; set; }

        public List<CmsPage> cmsPages { get; set; }

        public string? Title { get; set; } = string.Empty;

        public string? Description { get; set; }= string.Empty;

        public string Slug { get; set; } = null!;

        public int? Status { get; set; } = 1;
    }
}
