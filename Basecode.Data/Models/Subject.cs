using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Models
{
    public class Subject
    {
        [Key]
        public int Subject_Id { get; set; }
        public string Subject_Name { get; set; }    
        public string Grade { get; set; }   
        public bool HasChild { get; set; }
    }
}
