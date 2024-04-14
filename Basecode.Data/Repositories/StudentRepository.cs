using Basecode.Data.Interfaces;
using Basecode.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Repositories
{
    public class StudentRepository: BaseRepository, IStudentRepository
    {
        BasecodeContext _context;

        public StudentRepository(IUnitOfWork unitOfWork, BasecodeContext context) : base(unitOfWork) 
        { 
            _context = context;
        }
        public void AddStudent(Student student)
        {
            try
            {
                _context.Student.Add(student);
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.InnerException.Message);
            }
        }
        public Student GetStudent(int id)
        {
            try
            {
                return  _context.Student.Find(id);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public Task UpdateStudentAsync(Student student)
        {
           
             return Task.Run(() => UpdateStudent(student));
            
        }

        public void UpdateStudent(Student student)
        {
            try
            {
                _context.Student.Update(student);
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
