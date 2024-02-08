using Basecode.Data.Models;
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
    }
}
