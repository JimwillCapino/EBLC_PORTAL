using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Models
{
    public class RegisterStudent
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set;}
        public DateTime Birthday { get; set; }
        public IFormFile? BirthCertificateFile { get; set; }
        public string sex { get; set; }
        public  int GradeEnrolled { get; set; } 
        public IFormFile? CGMFile { get; set; }
        public IFormFile? TORFile { get; set; }
        public byte[]? BirthCertificateRecieve { get; set; }
        public byte[]? CGMRecieve { get; set; }
        public byte[]? TORRecieve { get; set; }
        public DateTime? ExamSchedule { get; set; }
        public string ParentFirstName { get; set; }
        public string ParentMiddleName { get; set; }
        public string ParentLastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Gcash { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public DateTime ParentBirthday { get; set; }
        public string Parentsex { get; set; }
        
       
       
    }
}
