using Basecode.Data.Interfaces;
using Basecode.Data.Models;
using Basecode.Data.ViewModels;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Repositories
{
    public class SubjectRepository : BaseRepository, ISubjectRepository
    {
        BasecodeContext _context;
        public SubjectRepository(IUnitOfWork unitOfWork, BasecodeContext context) : base(unitOfWork)
        {
            _context = context;
        }
        public int AddSubject(Subject subject)
        {
            try
            {
                var enitity =_context.Subject.Add(subject);
                _context.SaveChanges();
                var id = enitity.Entity.Subject_Id;
                return id;
                //
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.InnerException.Message);
            }
        }
       public List<Subject> GetAllSubjectTakenByStudent(int studentId, int gradeLevel)
        {
            try
            {
                var subjects = this.GetDbSet<Subject>();
                var Classes = this.GetDbSet<Class>().Where(p => p.Grade == gradeLevel);
                var ClassStudents = this.GetDbSet<ClassStudents>().Where(p => p.Student_Id == studentId);
                var ClassSubjects = this.GetDbSet<ClassSubjects>();
                var headSubjects = this.GetDbSet<HeadSubject>();

                var SubjectsTaken = from Class in Classes
                                    join classstudent in ClassStudents
                                    on Class.Id equals classstudent.Class_Id
                                    join classsubject in ClassSubjects
                                    on Class.Id equals classsubject.ClassId
                                    join subject in subjects on
                                    classsubject.Subject_Id equals subject.Subject_Id
                                    join headsubject in headSubjects
                                    on subject.Subject_Id equals headsubject.Subect_Id
                                    select new Subject
                                    {
                                        Subject_Id = subject.Subject_Id,
                                        Subject_Name = subject.Subject_Name,
                                        HasChild = subject.HasChild,
                                        Grade = subject.Grade
                                    };
                return SubjectsTaken.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
       public List<Subject> GetAllSubjects(int studentId, int gradeLevel) 
       {
            try
            {
                List<Subject> ChildSubjectContainer = new List<Subject>();
                //var mainClass = this.GetDbSet<Class>().Where(p => p.Grade == gradeLevel);
                //var mainClassStudent = this.GetDbSet<ClassStudents>().Where(p => p.Student_Id == studentId);
                //var mainClassSubjects = this.GetDbSet<ClassSubjects>();
                var referenceSubjcect = this.GetDbSet<Subject>().Where(p => p.Grade == gradeLevel);
                var nochildsub = this.GetDbSet<Subject>().Where(p => !p.HasChild);
                var subjects = this.GetDbSet<HeadSubject>();
                var HeadSubs = this.GetDbSet<Subject>().Where(p => p.HasChild);
                var childSubs = this.GetDbSet<ChildSubject>();

                var subsWithNoChild = from subs in nochildsub
                                       join s in subjects on subs.Subject_Id equals s.Subect_Id
                                       join reference in referenceSubjcect
                                       on subs.Subject_Id equals reference.Subject_Id
                                      select new Subject
                                      {
                                          Subject_Id = subs.Subject_Id,
                                          Subject_Name = subs.Subject_Name,
                                          Grade = subs.Grade,
                                          HasChild = subs.HasChild,
                                      };

                var unionHeadSubject = from subs in HeadSubs
                                       join reference in referenceSubjcect
                                       on subs.Subject_Id equals reference.Subject_Id
                                       select new Subject
                                       {
                                           Subject_Id = subs.Subject_Id,
                                           Subject_Name = subs.Subject_Name,
                                           Grade = subs.Grade,
                                           HasChild = subs.HasChild,
                                       };
                var unionChildSubjects = from subs in childSubs
                                         join reference in referenceSubjcect
                                         on subs.Subject_Id equals reference.Subject_Id
                                         select new 
                                         {
                                             HeadId = subs.HeadSubjectId,
                                             Subject_Id = subs.Subject_Id,
                                             Subject_Name = reference.Subject_Name,
                                             Grade = reference.Grade,
                                             HasChild = reference.HasChild,
                                         };
                foreach(var sub in unionHeadSubject)
                {
                    ChildSubjectContainer.Add(sub);
                    var holder = unionChildSubjects.Where(p => p.HeadId == sub.Subject_Id);
                    foreach(var subs in holder)
                    {
                        var childsubject = new Subject
                        {
                            Subject_Id = subs.Subject_Id,
                            Subject_Name = subs.Subject_Name,
                            Grade = subs.Grade,
                            HasChild = subs.HasChild
                        };
                        ChildSubjectContainer.Add(childsubject);
                    }                   
                }
                var concat = subsWithNoChild.ToList().Concat(ChildSubjectContainer);
                return concat.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public List<SubjectViewModel> GetsSubjectsForDataTables()
        {
            try
            {
                var headSubject = this.GetDbSet<HeadSubject>();
                var subject = this.GetDbSet<Subject>();

                var viewSubject = from h in headSubject
                                  join s in subject
                                  on h.Subect_Id equals s.Subject_Id
                                  select new SubjectViewModel
                                  {
                                      subjectid = s.Subject_Id,
                                      subjectname = s.Subject_Name,
                                      grade = s.Grade,
                                      haschild = s.HasChild,
                                  };
                return viewSubject.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.InnerException.Message);
            }
        }
        public List<Subject> GetSubjects()
        {
            try
            {
                var headSubject = this.GetDbSet<HeadSubject>();
                var subject = this.GetDbSet<Subject>();

                var viewSubject = from h in headSubject join s in subject
                                  on h.Subect_Id equals s.Subject_Id
                                  select new Subject
                                  {
                                      Subject_Id = s.Subject_Id,
                                      Subject_Name = s.Subject_Name,
                                      Grade = s.Grade,
                                      HasChild = s.HasChild,
                                  };
                return viewSubject.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.InnerException.Message);
            }
        }
        public List<ChildSubjectView> GetChildSubjects(int headId)
        {
            try
            {
                var childSubject = this.GetDbSet<ChildSubject>().Where(p=> p.HeadSubjectId==headId);
                var subject = this.GetDbSet<Subject>();

                var viewSubject = from c in childSubject join
                                  s in subject on c.Subject_Id equals s.Subject_Id
                                  select new ChildSubjectView
                                  {
                                      Id = c.Id,
                                      subjectId = c.Subject_Id,
                                      Name = s.Subject_Name
                                  };
                return viewSubject.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }     
        public void RemoveChildSubject(int id)
        {
            try
            {
                var childSub = _context.ChildSubject.Find(id);
                _context.ChildSubject.Remove(childSub);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public void RemoveSubject(Subject subject)
        {
            try
            {
                _context.Subject.Remove(subject);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public Subject GetSubjectById(int id)
        {
            try
            {
                return _context.Subject.Find(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public void AddChildSubject(ChildSubject subject)
        {
            try
            {
                _context.ChildSubject.Add(subject);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public void AddHeadSubejct(HeadSubject subject)
        {
            try
            {
                _context.HeadSubject.Add(subject);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public List<HeadSubject> GetAllHeadSubject()
        {
            try
            {
                return this.GetDbSet<HeadSubject>().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

        }
    public HeadSubject GetHeadSubjectById(int id)
        {
            try
            {
                return _context.HeadSubject.Find(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public void SaveDbChanges()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public void UdpateSubject(Subject subject)
        {
            try
            {
                _context.Subject.Update(subject);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
