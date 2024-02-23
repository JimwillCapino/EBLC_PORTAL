using Basecode.Data.Interfaces;
using Basecode.Data.Models;
using Basecode.Data.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Repositories
{
    public class ClassManagementRepository : BaseRepository, IClassManagementRepository
    {
        BasecodeContext _context;
        ITeacherRepository _teacherRepository;
        public ClassManagementRepository(IUnitOfWork unitOfWork, 
            BasecodeContext context,
            ITeacherRepository teacherRepository) : base(unitOfWork)
        {
            _context = context;
            _teacherRepository = teacherRepository;
        }
        public int AddClass(Class classroom)
        {
            try
            {
                var classRoom = _context.Class.Add(classroom);
                _context.SaveChanges();
                return classRoom.Entity.Id;
            }
           
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.InnerException.Message);
            }
        
        }
        public Class GetClass(int id)
        {
            try
            {
                return _context.Class.Find(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.InnerException.Message);
            }
        }
        public void RemoveClass(Class classroom)
        {
            try
            {
                _context.Class.Remove(classroom);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.InnerException.Message);
            }
        }
        public void RemoveClassStudents(ClassStudents student)
        {
            try
            {
                _context.ClassStudents.Remove(student);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.InnerException.Message);
            }
        }
        public void RemoveClassSubjects(ClassSubjects subjects)
        {
            try
            {
                _context.ClassSubjects.Remove(subjects);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.InnerException.Message);
            }
        }
        public ClassStudents GetClassStudentsById(int id)
        {
            try
            {
                var classStudent = _context.ClassStudents.Find(id);
                return classStudent;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.InnerException.Message);
            }
        }
        public ClassSubjects GetClassSubjectById(int id)
        {
            try
            {
                return _context.ClassSubjects.Find(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.InnerException.Message);
            }
        }
        public void AddClassSubject(ClassSubjects classSubjects)
        {
            try
            {
                _context.ClassSubjects.Add(classSubjects);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.InnerException.Message);
            }
        }
        public void AddClassStudent(ClassStudents students)
        {
            try
            {
                _context.ClassStudents.Add(students);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.InnerException.Message);
            }
        }
        public List<Subject> GetSubjects()
        {
            return this.GetDbSet<Subject>().ToList();
        }
        public List<ClassStudentViewModel> GetStudents(int grade)
        {
            try
            {
                var student = this.GetDbSet<Student>().ToList().FindAll(s => s.CurrGrade == grade);
                var userstudent = this.GetDbSet<UsersPortal>().ToList();

                var students = from s in student
                               join u in userstudent on s.UID equals u.UID
                               select new ClassStudentViewModel
                               {
                                   Student_Id =s.Student_Id,
                                   FirstName = u.FirstName,
                                   MiddleName = u.MiddleName,
                                   LastName =   u.LastName,
                               };
                return students.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.InnerException.Message);
            }
        }
        public List<ClassStudentViewModel> GetClassStudents(int classId)
        {
            try
            {
                var classStudents = this.GetDbSet<ClassStudents>().Where(cs => cs.Class_Id == classId);
                var students = this.GetDbSet<Student>();
                var u_students = this.GetDbSet<UsersPortal>();

                var studentlist = from cs in classStudents
                                  join s in students on cs.Student_Id equals s.Student_Id
                                  join u in u_students on s.UID equals u.UID
                                  select new ClassStudentViewModel
                                  {
                                      Id = cs.Id,
                                      Student_Id = cs.Student_Id,
                                      FirstName = u.FirstName,
                                      MiddleName = u.MiddleName,
                                      LastName = u.LastName,
                                  };
                return studentlist.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.InnerException.Message);
            }
        }
        public async Task<List<TeacherViewModel>> GetAllTeachers()
        {
            return await _teacherRepository.GetAllTeachersInitViewAsync();
        }
        public async Task<List<ClassSubjectViewModel>> GetClassSubjects(int classId)
        {
            try
            {
                var subjects = this.GetDbSet<Subject>().ToList();
                var classssubjects = this.GetDbSet<ClassSubjects>().ToList().Where(s => s.ClassId == classId);
                var teachers = await _teacherRepository.GetAllTeachersInitViewAsync();

                var classSubs = from classsub in classssubjects
                                join subs in subjects on classsub.Subject_Id equals subs.Subject_Id
                                join t in teachers on classsub.Teacher_Id equals t.Id
                                select new ClassSubjectViewModel
                                {
                                    Id = classsub.Id,
                                    Subject_Id = subs.Subject_Id,
                                    TeacherId = t.Id,
                                    SubjectName = subs.Subject_Name,
                                    TeacherName = t.FirstName+ " " +t.LastName
                                };
                return classSubs.ToList();
                                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.InnerException.Message);
            }
        }
        public async Task<List<ClassInitView>> GetAllClass()
        {
            var classes = this.GetDbSet<Class>().ToList();
            var teachers = await _teacherRepository.GetAllTeachersInitViewAsync();

            var classesdetails = from c in classes
                                 join t in teachers
                                 on c.Adviser equals t.Id
                                 select new ClassInitView
                                 {
                                     Id = c.Id,
                                     AdviserId = t.Id,
                                     Grade = c.Grade,
                                     ClassSize = c.ClassSize,
                                     ClassName = c.ClassName,
                                     AdviserName = t.FirstName + " " + t.LastName
                                 };
            return classesdetails.ToList();
        }
        public async Task<ClassViewModel> GetClassViewModelById(int id)
        {
            try
            {
                var selectedClass = this.GetDbSet<Class>().Find(id);
                var teachers = await _teacherRepository.GetAllTeachersInitViewAsync();
                var teacher = teachers.Find(t => t.Id == selectedClass.Adviser);
                var studentsByGrade = this.GetStudents(selectedClass.Grade);
                var classdetails = new ClassViewModel
                {
                    Id = selectedClass.Id,
                    Adviser = teacher.Id,
                    ClassName = selectedClass.ClassName,
                    AdviserName = teacher.FirstName+ " " + teacher.LastName,
                    ClassSize = selectedClass.ClassSize,
                    Grade = selectedClass.Grade
                };
                var studentExist = this.GetDbSet<ClassStudents>();
                classdetails.Teachers = teachers;
                var subs = classdetails.ClassSubjects = await this.GetClassSubjects(selectedClass.Id);       
                var cl = classdetails.ClassStudents = this.GetClassStudents(id);

                //Gets the student not existing on all the classes
                classdetails.Students = this.GetStudents(selectedClass.Grade).Where(s => !studentExist.Any(st => st.Student_Id == s.Student_Id)).ToList();
                classdetails.Subjects = this.GetSubjects().Where(s => (!subs.Any(cs => cs.Subject_Id == s.Subject_Id)) && s.Grade == selectedClass.Grade).ToList();

                return classdetails;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.InnerException.Message);
            }
        }
    }
}
