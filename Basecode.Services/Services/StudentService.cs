
using AutoMapper;
using Basecode.Data;
using Basecode.Data.Interfaces;
using Basecode.Data.Models;
using Basecode.Data.Repositories;
using Basecode.Data.ViewModels;
using Basecode.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Basecode.Services.Services
{
    public class StudentService : IStudentService
    {
        IClassManagementRepository _classmanagement;
        IStudentRepository _studentRepository;
        IUsersRepository _usersRepository;
        IParentRepository _parentRepository;
        IRTPRepository _rtpRepository;
        IMapper _mapper;
        ISettingsRepository _settingsRepository;
        public StudentService(IStudentRepository studentRepository,
            IUsersRepository usersRepository,
            IParentRepository parentRepository,
            IRTPRepository rtpRepository,
            IMapper mapper,
            IClassManagementRepository classManagement,
            ISettingsRepository settingsRepository) 
        { 
            _studentRepository = studentRepository;
            _usersRepository = usersRepository;
            _parentRepository = parentRepository;
            _rtpRepository = rtpRepository;
            _mapper = mapper;
            _classmanagement = classManagement;
            _settingsRepository = settingsRepository;
        }
        public int AddStudent(Student student)
        {
            try
            {
                return _studentRepository.AddStudent(student);
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
                if (newStudent.ProfilePicRecieve != null)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        newStudent.ProfilePicRecieve.CopyTo(stream);
                        StudentUserPortal.ProfilePic = stream.ToArray();
                    }
                }
                else
                {
                    newStudent.ProfilePic = newStudent.ProfilePic;
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
                var studId = _studentRepository.AddStudent(student);
                var settings = _settingsRepository.GetSettings();
                var scholasticRecords = new ScholasticRecords()
                {
                    School = settings.School_Name,
                    SchoolYear = _settingsRepository.GetSchoolYear(),
                    SchoolId = settings.SchoolId.Value,
                    District = settings.District,
                    Division = settings.Division,
                    Region = settings.Region,
                    Section = "Not Set",
                    Adviser = "Not Set",
                    StudentId = studId,
                    Grade = student.CurrGrade
                };
                _classmanagement.AddSholasticRecord(scholasticRecords);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(ex.Message);
            }
        }
        public int GetUnEnrolledCount()
        {
            try
            {
                return _studentRepository.GetAllStudent().Where(p => p.status == "Not Enrolled").Count();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(Constants.Exception.DB);
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
                var students = _studentRepository.GetAllStudent().ToList();   
                foreach (var student in students)
                {
                    var classbelong = await _classmanagement.GetClassWhereStudentBelong(student.Student_Id, student.CurrGrade);
                    var ClassStudentViewModel = _classmanagement.GetClassStudents(classbelong.id).FirstOrDefault(p => p.studentid == student.Student_Id);
                    var classStudent = _classmanagement.GetClassStudentsById(ClassStudentViewModel.id);
                    student.status = "Not Enrolled";
                    _studentRepository.UpdateStudent(student);
                    _classmanagement.RemoveClassStudents(classStudent);
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
