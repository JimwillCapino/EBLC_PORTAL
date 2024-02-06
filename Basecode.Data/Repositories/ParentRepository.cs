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
        BasecodeContext _context;
        public ParentRepository(IUnitOfWork unitOfWork, BasecodeContext context):base(unitOfWork) 
        { 
            _context = context;
        }
        public void AddParent(Parent parent)
        {
            try
            {
                _context.Parent.Add(parent);
                _context.SaveChanges();
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.ToString());       
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
