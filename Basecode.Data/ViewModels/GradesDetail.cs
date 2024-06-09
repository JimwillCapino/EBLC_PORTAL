using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class GradesDetail
    {
        public int Student_Id { get; set; }
        public int Subject_Id { get; set; }
        public int class_id { get; set; }
        public int Quarter { get; set; }
        [Display(Name ="grade input")]
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "The field must be a whole number.")]
        [Range(60, 100, ErrorMessage = "Grade must be between 50 and 100.")]
        public int GradeInput { get; set; }
        public List<GradesViewModel> Grades { get; set; }
        public int? passingGrade { get; set; }
    }
}
