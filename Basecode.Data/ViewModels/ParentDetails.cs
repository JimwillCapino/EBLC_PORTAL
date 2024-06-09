using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class ParentDetails
    {
        public int UID { get; set; }
        public int ParentId { get ; set; } 
        public int RTPCommonsId { get; set; }
        [Display(Name ="first name")]
        [Required(ErrorMessage = "First Name is Required.")]
        public string ParentFirstName { get; set; }
        [Display(Name = "middle name")]
        [Required(ErrorMessage = "Middle Name is Required.")]
        public string ParentMiddleName { get; set; }
        [Display(Name = "last name")]
        [Required(ErrorMessage = "Last Name is Required.")]
        public string ParentLastName { get; set; }
        [Display(Name = "phone number")]
        [Required(ErrorMessage = "Phone number is Required.")]
        [StringLength(11)]
        public string PhoneNumber { get; set; }
        [Display(Name = "address")]
        [Required(ErrorMessage = "Address is Required.")]
        public string Address { get; set; }
        [Display(Name = "email")]
        [Required(ErrorMessage = "email is Required.")]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "birthday")]
        [Required(ErrorMessage = "birthday is Required.")]
        public DateTime ParentBirthday { get; set; }
        [Display(Name = "sex")]
        [Required(ErrorMessage = "gender is Required.")]
        public string Parentsex { get; set; }
    }
}
