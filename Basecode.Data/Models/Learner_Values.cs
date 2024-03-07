using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Models
{
    public class Learner_Values
    {
        public int Id { get; set; }
        public int Behavioural_Statement { get; set; }
        public int Quarter {  get; set; }
        public int Grade { get; set; }
        public int Grade_Level { get; set; }
        public string School_Year { get; set; }
    }
}
