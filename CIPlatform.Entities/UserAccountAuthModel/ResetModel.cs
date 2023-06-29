using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.UserAccountAuthModel
{
    public class ResetModel
    {
        [Required]
        public string Password { get; set; } = null!;

        [Required]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match.")]
        public string ConfirmPassword { get; set; }


        public string Email { get; set; }
        public string Token { get; set; }
    }
}
