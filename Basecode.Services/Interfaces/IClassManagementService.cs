using Basecode.Data.Models;
using Basecode.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Services.Interfaces
{
    public interface IClassManagementService
    {
        public void AddClass(Class classroom);
        public void AddClass(ClassViewModel classViewModel);
        public Task AddClassSubjects(ClassSubjects classSubjects);
        public void AddClassStudent(ClassStudents student);
        public ClassViewModel InitilaizeClassViewModel(string grade);
        public Task<ClassDetailsViewModel> GetAllClass();
        public Task<ClassViewModel> GetClassViewModelById(int id);
        public void RemoveClassStudent(int id);
        public void RemoveClassSubject(int id);
        public void RemoveClass(int id);
        public List<TeacherClassDetails> GetTeacherClassDetails(string teacher_Id);
        public List<HomeRoom> GetTeacherHomeRoom(string teacher_Id);
        public void UpdateClass(ClassViewModel classdetails);
        public void AddClassStudents(List<ClassStudents> students);
        public List<ClassStudentViewModel> GetClassStudents(int classId);
        public TeacherClassDetails GetTeacherSubjectDetails(int classid, int subjectId);
        public Task<int> isScheduleCollided(string schedule, string teacherId);
    }
}
