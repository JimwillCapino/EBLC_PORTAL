using Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class Learner_Values_ViewModel
    {
        public int Id { get; set; }
        public List<Core_Values> CoreValues { get; set; }
        [Display(Name ="core values")]
        [Required]
        public int Core_Values { get; set; }
        [Display(Name = "behavioral statement")]
        [Required]
        [StringLength(100, ErrorMessage = "The Behavioral Statement must be at most {1} characters long.")]
        public string Behavioural_Statement { get; set; }
    }
}
