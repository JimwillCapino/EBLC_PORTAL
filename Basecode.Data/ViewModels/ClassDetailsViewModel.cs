using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public  class ClassDetailsViewModel
    {
        public List<ClassInitView> Classes {  get; set; }
        public List<TeacherViewModel> Teachers { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "The length of the string must be between 3 and 50 characters.")]
        public string ClassName { get; set; }
        [Required]
        public string Adviser { get; set; }
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter a valid integer.")]
        public int Grade { get; set; }
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter a valid integer.")]
        public int ClassSize { get; set; }        
    }
}
