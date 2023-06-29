using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.UserAccountAuthModel
{
    public class RegisterModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter your First Name")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter your Last Name")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        public string PhoneNumber { get; set; } = string.Empty;
        public string? Mobileno { get; set; } = null;

        [Required(ErrorMessage = "Enter your Email")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Enter valid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter your Password")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "The password must be at least 8 characters long")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Re-enter your password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm Password doesn't match the Password")]
        public string ConPassword { get; set; }
    }
}
