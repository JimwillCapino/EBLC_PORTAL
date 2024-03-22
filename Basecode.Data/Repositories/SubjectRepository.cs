using Basecode.Data.Interfaces;
using Basecode.Data.Models;
using Basecode.Data.ViewModels;
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
