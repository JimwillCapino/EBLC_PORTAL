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
        [Required(ErrorMessage = "First Name is Required.")]
        public string ParentFirstName { get; set; }
        [Required(ErrorMessage = "Middle Name is Required.")]
        public string ParentMiddleName { get; set; }
        [Required(ErrorMessage = "Last Name is Required.")]
        public string ParentLastName { get; set; }
        [Required(ErrorMessage = "Phone number is Required.")]
        [StringLength(11)]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Address is Required.")]
        public string Address { get; set; }
        [Required(ErrorMessage = "email is Required.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Birthday is Required.")]
        public DateTime ParentBirthday { get; set; }
        [Required(ErrorMessage = "gender is Required.")]
        public string Parentsex { get; set; }
    }
}
