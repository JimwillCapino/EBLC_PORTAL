using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basecode.Data.Models;

namespace Basecode.Data.ViewModels
{
    public class SchoolasticViewModel
    {
        
        public int StudentId { get; set; }
        [Display(Name = "School")]
        [Required]
        public string School { get; set; }
        [Display(Name = "School Id")]
        [Required]
        public int SchoolId { get; set; }
        [Display(Name = "District")]
        [Required]
        public string District { get; set; }
        [Display(Name = "Division")]
        [Required]
        public string Division { get; set; }
        [Display(Name = "Region")]
        [Required]
        public string Region { get; set; }
        [Display(Name = "Grade")]
        [Required]
        public string Grade { get; set; }
        [Display(Name = "Section")]
        [Required]
        public string Section { get; set; }
        [Display(Name = "School year")]
        [Required]
        public string SchoolYear { get; set; }
        [Display(Name = "Adviser")]
        [Required]
        public string Adviser { get; set; }

       
        public List<Subject> subjects { get; set;}    
       
        public DateTime from { get; set; }
        public DateTime to { get; set; }
    }
}
