using Basecode.Data.Models;
using Basecode.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Interfaces
{
    public interface ITeacherRepository
    {
        public void AddTeacher(Teacher teacher);
        public void AddTeacherRegistration(TeacherRegistration teacher);
        public void RemoveTeacherRegistration(int id);
        public TeacherRegistration GetTeacherRegistration(int id);
        public List<TeacherRegistrarionViewModel> GetAllTeacherApplicants();

        public Task<List<TeacherViewModel>> GetAllTeachersInitViewAsync();
    }
}
