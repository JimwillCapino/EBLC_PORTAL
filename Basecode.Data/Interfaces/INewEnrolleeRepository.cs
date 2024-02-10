using Basecode.Data.Models;
using Basecode.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Interfaces
{
    public interface INewEnrolleeRepository
    {
        public bool RegisterStudent(NewEnrollee newEnrollee);
        public IEnumerable<RegisterStudent> GetAllEnrollees();
        public IEnumerable<NewEnrolleeViewModel> GetNewEnrolleeInitView();
        public RegisterStudent GetStudent(int id);
        public void AddSchedule(NewEnrollee enrollee);
        public NewEnrollee GetEnrolleeByID(int id);
        public void RemoveEnrollee(NewEnrollee enrollee);
    }
}
