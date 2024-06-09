using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class StudentPreviewInformation
    {
        public int studentid { get; set; }
        public string fullname { get; set; }
        public string? status { get; set; }
        public int? age { get; set; }
        public string? lrn { get; set; }
        public string? grade { get; set; }
    }
}
