using Basecode.Data.Models;
using Basecode.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Services.Interfaces
{
    public interface ITeacherService
    {
        public void AddTeacher(Teacher teacher);
        public IEnumerable<TeacherViewModel> GetTeacherinitView();
    }
}
