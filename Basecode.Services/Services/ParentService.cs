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
    public class ParentService: IParentService
    {
        IParentRepository _parentRepository;

        public ParentService(IParentRepository parentRepository)
        {
            _parentRepository = parentRepository;
        }
        public void AddParent(Parent parent)
        {
            _parentRepository.AddParent(parent);
        }
        public IEnumerable<Parent> GetAllParents()
        {
            return _parentRepository.GetAllParents();
        }
    }
}
