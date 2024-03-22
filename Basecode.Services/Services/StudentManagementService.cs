using Basecode.Data.Interfaces;
using Basecode.Data.ViewModels;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basecode.Data;
using Basecode.Data.Models;
using Basecode.Services.Interfaces;
namespace Basecode.Services.Services
{
    public class StudentManagementService:IStudentManagementService
    {
        private readonly IStudentManagementRepository _studentManagementRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ISettingsRepository _settingsRepository;
        private readonly ISubjectRepository _subjectRepository;
        public StudentManagementService(IStudentManagementRepository studentManagementRepository,
            IStudentRepository studentRepository,
            ISettingsRepository settings,
            ISubjectRepository subjectRepository) 
        {
            _studentManagementRepository = studentManagementRepository;
            _studentRepository = studentRepository;
            _settingsRepository = settings;
            _subjectRepository = subjectRepository;
        }
        public GradesDetail GetStudentGradeBySubject(int student_Id, int subject_Id)
        {
            try
            {
                return _studentManagementRepository.GetStudentGradeBySubject(student_Id, subject_Id);
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public void SubmitGrade(int student_id, int Subject_Id, int grade, int Quarter)
        {
            try
            {
                var student = _studentRepository.GetStudent(student_id);
                var grades = new Grades
                {
                    Student_Id = student_id,
                    Subject_Id = Subject_Id,
                    Grade = grade,
                    Quarter =   Quarter,
                    Grade_Level = student.CurrGrade,
                    School_Year =_settingsRepository.GetSchoolYear()
                };
                _studentManagementRepository.SubmitGrade(grades);
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public void EditGrade(int Grade_Id, int student_id, int Subject_Id, int grade, int Quarter)
        {
            try
            {
                var student = _studentRepository.GetStudent(student_id);
                var grades = new Grades
                {
                    Grade_Id = Grade_Id,
                    Student_Id = student_id,
                    Subject_Id = Subject_Id,
                    Grade = grade,
                    Quarter = Quarter,
                    Grade_Level = student.CurrGrade,
                    School_Year = _settingsRepository.GetSchoolYear()
                };
                _studentManagementRepository.EditGrade(grades);
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public ChildSubjectGrades GetChildSubjectGrades(int headId, int studentId)
        {
            try
            {
                var childSubjectGrade = new ChildSubjectGrades();
                childSubjectGrade.ChildSubjects = _subjectRepository.GetChildSubject(headId);
                childSubjectGrade.GradesContainer = new List<GradesDetail>();
                childSubjectGrade.HeadId = headId;
                childSubjectGrade.StudentId = studentId;
                foreach(var subject in childSubjectGrade.ChildSubjects)
                {
                    childSubjectGrade.GradesContainer.Add(_studentManagementRepository.GetStudentGradeBySubject(studentId, subject.Id));
                }
                return childSubjectGrade;
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public List<Core_Values> GetAllCoreValues()
        {
            try
            {
                return _studentManagementRepository.GetAllCoreValues();
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public StudentDetailsWithGrade GetStudentGrades(int student_Id, string school_year)
        {
            try
            {
                var student = new StudentDetailsWithGrade();
                student.School_Years = _studentManagementRepository.GetSchoolYears(student_Id);
                student.Student = _studentManagementRepository.GetStudent(student_Id);
                student.grades = _studentManagementRepository.GetStudentGrades(student_Id,school_year);
                student.valuesGrades = _studentManagementRepository.GetValuesGrades(student_Id,school_year);
                student.learnersValues = _studentManagementRepository.GetLearnersValues();
                return student;
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public List<StudentViewModel> GetAllStudents()
        {
            try
            {
                return _studentManagementRepository.GetAllStudents();
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public void AddCoreValues(Core_Values values)
        {
            try
            {
                _studentManagementRepository.AddCoreValues(values);
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public void AddBehavioralStatement(Behavioural_Statement statement)
        {
            try
            {
                _studentManagementRepository.AddBehaviouralStatement(statement);
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public List<Learners_Values_Report> GetLearnersValues()
        {
            try
            {
                return _studentManagementRepository.GetLearnersValues();
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public void UpdateCoreValues(Core_Values values)
        {
            try
            {
                _studentManagementRepository.UpdateCoreValues(values);
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public void UpdateBehavioralStatement(Behavioural_Statement statement)
        {
            try
            {
                _studentManagementRepository.UpdateBehavioralStatement(statement);
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public void DeleteCoreValues(Core_Values values)
        {
            try
            {
                _studentManagementRepository.DeleteCore_Values(values);
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public Behavioural_Statement GetBehaviouralStatementById(int Id)
        {
            try
            {
                return _studentManagementRepository.GetBehaviouralStatementById(Id);
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public Core_Values GetCoreValuesById(int Id)
        {
            try
            {
                return _studentManagementRepository.GetCoreValuesById(Id);
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public ValuesWithGradesContainer GetValuesWithGrades(int StudentId, string schoolyear)
        {
            try
            {
                var valueswithgrades = new ValuesWithGradesContainer();
                valueswithgrades.Student_Id = StudentId;
                valueswithgrades.School_Year = schoolyear;
                valueswithgrades.Grades = _studentManagementRepository.GetValuesGrades(StudentId, schoolyear);
                valueswithgrades.Values = _studentManagementRepository.GetLearnersValues();
                valueswithgrades.Scool_Years = _studentManagementRepository.GetValuesSchoolyear(StudentId);
                return valueswithgrades;
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public void AddLearnerValues(Learner_Values values)
        {
            try
            {
                _studentManagementRepository.AddLearnerValues(values);
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public void UpdateLearnerValues(int id, string grade)
        {
            try
            {
                var valuesgrade = _studentManagementRepository.GetLearnerValuesById(id);
                valuesgrade.Grade = grade;
                _studentManagementRepository.UpdateLearnerValues(valuesgrade);
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
    }
}
