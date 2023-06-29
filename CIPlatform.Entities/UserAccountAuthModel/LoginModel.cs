using CIPlatform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.UserAccountAuthModel
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Enter Your Email")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Invalid Email")]
        public string email { get; set; }

        [Required(ErrorMessage = "Enter correct Password")]
        public string password { get; set; }

        public List<Banner> banners { get; set; } = new List<Banner>();
    }
}
