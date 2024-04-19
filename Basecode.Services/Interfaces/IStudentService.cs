using Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Services.Interfaces
{
    public interface IStudentService
    {
        public void AddStudent(Student student);
        public Student GetStudent(int id);
        public void AddStudent(RegisterStudent newStudent);
        public Task UpdateStudentAsync(Student student);
    }
}
