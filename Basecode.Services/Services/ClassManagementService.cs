using Basecode.Data;
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
    public class ClassManagementService:IClassManagementService
    {
        IClassManagementRepository _repository;
        public ClassManagementService(IClassManagementRepository classManagementRepository) 
        { 
            _repository = classManagementRepository;
        }
        public void AddClass(Class classroom)
        {
            try
            {
                _repository.AddClass(classroom);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
    }
}
