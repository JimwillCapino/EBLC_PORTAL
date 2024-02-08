using Basecode.Data.Interfaces;
using Basecode.Data.Models;
using Basecode.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Services.Interfaces
{
    public interface INewEnrolleeService
    {
        public void RegisterStudent(RegisterStudent student);
        public IEnumerable<RegisterStudent> GetAllEnrollees();
        public IEnumerable<NewEnrolleeViewModel> GetNewEnrolleeInitView();
        public RegisterStudent GetStudent(int id);
    }
}
