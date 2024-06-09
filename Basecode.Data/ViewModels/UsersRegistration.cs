using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class UsersRegistration
    {
        [Display(Name = "Password")]
        [Required(ErrorMessage = "The password field is required.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
     ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        [StringLength(100, ErrorMessage = "The password must be at least {2} and at most {1} characters long.", MinimumLength = 8)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "confrm password")]
        [JsonProperty(PropertyName = "confirm_pass")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required]
        [Display(Name = "email")]
        [JsonProperty(PropertyName = "email")]
        public string EmailAddress { get; set; }
    }
}
