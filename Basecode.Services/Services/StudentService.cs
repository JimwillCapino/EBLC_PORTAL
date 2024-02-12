
using Basecode.Data;
using Basecode.Data.Interfaces;
using Basecode.Data.Models;
using Basecode.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Services.Services
{
    public class StudentService : IStudentService
    {
        IStudentRepository _studentRepository;
        public StudentService(IStudentRepository studentRepository) 
        { 
            _studentRepository = studentRepository;
        }
        public void AddStudent(Student student)
        {
            try
            {
                _studentRepository.AddStudent(student);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
    }
}
