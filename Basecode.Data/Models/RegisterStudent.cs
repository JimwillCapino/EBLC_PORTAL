using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Models
{
    public class RegisterStudent
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "First Name is Required.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Middle Name is Required.")]
        public string MiddleName { get; set; }
        [Required(ErrorMessage = "Last Name is Required.")]
        public string LastName { get; set;}
        [Required(ErrorMessage = "First Name is Required.")]
        public DateTime Birthday { get; set; }
        public IFormFile? BirthCertificateFile { get; set; }
        public byte[]? ProfilePic { get; set; }
        public IFormFile? ProfilePicRecieve { get; set; }
        [Required(ErrorMessage = "gender is Required.")]
        public string sex { get; set; }
        [Required(ErrorMessage = "grade is Required.")]
        public  int GradeEnrolled { get; set; }        
        public string? LRN { get; set; }
        public IFormFile? CGMFile { get; set; }
        public IFormFile? TORFile { get; set; }
        public byte[]? BirthCertificateRecieve { get; set; }
        public byte[]? CGMRecieve { get; set; }
        public byte[]? TORRecieve { get; set; }
        public DateTime? ExamSchedule { get; set; }
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
