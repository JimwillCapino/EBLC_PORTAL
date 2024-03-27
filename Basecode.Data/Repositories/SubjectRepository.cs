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
       public List<Subject> GetAllSubjects(int studentId, string schoolYear) 
       {
            try
            {
                List<Subject> ChildSubjectContainer = new List<Subject>();
                var mainClass = this.GetDbSet<Class>().Where(p => p.SchoolYear == schoolYear);
                var mainClassStudent = this.GetDbSet<ClassStudents>().Where(p => p.Student_Id == studentId);
                var mainClassSubjects = this.GetDbSet<ClassSubjects>();
                var referenceSubjcect = this.GetDbSet<Subject>();
                var nochildsub = this.GetDbSet<Subject>().Where(p => !p.HasChild);
                var subjects = this.GetDbSet<HeadSubject>();
                var HeadSubs = this.GetDbSet<Subject>().Where(p => p.HasChild);
                var childSubs = this.GetDbSet<ChildSubject>();

                var unionMainSubjects = from mclass in mainClass
                                        join student in mainClassStudent
                                        on mclass.Id equals student.Class_Id
                                        join subject in mainClassSubjects
                                        on mclass.Id equals subject.ClassId
                                        join nc in nochildsub
                                        on subject.Subject_Id equals nc.Subject_Id
                                        join s in subjects 
                                        on nc.Subject_Id equals s.Subect_Id
                                        select new Subject
                                        {
                                            Subject_Id = nc.Subject_Id,
                                            Subject_Name = nc.Subject_Name,
                                            Grade = nc.Grade,
                                            HasChild = nc.HasChild,
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
                var concat = unionMainSubjects.ToList().Concat(ChildSubjectContainer);
                return concat.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
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
        public List<ChildSubjectView> GetChildSubject(int headId)
        {
            try
            {
                var childSubject = this.GetDbSet<ChildSubject>().Where(p=> p.HeadSubjectId==headId);
                var subject = this.GetDbSet<Subject>();

                var viewSubject = from c in childSubject join
                                  s in subject on c.Subject_Id equals s.Subject_Id
                                  select new ChildSubjectView
                                  {
                                      Id = c.Subject_Id,
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
    }
}
