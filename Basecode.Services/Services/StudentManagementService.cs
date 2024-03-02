﻿using Basecode.Data.Interfaces;
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
        public StudentManagementService(IStudentManagementRepository studentManagementRepository,
            IStudentRepository studentRepository,
            ISettingsRepository settings) 
        {
            _studentManagementRepository = studentManagementRepository;
            _studentRepository = studentRepository;
            _settingsRepository = settings;
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
        public StudentDetailsWithGrade GetStudentGrades(int student_Id, string school_year)
        {
            try
            {
                var student = new StudentDetailsWithGrade();
                student.School_Years = _studentManagementRepository.GetSchoolYears(student_Id);
                student.Student = _studentManagementRepository.GetStudent(student_Id);
                student.grades = _studentManagementRepository.GetStudentGrades(student_Id,school_year);
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
    }
}
