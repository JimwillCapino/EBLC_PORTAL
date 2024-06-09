using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Models
{
    public class RemedialDetails
    {
        public int Id { get; set; }
        public int RemedialClass { get; set; }
        public string LearningAreas { get; set; }
        public int FinalRating { get; set; }
        public string RemidialClassMark { get ; set; }
        public int RecomputedFinalGrade { get; set; }
        public string Remarks { get ; set; }    
    }
}
