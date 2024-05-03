using Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Interfaces
{
    public interface IStudentRepository
    {
        public void AddStudent(Student student);
        public Student GetStudent(int id);
        public Task UpdateStudentAsync(Student student);
        public IEnumerable<Student> GetAllStudent();
        public void RemoveStudent(Student student);
        public bool isExisting(UsersPortal student);
    }
}
