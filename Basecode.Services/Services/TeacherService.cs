using Basecode.Data;
using Basecode.Data.Interfaces;
using Basecode.Data.Models;
using Basecode.Data.ViewModels;
using Basecode.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Services.Services
{
    public class TeacherService:ITeacherService
    {
        ITeacherRepository _TeacherRepository;
        public TeacherService(ITeacherRepository teacherRepository) 
        {
            _TeacherRepository = teacherRepository;
        }
        public void AddTeacher(Teacher teacher)
        {
            try
            {
                _TeacherRepository.AddTeacher(teacher);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public async Task<List<TeacherViewModel>> GetTeacherinitView()
        {
            try
            {
                return await _TeacherRepository.GetAllTeachersInitViewAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
    }
}
