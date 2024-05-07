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
        public IEnumerable<Student> GetAllStudent()
        {
            try
            {
                return this.GetDbSet<Student>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public void RemoveStudent(Student student)
        {
            try
            {
                _context.Student.Remove(student);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public bool isExisting(UsersPortal student)
        {
            try
            {
                var studentTable = this.GetDbSet<Student>();
                var user = this.GetDbSet<UsersPortal>();
                var unionportal = from s in studentTable
                                  join
                                  u in user on s.UID equals u.UID
                                  select new UsersPortal
                                  {
                                      UID = u.UID,
                                      FirstName = u.FirstName,
                                      LastName = u.LastName,
                                      MiddleName = u.MiddleName,
                                      Birthday = u.Birthday,
                                      ProfilePic = u.ProfilePic,
                                      sex = u.sex,
                                  };
                var studentsCount = unionportal.Where(p => p.FirstName.ToLower().Equals(student.FirstName) && p.MiddleName.ToLower().Equals(student.MiddleName)
                && p.LastName.ToLower().Equals(student.LastName)).Count();
                return studentsCount > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
