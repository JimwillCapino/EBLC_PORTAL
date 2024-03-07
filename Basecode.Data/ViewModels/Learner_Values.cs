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
        [Required]
        public int Core_Values { get; set; }
        [Required]
        public string Behavioural_Statement { get; set; }
    }
}
