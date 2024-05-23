using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class ProfileViewModel
    {
        public int UID { get; set; }
        public string AspUserId { get; set; }
        public int RTPCommonsId { get; set; }
        public int RTPUsersId { get; set; }
        public byte[]? ProfilePic { get; set; }
        public IFormFile? ProfilePicRecieve { get; set; }
        [Display(Name = "first Name")]
        [Required]
        [StringLength(50, ErrorMessage = "Must be at least {2} and at most {1} characters long.", MinimumLength = 1)]
        public string FirstName { get; set; }
        [Display(Name = "middle Name")]
        [StringLength(50, ErrorMessage = "Must be at least {2} and at most {1} characters long.", MinimumLength = 1)]
        public string MiddleName { get; set; }
        [Display(Name = "last Name")]
        [Required]
        [StringLength(50, ErrorMessage = " Must be at least {2} and at most {1} characters long.", MinimumLength = 1)]
        public string LastName { get; set; }
        [Display(Name = "birthday")]
        [Required]
        public DateTime Birthday { get; set; }
        [Display(Name = "sex")]
        [Required]
        public string sex { get; set; }        
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email Address")]
        public string Email { get; set; }
        [Display(Name = "adrress")]
        [Required]
        [StringLength(50, ErrorMessage = "Must be at least {2} and at most {1} characters long.")]
        public string Address { get; set; }
        [Display(Name = "phone number")]
        [Required]
        [RegularExpression(@"^0\d{9,14}$", ErrorMessage = "Please enter a valid phone number starting with zero.")]
        public string PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "The password field is required.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
     ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        [StringLength(100, ErrorMessage = "The password must be at least {2} and at most {1} characters long.", MinimumLength = 8)]
        public string NewPassword { get; set; }
    }
}
