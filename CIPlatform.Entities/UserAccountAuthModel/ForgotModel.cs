using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.UserAccountAuthModel
{
    public class ForgotModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Please Enter Valid Email Address.")]
        public string Email { get; set; }
    }
}
