using Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class Learners_Values_Report
    {
        public int Core_Values_Id { get; set; }        
        public string Core_Values { get; set; }
        public List<Behavioural_Statement> behavioural_Statements { get; set; }
    }
}
