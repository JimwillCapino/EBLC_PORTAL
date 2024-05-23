﻿using Basecode.Data.Models;
using Basecode.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Interfaces
{
    public interface IStudentManagementRepository
    {
        public void SubmitGrade(Grades grade);
        public GradesDetail GetStudentGradeBySubject(int student_Id, int subject_Id);
        public void EditGrade(Grades grade);
        public List<StudentGrades> GetStudentGrades(int student_Id, string school_year);
        public List<StudentViewModel> GetAllStudents();
        public StudentViewModel GetStudent(int student_Id);
        public List<string> GetSchoolYears(int student_Id);
        public void AddCoreValues(Core_Values core_Values);
        public void AddBehaviouralStatement(Behavioural_Statement behaviour_Statement);
        public void AddLearnerValues(Learner_Values values);
        public List<Core_Values> GetAllCoreValues();
        public List<Learners_Values_Report> GetLearnersValues();
        public void UpdateCoreValues(Core_Values values);
        public void DeleteCore_Values(Core_Values values);
        public void UpdateBehavioralStatement(Behavioural_Statement statement);
        public Behavioural_Statement GetBehaviouralStatementById(int Id);
        public Core_Values GetCoreValuesById(int Id);
        public List<ValuesGrades> GetValuesGrades(int StudentId, string schoolyear);
        public Learner_Values GetLearnerValuesById(int id);
        public void UpdateLearnerValues(Learner_Values valuesgrades);       
        public void AddAttendance(Attendance attendance);
        public void UpdateAttendance(Attendance attendance);
        public void DeleteAttendance(Attendance attendance);
        public List<Attendance> GetStudentAtendance(int student_Id, string schoolYear);
        public bool isDateExisting(int month, string schoolYear, int studentid);
        
        public IEnumerable<StudentPreviewInformation> GetAllStudentPreview();
        public int GetBehavioralMaxQuarter(int studentId, int BehavioralId, string schoolYear);
        public Behavioural_Statement GetBehavioural_Statement(int id);
        public void DeleteBehavioralStatement(Behavioural_Statement statement);
        public string GradeLevel(int studentId, string schoolYear);
        public List<string> GetSchoolYearsWithOutGradeLevel(int student_Id);
        public void AddStudentAdviser(StudentAdviser studentAdviser);
    }
}
