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
        [Required]
        public string Name { get; set; }
        public List<ChildSubjectView> ChildSubjects { get; set; }
    }
}
