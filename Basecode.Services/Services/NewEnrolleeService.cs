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
        IUsersService _usersService;
        IMapper _mapper;
        IRTPService _rtpService;
        IParentService _parentService;
        public NewEnrolleeService(INewEnrolleeRepository repository,
            IMapper mapper,
            IUsersService userService,
            IRTPService rTPService,
            IParentService parentService) 
        { 
            _repository = repository;
            _mapper = mapper;
            _usersService = userService;
            _parentService = parentService;
            _rtpService = rTPService;
        }
        public void RegisterStudent(RegisterStudent student)
        {
                NewEnrollee enrollee = _mapper.Map<NewEnrollee>(student);
                Constants.Enrollee.id = _usersService.AddUser(_mapper.Map<UsersPortal>(student));
                enrollee.UID = Constants.Enrollee.id;
                var parent = new UsersPortal();
                Parent parentDetails =_mapper.Map<Parent>(student);
                var rtpcommons = _mapper.Map<RTPCommons>(student);

                //Initialize parents for the users portal table
                parent.FirstName = student.ParentFirstName;
                parent.MiddleName = student.ParentMiddleName;
                parent.LastName = student.ParentLastName;
                parent.Birthday = student.ParentBirthday;
                parent.sex = student.Parentsex;

             
                //Add parents to the UsersPortal Table
                var id = _usersService.AddUser(parent);

                //Add Parents Detail the Parents Table
                parentDetails.UID = id;                
                var parentid = _parentService.AddParent(parentDetails, "Not Enrolled");

                //Add another info to the RTPCommons table
                rtpcommons.UID = id;
                _rtpService.addRTPCommons(rtpcommons);

                //Student Files processing
                if ((student.BirthCertificateFile == null && student.BirthCertificateFile.Length == 0)
                    && (student.CGMFile == null && student.CGMFile.Length == 0)
                    && (student.TORFile == null && student.TORFile.Length == 0))
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
                    enrollee.ParentID = parentid;
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
