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
        public StudentDetailsWithGrade GetStudentGrades(int student_Id, string school_year);
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
    }
}
