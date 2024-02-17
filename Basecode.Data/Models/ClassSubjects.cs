using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Models
{
    public class ClassSubjects
    {
        [Key]
        public int Id { get; set; }
        public int ClassId { get; set; }
        public int Subject_Id {  get; set; }
        public string Teacher_Id { get; set; }
    }
}
