
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
    }
}
