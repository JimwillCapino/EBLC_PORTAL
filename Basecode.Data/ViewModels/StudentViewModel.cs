
using Microsoft.AspNetCore.Http;
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
        public IFormFile? ProfilePicRecieve { get; set; }
        public int Student_ID { get; set; }
        [Display(Name ="first name")]
        [Required(ErrorMessage = "First Name is Required.")]

        public string FirstName { get; set; }
        [Display(Name = "middle name")]
        [Required(ErrorMessage = "Middle Name is Required.")]
        public string MiddleName { get; set; }
        [Display(Name = "last name")]
        [Required(ErrorMessage = "Last Name is Required.")]
        public string LastName { get; set; }       
        public string Status { get; set; }
        [Display(Name = "birthday")]
        [Required(ErrorMessage = "Birhtdate is required.")]
        public DateTime Birthday { get; set; }
        [Display(Name = "sex")]
        [Required]
        public string sex { get; set; }
        public byte[]? profilePicture { get; set; }
        [Display(Name = "age")]
        [Required]
        public int age { get; set; }
        [Display(Name = "grade")]
        [Required]
        public string Grade { get; set; }
        [Display(Name = "LRN")]
        [Required(ErrorMessage = "LRN is Required.")]
        public string lrn { get; set; }          
    }
}
