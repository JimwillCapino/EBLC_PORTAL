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
        public StudentAdviser GetStudentAdviserByStudentId(int id, string schoolYear);
        public void AddStudentAdviser(StudentAdviser studentAdviser);
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
        public List<ClassStudentViewModel> GetStudents(string grade);
        public Class GetClass(int id);
        public void RemoveClass(Class classroom);
        public List<TeacherClassDetails> GetTeacherClassDetails(string teacher_Id);
        public List<HomeRoom> GetTeacherHomeRoom(string teacher_Id);
        public Task<ClassInitView> GetClassWhereStudentBelong(int studentId, string gradeLevel);
        public Task<string> GetStudentYearLevel(int studentId);
        public void UpdateClass(Class classroom);
        public List<ClassStudentViewModel> GetClassStudents(int classId);
        public TeacherClassDetails GetTeacherSubjectDetails(int classid, int subjectId);
        public  Task<List<ClassSubjectViewModel>> GetClassSubjects(int classId);
        public ScholasticRecords GetScholasticRecordsById(int id);
        public ScholasticRecords GetScholasticRecords(int studentId, string SchoolYear);
        public RemedialClass GetRemedialById(int id);
        public RemedialClass GetRemedial(int studentId, int scholasticId);
        public List<RemedialDetails> GetRemedialDetailsByClass(int RemedialClassId);
        public int AddSholasticRecord(ScholasticRecords scholasticRecords);
        public int AddRemedialClass(RemedialClass remedial);
        public void AddRemedialDetails(RemedialDetails details);
        public void UpdateScholasticRecords(ScholasticRecords records);
        public int UpdateRemedialClass(RemedialClass remedialClass);
    }
}
