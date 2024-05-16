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
        public List<ClassInitView> classes {  get; set; }
        public List<TeacherViewModel> teachers { get; set; }
       
        public int classid { get; set; }
        [Display(Name ="class name")]
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "The length of the string must be between 3 and 50 characters.")]
        public string classname { get; set; }
        [Display(Name = "adviser")]
        [Required]
        public string adviser { get; set; }
        [Display(Name = "grade")]
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter a valid integer.")]
        public string grade { get; set; }
        [Display(Name = "class size")]
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter a valid integer.")]
        public int classsize { get; set; }        
    }
}
