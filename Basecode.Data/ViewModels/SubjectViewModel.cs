using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class SubjectViewModel
    {
        public int subjectid { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 4)]
        public string subjectname { get; set; }
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter a valid integer.")]
        public int grade { get; set; }
        [Required]
        public bool haschild { get; set; }
    }
}
