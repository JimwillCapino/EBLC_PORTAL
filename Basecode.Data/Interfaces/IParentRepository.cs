using Basecode.Data.Models;
using Basecode.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Interfaces
{
    public interface IParentRepository
    {
        public int AddParent(Parent parent);
        public IEnumerable<Parent> GetAllParents();
        public void RemoveParent(Parent parent);
        public Parent GetParentById(int id);
        public  Task<ParentDetails> GetParentDetailById(int studentId);
        public Task UpdateParentAsyn(Parent parent);
    }
}
