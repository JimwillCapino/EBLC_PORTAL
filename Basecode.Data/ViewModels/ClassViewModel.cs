using Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class ClassViewModel
    {
        public int Id { get; set; }
        public string Adviser {  get; set; }
        public int Grade { get; set; }
        public int ClassSize { get; set; }
        public string? ClassName { get; set; }
        public string AdviserName { get; set; }
        public List<ClassSubjectViewModel> ClassSubjects { get; set; }
        public List<BasicInfoViewModel> ClassStudents { get; set; }
    }
}
