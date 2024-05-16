using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public  class ChildSubjectContainer
    {
        public int HeadId { get; set; }
        [Display(Name = "subject name")]
        [Required]
        public string Name { get; set; }
        public int ChildSubId { get; set; }
        public List<ChildSubjectView> ChildSubjects { get; set; }
    }
}
