﻿using Basecode.Data;
using Basecode.Data.Interfaces;
using Basecode.Data.Models;
using Basecode.Data.ViewModels;
using Basecode.Services.Interfaces;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Services.Services
{
    public class ClassManagementService : IClassManagementService
    {
        IClassManagementRepository _repository;
        ISettingsRepository _settings;
        public ClassManagementService(IClassManagementRepository classManagementRepository,
            ISettingsRepository settings)
        {
            _repository = classManagementRepository;
            _settings = settings;
        }
        public void AddClass(Class classroom)
        {
            try
            {
                classroom.SchoolYear = _settings.GetSchoolYear();
                _repository.AddClass(classroom);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public void AddClass(ClassViewModel classViewModel)
        {
            try
            {
                Class classSection = new Class
                {
                    ClassName = classViewModel.ClassName,
                    Adviser = classViewModel.Adviser,
                    Grade = classViewModel.Grade,
                    ClassSize = classViewModel.ClassSize,
                    SchoolYear = _settings.GetSchoolYear()
                };

                var classid = _repository.AddClass(classSection);

                //Adding Class subjects 
                foreach (var classSubject in classViewModel.ClassSubjects)
                {
                    ClassSubjects subs = new ClassSubjects
                    {
                        ClassId = classViewModel.Id,
                        Subject_Id = classSubject.Subject_Id,
                        Teacher_Id = classSubject.TeacherId
                    };
                    _repository.AddClassSubject(subs);
                }
                //Adding students to a class
                foreach (var classStudent in classViewModel.ClassStudents)
                {
                    var student = new ClassStudents
                    {
                        Class_Id = classViewModel.Id,
                        Student_Id = classStudent.Id
                    };
                    _repository.AddClassStudent(student);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public void AddClassSubjects(ClassSubjects classSubjects)
        {
            try
            {
                _repository.AddClassSubject(classSubjects);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public void AddClassStudent(ClassStudents student)
        {
            try
            {
                _repository.AddClassStudent(student);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public ClassViewModel InitilaizeClassViewModel(int grade)
        {
            try
            {
                ClassViewModel classroom = new ClassViewModel();
                classroom.Subjects = _repository.GetSubjects();
                classroom.ClassStudents = _repository.GetStudents(grade);
                return classroom;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public async Task<ClassDetailsViewModel> GetAllClass()
        {
            try
            {
                ClassDetailsViewModel classDetailsViewModel = new ClassDetailsViewModel();
                var classes = await _repository.GetAllClass();
                var teachers = await _repository.GetAllTeachers();
                classDetailsViewModel.classes = classes;
                classDetailsViewModel.teachers = teachers;
                return classDetailsViewModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public async Task<ClassViewModel> GetClassViewModelById(int id)
        {
            try
            {
                return await _repository.GetClassViewModelById(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public void RemoveClassStudent(int id)
        {
            try
            {
                var classStudent = _repository.GetClassStudentsById(id);
                _repository.RemoveClassStudents(classStudent);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public void RemoveClassSubject(int id)
        {
            try
            {
                var classSubject = _repository.GetClassSubjectById(id);
                _repository.RemoveClassSubjects(classSubject);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public void RemoveClass(int id)
        {
            try
            {
                var classremove = _repository.GetClass(id);
                _repository.RemoveClass(classremove);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public List<TeacherClassDetails> GetTeacherClassDetails(string teacher_Id)
        {
            try
            {
                return _repository.GetTeacherClassDetails(teacher_Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public List<HomeRoom> GetTeacherHomeRoom(string teacher_Id)
        {
            try
            {
                return _repository.GetTeacherHomeRoom(teacher_Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public void UpdateClass(ClassViewModel classdetails)
        {
            try
            {
                var classTable = new Class()
                {
                    Id = classdetails.Id,
                    ClassName = classdetails.ClassName,
                    Adviser = classdetails.Adviser,
                    Grade = classdetails.Grade,
                    SchoolYear = classdetails.SchoolYear,
                    ClassSize = classdetails.ClassSize
                };
                _repository.UpdateClass(classTable);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
    }
}
