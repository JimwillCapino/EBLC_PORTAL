using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Models
{
    public class Student
    {
        [Key]
        public int Student_Id { get; set; }
        public int UID { get; set; }
        public string? status { get; set; }
        public string? LRN { get; set; }   
        public int ParentId {  get; set; }
        public string CurrGrade { get; set; }
    }
}
