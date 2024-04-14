using System;
using System.Collections.Generic;
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
        public string ParentFirstName { get; set; }
        public string ParentMiddleName { get; set; }
        public string ParentLastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public DateTime ParentBirthday { get; set; }
        public string Parentsex { get; set; }
    }
}
