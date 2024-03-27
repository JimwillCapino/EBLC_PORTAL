using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Models
{
    public class Grades
    {
        [Key]
        public int Grade_Id { get; set; }
        public int Student_Id { get; set; }
        public int Subject_Id { get; set; } 
        public int Grade { get; set; }
        public int? Quarter { get; set; }
        public int Grade_Level { get; set; }
        public string School_Year { get; set; }
    }
}
