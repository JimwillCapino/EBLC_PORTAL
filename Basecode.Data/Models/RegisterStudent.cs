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
        [Display(Name = "first name")]
        [Required]
        [StringLength(50, ErrorMessage = "Must be at least {2} and at most {1} characters long.", MinimumLength = 1)]
        public string FirstName { get; set; }  
        public string? MiddleName { get; set; }
        [Display(Name = "last name")]
        [Required]
        [StringLength(50, ErrorMessage = "Must be at least {2} and at most {1} characters long.", MinimumLength = 1)]
        public string LastName { get; set;}
        [Display(Name = "birthday")]
        [Required]
        public DateTime Birthday { get; set; }
        [Display(Name = "birth certificate")]
        [Required]
        [RegularExpression(@"^.*\.pdf$", ErrorMessage = "Please upload a PDF file.")]
        public IFormFile? BirthCertificateFile { get; set; }    
        public byte[]? ProfilePic { get; set; }
        [Display(Name = "profile picture")]
        [Required]
        [RegularExpression(@"^.*\.jpg", ErrorMessage = "Please upload a JPG file.")]
        public IFormFile? ProfilePicRecieve { get; set; }
        [Display(Name = "sex")]
        [Required]
        public string sex { get; set; }
        [Display(Name = "grade to enroll")]
        [Required]
        public  string GradeEnrolled { get; set; }
        [Display(Name = "LRN")]
        [Required]
        public string? LRN { get; set; }
        [Display(Name = "Certificate of Good Moral")]       
        [RegularExpression(@"^.*\.pdf$", ErrorMessage = "Please upload a PDF file.")]
        public IFormFile? CGMFile { get; set; }
        [Display(Name = "TOR")]        
        [RegularExpression(@"^.*\.pdf$", ErrorMessage = "Please upload a PDF file.")]
        public IFormFile? TORFile { get; set; }
        public byte[]? BirthCertificateRecieve { get; set; }
        public byte[]? CGMRecieve { get; set; }
        public byte[]? TORRecieve { get; set; }
        public string? ExamSchedule { get; set; }
        [Display(Name = "parent first name")]
        [Required]
        [StringLength(50, ErrorMessage = "Must be at least {2} and at most {1} characters long.", MinimumLength = 1)]
        public string ParentFirstName { get; set; }
        
        public string? ParentMiddleName { get; set; }
        [Display(Name = "parent last name")]
        [Required]
        [StringLength(50, ErrorMessage = "Must be at least {2} and at most {1} characters long.", MinimumLength = 1)]
        public string ParentLastName { get; set; }  
        [Display(Name = "phone number")]
        [Required]
        [StringLength(11, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 11)]
        public string PhoneNumber { get; set; }
        [Display(Name = "address")]
        [Required]
        [StringLength(50, ErrorMessage = "Must be at least {2} and at most {1} characters long.", MinimumLength = 8)]
        public string Address { get; set; }
        [Display(Name = "email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "parent's birthday")]
        [Required]
        public DateTime ParentBirthday { get; set; }

        [Display(Name = "parent sex")]
        [Required]
        public string Parentsex { get; set; }                  
    }
}
