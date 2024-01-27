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
    public class NewEnrolleeService:INewEnrolleeService
    {
        INewEnrolleeRepository _repository;
        public NewEnrolleeService(INewEnrolleeRepository repository) 
        { 
            _repository = repository;
        }
        public void RegisterStudent(NewEnrollee newEnrollee)
        {
            if (!_repository.RegisterStudent(newEnrollee))
                throw new Exception("An error occured.See the console for more info");
        }
    }
}
