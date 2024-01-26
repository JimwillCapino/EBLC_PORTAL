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
    public class NewEnrolleeService : INewEnrolleeService
    {
        INewEnrolleeRepository _repository;
        public NewEnrolleeService(INewEnrolleeRepository newEnrolleeRepository) 
        { 
            _repository = newEnrolleeRepository;
        }
        public int RegisterStudent(NewEnrollee newEnrollee)
        {

            return _repository.RegisterStudent(newEnrollee);
        }
    }
}
