using Basecode.Data.Interfaces;
using Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Repositories
{
    public class NewEnrolleeRepository : BaseRepository, INewEnrolleeRepository
    {
        private readonly BasecodeContext _context;
        public NewEnrolleeRepository(IUnitOfWork unitOfWork, BasecodeContext context) : base(unitOfWork)
        {
            _context = context;
        }
        public string RegisterStudent(NewEnrollee newEnrollee) 
        {
            try
            {
                _context.NewEnrollee.Add(newEnrollee);
                _context.SaveChanges();
                return "success";
            }
            catch (Exception ex)
            {
                return ex + " " + ex.Message;
            }
        }
    }
}
