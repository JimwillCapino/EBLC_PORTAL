using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Models
{
    public class Settings
    {
        public int Id { get; set; }
        public DateTime? StartofClass {  get; set; }
        public DateTime? EndofClass { get; set; }
    }
}
