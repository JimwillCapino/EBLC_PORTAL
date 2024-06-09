using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Models
{
    public class RemedialClass
    {
        public int Id { get; set; }
        public DateTime from { get; set; }
        public DateTime to { get; set; }
        public int SchoolId { get; set; }
        public int StudentId { get; set; }
    }
}
