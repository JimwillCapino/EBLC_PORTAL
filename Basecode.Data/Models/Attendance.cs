using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public int Studentid { get; set; }
        public string Month { get; set; }
        public string School_Year { get; set; }
        public int Days_of_Schoool { get; set; }
        public int Days_of_Present { get; set; }
        public int Time_of_Tardy { get; set; }
    }
}
