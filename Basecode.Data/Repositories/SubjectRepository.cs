using Basecode.Data.Interfaces;
using Basecode.Data.Models;
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
        public void AddSubject(Subject subject)
        {
            try
            {
                _context.Subject.Add(subject);
                _context.SaveChanges();
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
                return this.GetDbSet<Subject>().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.InnerException.Message);
            }
        }
    }
}
