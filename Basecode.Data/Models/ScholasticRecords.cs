using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Models
{
    public class ScholasticRecords
    {
        [Key]
        public int SKId { get; set; }
        public int StudentId { get; set; }
        public string School { get; set; }
        public int SchoolId { get; set; }
        public string District { get; set; }
        public string Division { get; set; }
        public string Region { get; set; }
        public string Grade { get ; set; }
        public string Section { get; set; }
        public string SchoolYear { get; set; }
        public string Adviser { get; set; }
    }
}
