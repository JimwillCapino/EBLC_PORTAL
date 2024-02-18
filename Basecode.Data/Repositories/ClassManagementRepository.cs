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
        public void RemoveClass(Class classroom)
        {


        }
        public void DeleteClass(Class classroom)
        {

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

    }
}
