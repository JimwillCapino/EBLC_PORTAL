using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Models
{
    public class Teacher
    {
        [Key]
        public int Teacher_Id { get; set; }
        public int UID { get; set; }
    }
}
