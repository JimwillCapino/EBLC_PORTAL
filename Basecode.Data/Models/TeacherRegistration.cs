using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Models
{
    public class TeacherRegistration
    {
        public int Id { get; set; }
        public int UserPortalID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
