using Basecode.Data.Models;
using Basecode.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Services.Interfaces
{
    public interface IStudentManagementService
    {
        public GradesDetail GetStudentGradeBySubject(int student_Id, int subject_Id);
        public void SubmitGrade(int student_id, int Subject_Id, int grade, int Quarter);
        public void EditGrade(int Grade_Id,int student_id, int Subject_Id, int grade, int Quarter);
        public Task<StudentDetailsWithGrade> GetStudentGrades(int student_Id, string school_year);
        public List<StudentViewModel> GetAllStudents();
        public void AddCoreValues(Core_Values values);
        public void AddBehavioralStatement(Behavioural_Statement statement);
        public List<Core_Values> GetAllCoreValues();
        public List<Learners_Values_Report> GetLearnersValues();
        public void UpdateCoreValues(Core_Values values);
        public void UpdateBehavioralStatement(Behavioural_Statement statement);
        public void DeleteCoreValues(Core_Values values);
        public Behavioural_Statement GetBehaviouralStatementById(int Id);
        public Core_Values GetCoreValuesById(int Id);
        public ValuesWithGradesContainer GetValuesWithGrades(int StudentId, string schoolyear);
        public void AddLearnerValues(Learner_Values values);
        public void UpdateLearnerValues(int id, string grade);
        public ChildSubjectGrades GetChildSubjectGrades(int headId, int studentId);
        public void AddStudentAttendance(int studentId, int Days_of_School, int Days_of_Present, int Time_of_Tardy, string month);
        public AttendanceContainer GetStudentAttendance(int studentId, string schoolYear);
        public void UpdateAttendance(int id, int studentId, int Days_of_School, int Days_of_Present, int Time_of_Tardy, string month);
        public Task<Form137Container> GetStudentForm137(int studentId);
        public Task UpdateStudentDetails(StudentDetailsContainer studentDetails);
        public Task<StudentDetailsContainer> GetStudentDetails(int studentId);
        public IEnumerable<StudentPreviewInformation> GetStudentPreviewInformation();
        public List<ClassStudentViewModel> GetStudentWithNoGradePerQuarter(int classid, int subjectid, int quarter);
        public List<StudentQuarterlyAverage> GetStudentRanking(string gradeLevel, int quarter, int rank);
        public void DeleteBehavioralStatement(int id);
        public void EditChildSubGrade(int Grade_Id, int headId, int student_id, int Subject_Id, int grade, int Quarter);
    }
}
