using Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class Form137Container
    {
        public StudentViewModel Student { get; set; }
        public Settings Settings { get; set; }
        public List<Form137ViewModel> StudentForm137 { get; set; }
    }
}
