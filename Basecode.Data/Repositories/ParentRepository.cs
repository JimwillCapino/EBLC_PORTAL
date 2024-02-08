using Basecode.Data.Interfaces;
using Basecode.Data.Models;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Repositories
{
    public class ParentRepository:BaseRepository,IParentRepository
    {
        private readonly BasecodeContext _context;
        public ParentRepository(IUnitOfWork unitOfWork, BasecodeContext context):base(unitOfWork) 
        { 
            _context = context;
        }
        public int AddParent(Parent parent)
        {
            try
            {
                var a =_context.Parent.Add(parent);
                _context.SaveChanges();
                return a.Entity.Id;
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }
        public IEnumerable<Parent> GetAllParents()  
        {
            try
            {
                return this.GetDbSet<Parent>();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }
    }
}
