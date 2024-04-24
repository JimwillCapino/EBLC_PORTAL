
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class StudentViewModel
    {
        public int Student_ID { get; set; }
        [Required(ErrorMessage = "First Name is Required.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "First Name is Required.")]
        public string MiddleName { get; set; }
        [Required(ErrorMessage = "Middle Name is Required.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Last Name is Required.")]
        public string Status { get; set; }
        [Required(ErrorMessage = "Status is required")]
        public DateTime Birthday { get; set; }
        [Required(ErrorMessage = "Birhtdate is required.")]
        public string sex { get; set; }
        public byte[]? profilePicture { get; set; }
        [Required(ErrorMessage = "First Name is Required.")]
        public int age { get; set; }
        [Required(ErrorMessage = "grade Level Name is Required.")]
        public int Grade { get; set; }
        [Required(ErrorMessage = "LRN is Required.")]
        public string lrn { get; set; }
    }
}
