using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Models
{
    public class Class
    {
        [Key]
        public int Id { get; set; }
        public string? ClassName { get; set; }
        public string Adviser { get; set; }
        public string Grade { get; set; }
        public int ClassSize { get; set; }
        //public string SchoolYear{get; set;}
    }
}
