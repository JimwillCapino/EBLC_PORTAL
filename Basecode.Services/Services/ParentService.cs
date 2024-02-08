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
        private readonly IParentRepository _parentRepository;

        public ParentService(IParentRepository parentRepository)
        {
            _parentRepository = parentRepository;
        }
        public int AddParent(Parent parent, string status)
        {
            parent.status = status;
            return _parentRepository.AddParent(parent);
        }
        public IEnumerable<Parent> GetAllParents()
        {
            return _parentRepository.GetAllParents();
        }
    }
}
