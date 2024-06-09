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
        public byte[]? ProfilePic { get; set; }
        public string Grade { get; set; }
        public int ClassSize { get; set; }
        public string? ClassName { get; set; }
        public string AdviserName { get; set; }       
        public List<ClassSubjectViewModel> ClassSubjects { get; set; }
        public List<ClassStudentViewModel> ClassStudents { get; set; }
        public List<TeacherViewModel> Teachers { get; set; }
        public List<Subject>Subjects { get; set; }
        public List<ClassStudentViewModel> Students { get; set; }
        public List<int> SelectedStudents { get; set; }
    }
}
