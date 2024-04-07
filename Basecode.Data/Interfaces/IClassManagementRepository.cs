using Basecode.Data.Models;
using Basecode.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Interfaces
{
    public interface IClassManagementRepository
    {
        public int AddClass(Class classroom);
        public void AddClassSubject(ClassSubjects classSubjects);
        public void AddClassStudent(ClassStudents students);
        public  Task<List<ClassInitView>> GetAllClass();
        public  Task<ClassViewModel> GetClassViewModelById(int id);
        public Task<List<TeacherViewModel>> GetAllTeachers();
        public ClassSubjects GetClassSubjectById(int id);
        public ClassStudents GetClassStudentsById(int id);
        public void RemoveClassSubjects(ClassSubjects subjects);
        public void RemoveClassStudents(ClassStudents student);
        public List<Subject> GetSubjects();
        public List<ClassStudentViewModel> GetStudents(int grade);
        public Class GetClass(int id);
        public void RemoveClass(Class classroom);
        public List<TeacherClassDetails> GetTeacherClassDetails(string teacher_Id);
        public List<HomeRoom> GetTeacherHomeRoom(string teacher_Id);
        public Task<ClassInitView> GetClassWhereStudentBelong(int studentId, string schoolYear);
    }
}
