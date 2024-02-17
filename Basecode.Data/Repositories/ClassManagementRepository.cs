using Basecode.Data.Interfaces;
using Basecode.Data.Models;
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
        public ClassManagementRepository(IUnitOfWork unitOfWork, BasecodeContext context) : base(unitOfWork)
        {
            _context = context;
        }
        public void AddClass(Class classroom)
        {
            try
            {
                _context.Class.Add(classroom);
                _context.SaveChanges();
            }
           
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.InnerException.Message);
            }
        
        }
    }
}
