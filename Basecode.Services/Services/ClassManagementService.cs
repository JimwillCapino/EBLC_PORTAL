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
                        Student_Id = classStudent.id
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
        public async Task AddClassSubjects(ClassSubjects classSubjects)
        {
            try
            {
                int checkSched = await this.isScheduleCollided(classSubjects.Schedule, classSubjects.Teacher_Id);
                if(checkSched == 1)
                {
                    throw new Exception("The schedule conflicts with another schedule within the class.");
                }
                else if(checkSched == 2)
                {
                    throw new Exception("The schedule conflicts with another schedule in another class of the teacher.");
                }
                _repository.AddClassSubject(classSubjects);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(ex.Message);
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
        public void AddClassStudents(List<ClassStudents> students)
        {
            try
            {
                foreach(var student in students)
                    _repository.AddClassStudent(student);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public ClassViewModel InitilaizeClassViewModel(string grade)
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
        public List<ClassStudentViewModel> GetClassStudents(int classId)
        {
            try
            {
                return _repository.GetClassStudents(classId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public TeacherClassDetails GetTeacherSubjectDetails(int classid, int subjectId)
        {
            try
            {
                return _repository.GetTeacherSubjectDetails(classid, subjectId);
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
        public async Task<int> isScheduleCollided(string schedule, string teacherId)
        {
            try
            {
                var classes = await _repository.GetAllClass();
                
                foreach(var clazz in classes)
                {                   
                    var teacherSubjects = _repository.GetTeacherClassDetails(teacherId);
                    if (teacherSubjects.Where(p => p.schedule == schedule).Count() > 0)
                        return 2;
                    var subjects = await _repository.GetClassSubjects(clazz.id);
                    if (subjects.Where(p => p.Schedule == schedule).Count() > 0)
                        return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
    }
}
