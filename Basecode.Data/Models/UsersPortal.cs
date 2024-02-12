using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Models
{
    public  class UsersPortal
    {
        [Key]
        public int UID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string sex {  get; set; }
        public DateTime Birthday { get; set; }
        public byte[]? ProfilePic { get; set; }
    }
}
