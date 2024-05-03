﻿
using AutoMapper;
using Basecode.Data;
using Basecode.Data.Interfaces;
using Basecode.Data.Models;
using Basecode.Data.ViewModels;
using Basecode.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Services.Services
{
    public class StudentService : IStudentService
    {
        IStudentRepository _studentRepository;
        IUsersRepository _usersRepository;
        IParentRepository _parentRepository;
        IRTPRepository _rtpRepository;
        IMapper _mapper;
        public StudentService(IStudentRepository studentRepository,
            IUsersRepository usersRepository,
            IParentRepository parentRepository,
            IRTPRepository rtpRepository,
            IMapper mapper) 
        { 
            _studentRepository = studentRepository;
            _usersRepository = usersRepository;
            _parentRepository = parentRepository;
            _rtpRepository = rtpRepository;
            _mapper = mapper;
        }
        public void AddStudent(Student student)
        {
            try
            {
                _studentRepository.AddStudent(student);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public Student GetStudent(int id)
        {
            try
            {
                return _studentRepository.GetStudent(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public void AddStudent(RegisterStudent newStudent)
        {
            try
            {                
                var StudentUserPortal = _mapper.Map<UsersPortal>(newStudent);
                var parent = _mapper.Map<Parent>(newStudent);
                var parentRTP = _mapper.Map<RTPCommons>(newStudent);
                var parentUserPortal = new UsersPortal()
                {
                    FirstName = newStudent.ParentFirstName,
                    MiddleName = newStudent.ParentMiddleName,
                    LastName = newStudent.ParentLastName,
                    Birthday = newStudent.ParentBirthday,
                    sex = newStudent.Parentsex,
                    ProfilePic = null,
                };

                if(_studentRepository.isExisting(StudentUserPortal))
                {
                    throw new Exception("This student has already been added to the system.");                   
                } 

                var parentUID = _usersRepository.AddUser(parentUserPortal);
                parent.UID = parentUID;
                var parentId = _parentRepository.AddParent(parent);
                parentRTP.UID = parentUID;               
                _rtpRepository.addRTPCommons(parentRTP);
                var studentUID = _usersRepository.AddUser(StudentUserPortal);
                var student = new Student()
                {
                    UID = studentUID,
                    CurrGrade = newStudent.GradeEnrolled,
                    LRN = newStudent.LRN,
                    ParentId = parentId,
                    status = "Enrolled",
                };
                _studentRepository.AddStudent(student);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(ex.Message);
            }
        }
        public void RemoveStudent(int studentId)
        {
            try
            {
                var student = _studentRepository.GetStudent(studentId);
                var userdetails = _usersRepository.GetUserById(student.UID);
                var parent = _parentRepository.GetParentById(student.ParentId);
                var parentUser = _usersRepository.GetUserById(parent.UID);

                _studentRepository.RemoveStudent(student);
                _usersRepository.RemoveUser(userdetails);
                _parentRepository.RemoveParent(parent);
                _usersRepository.RemoveUser(parentUser);                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public async Task UpdateStudentAsync(Student student)
        {
            try
            {
                 await _studentRepository.UpdateStudentAsync(student);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public async Task UnEnrollStudents()
        {
            try
            {
                var students = _studentRepository.GetAllStudent();
                foreach(var student in students)
                {
                    student.status = "Not Enrolled";
                    await _studentRepository.UpdateStudentAsync(student);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
    }
}
