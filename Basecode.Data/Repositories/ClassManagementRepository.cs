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
        ISubjectRepository _subjectRepository;
        public ClassManagementRepository(IUnitOfWork unitOfWork, 
            BasecodeContext context,
            ITeacherRepository teacherRepository,
            ISubjectRepository subjectRepository) : base(unitOfWork)
        {
            _context = context;
            _teacherRepository = teacherRepository;
            _subjectRepository = subjectRepository;
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
        public void UpdateClass(Class classroom)
        {
            try
            {
                _context.Class.Update(classroom);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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
            return _subjectRepository.GetSubjects();
        }
        public List<ClassStudentViewModel> GetStudents(int grade)
        {
            try
            {
                var student = this.GetDbSet<Student>().ToList().FindAll(s => s.CurrGrade == grade && s.status == "Enrolled");
                var userstudent = this.GetDbSet<UsersPortal>().ToList();

                var students = from s in student
                               join u in userstudent on s.UID equals u.UID
                               select new ClassStudentViewModel
                               {
                                   studentid =s.Student_Id,
                                   firstname = u.FirstName,
                                   middlename = u.MiddleName,
                                   lastname =   u.LastName,
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
                                      id = cs.Id,
                                      studentid = cs.Student_Id,
                                      firstname = u.FirstName,
                                      middlename = u.MiddleName,
                                      lastname = u.LastName,
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
                                join t in teachers on classsub.Teacher_Id equals t.id
                                select new ClassSubjectViewModel
                                {
                                    Id = classsub.Id,
                                    Subject_Id = subs.Subject_Id,
                                    TeacherId = t.id,
                                    SubjectName = subs.Subject_Name,
                                    TeacherName = t.firstname+ " " +t.lastname
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
                                 on c.Adviser equals t.id
                                 select new ClassInitView
                                 {
                                     id = c.Id,
                                     adviserid = t.id,
                                     grade = c.Grade,
                                     classsize = c.ClassSize,
                                     classname = c.ClassName,
                                     advisername = t.firstname + " " + t.lastname                                     
                                 };
            return classesdetails.ToList();
        }
        public async Task<ClassViewModel> GetClassViewModelById(int id)
        {
            try
            {
                var selectedClass = this.GetDbSet<Class>().Find(id);
                var teachers = await _teacherRepository.GetAllTeachersInitViewAsync();
                var teacher = teachers.Find(t => t.id == selectedClass.Adviser);
                var studentsByGrade = this.GetStudents(selectedClass.Grade);
                var classdetails = new ClassViewModel
                {
                    Id = selectedClass.Id,
                    Adviser = teacher.id,
                    ClassName = selectedClass.ClassName,
                    AdviserName = teacher.firstname+ " " + teacher.lastname,
                    ClassSize = selectedClass.ClassSize,
                    Grade = selectedClass.Grade,
                    ProfilePic = teacher.profilepic,                   
                };
                var studentExist = this.GetDbSet<ClassStudents>();
                classdetails.Teachers = teachers;
                var subs = classdetails.ClassSubjects = await this.GetClassSubjects(selectedClass.Id);       
                var cl = classdetails.ClassStudents = this.GetClassStudents(id);

                //Gets the student not existing on all the classes
                classdetails.Students = this.GetStudents(selectedClass.Grade).Where(s => !studentExist.Any(st => st.Student_Id == s.studentid)).ToList();
                classdetails.Subjects = this.GetSubjects().Where(s => (!subs.Any(cs => cs.Subject_Id == s.Subject_Id)) && s.Grade == selectedClass.Grade).ToList();

                return classdetails;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.InnerException.Message);
            }
        }
        public List<TeacherClassDetails> GetTeacherClassDetails(string teacher_Id)
        {
            try
            {
                var teacherClassDetails = new TeacherClassDetails();
                var classList = this.GetDbSet<ClassSubjects>().Where(c => c.Teacher_Id == teacher_Id);
                var allclass = this.GetDbSet<Class>();
                var subject = this.GetDbSet<Subject>();

                var list = from cl in classList
                           join ac in allclass
                           on cl.ClassId equals ac.Id
                           join s in subject
                           on cl.Subject_Id equals s.Subject_Id
                           select new TeacherClassDetails
                           {
                               classid = ac.Id,
                               subjectid = s.Subject_Id,
                               subjectname = s.Subject_Name,
                               classname = ac.ClassName,
                               grade = ac.Grade,
                               haschild =s.HasChild
                           };
                var list1 = list.ToList();
                ////Initialize students belonging to the class
                //for (int x = 0; x < list1.Count(); x++)
                //{
                //    int class_Id = list1.ElementAt(x).classid;
                //    list1.ElementAt(x).Students = this.GetClassStudents(class_Id);
                //}

                return list1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.InnerException.Message);
            }
        }
        public TeacherClassDetails GetTeacherSubjectDetails(int classid, int subjectId)
        {
            try
            {
                var teacherClassDetails = new TeacherClassDetails();
                var teacherSubject = this.GetDbSet<ClassSubjects>().Where(p => p.ClassId == classid).FirstOrDefault(p => p.Subject_Id == subjectId);
                var allclass = this.GetDbSet<Class>().Find(teacherSubject.ClassId);
                var subject = this.GetDbSet<Subject>().Find(teacherSubject.Subject_Id);

                teacherClassDetails.subjectname = subject.Subject_Name;
                teacherClassDetails.subjectid = subjectId;
                teacherClassDetails.classname = allclass.ClassName;
                teacherClassDetails.classid = allclass.Id;
                teacherClassDetails.haschild = subject.HasChild;
                teacherClassDetails.grade = allclass.Grade;
                return teacherClassDetails;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(ex.Message);
            }
        }
        public List<HomeRoom> GetTeacherHomeRoom(string teacher_Id)
        {
            try
            {
                var classHome = this.GetDbSet<Class>().Where(c => c.Adviser == teacher_Id).Select(c => new HomeRoom
                {
                    classid = c.Id,
                    classname = c.ClassName,
                    grade = c.Grade
                }).ToList();                
                return classHome;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex);
                throw;
            }
        }        
        public async Task<ClassInitView> GetClassWhereStudentBelong(int studentId, int gradeLevel)
        {
            try
            {
                var teachers = await _teacherRepository.GetAllTeachersInitViewAsync();
                var classroom = this.GetDbSet<Class>().Where(s => s.Grade == gradeLevel).ToList();
                var studentsInClasses = this.GetDbSet<ClassStudents>().Where(s => s.Student_Id == studentId).ToList();

                var unionClassDetails = from c in classroom
                                        join s in studentsInClasses
                                        on c.Id equals s.Class_Id
                                        join t in teachers on 
                                        c.Adviser equals t.id
                                        select new ClassInitView
                                        {
                                            id = c.Id,
                                            classname = c.ClassName,
                                            grade = c.Grade,
                                            classsize = c.ClassSize,                                          
                                            advisername = t.firstname + " " + t.lastname,
                                        };
                return unionClassDetails.ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public async Task<int> GetStudentYearLevel(int studentId)
        {
            try
            {
                var teachers = await _teacherRepository.GetAllTeachersInitViewAsync();
                var classroom = this.GetDbSet<Class>().ToList();
                var studentsInClasses = this.GetDbSet<ClassStudents>().Where(s => s.Student_Id == studentId).ToList();

                var unionClassDetails = from c in classroom
                                        join s in studentsInClasses
                                        on c.Id equals s.Class_Id
                                        join t in teachers on
                                        c.Adviser equals t.id
                                        select new ClassInitView
                                        {
                                            id = c.Id,
                                            classname = c.ClassName,
                                            grade = c.Grade,
                                            classsize = c.ClassSize,                                           
                                            advisername = t.firstname + " " + t.lastname,
                                        };
                return unionClassDetails.OrderByDescending(p => p.grade).FirstOrDefault().grade;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
