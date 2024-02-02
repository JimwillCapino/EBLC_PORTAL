using AutoMapper;
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
    public class NewEnrolleeService:INewEnrolleeService
    {
        INewEnrolleeRepository _repository;
        IUsersService _userService;
        IMapper _mapper;
        public NewEnrolleeService(INewEnrolleeRepository repository,IMapper mapper,IUsersService userService) 
        { 
            _repository = repository;
            _mapper = mapper;
            _userService = userService;
        }
        public void RegisterStudent(RegisterStudent student)
        {
            NewEnrollee enrollee = _mapper.Map<NewEnrollee>(student);
            enrollee.UID = Constants.Enrollee.id;
            //Console.Write(registerStudent.BirthCertificate.Length);
            if ((student.BirthCertificateFile == null && student.BirthCertificateFile.Length == 0) 
                && (student.CGMFile == null && student.CGMFile.Length == 0)
                && (student.TORFile ==null && student.TORFile.Length ==0))
            {
                Console.WriteLine("File empty");
                throw new Exception(Constants.Attachment.FileEmpty);
            }
            else
            {       
                using (MemoryStream memory = new MemoryStream())
                {
                    student.BirthCertificateFile.CopyTo(memory);
                    enrollee.BirthCertificate = memory.ToArray();
                    
                    student.CGMFile.CopyTo(memory);
                    enrollee.CGM = memory.ToArray();
                    

                    student.TORFile.CopyTo(memory);
                    enrollee.TOR = memory.ToArray();
                   
                }
                if (!_repository.RegisterStudent(enrollee))
                    throw new Exception(Constants.Exception.DB);
            }         
        }
        public IEnumerable<RegisterStudent> GetAllEnrollees()
        {
            return _repository.GetAllEnrollees();
        }
    }
}
