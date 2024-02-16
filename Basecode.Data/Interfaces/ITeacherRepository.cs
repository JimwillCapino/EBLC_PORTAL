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
        public Task<List<TeacherViewModel>> GetAllTeachersInitViewAsync();
    }
}
