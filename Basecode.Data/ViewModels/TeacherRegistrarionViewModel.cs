using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class TeacherRegistrarionViewModel
    {
        public int Id { get; set; }
        public byte[]? ProfilePic { get; set; }
        public IFormFile? ProfilePicRecieve { get; set; } 
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string sex { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
