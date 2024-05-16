using AutoMapper;
using Basecode.Data;
using Basecode.Data.Interfaces;
using Basecode.Data.Models;
using Basecode.Data.ViewModels;
using Basecode.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
namespace Basecode.Services.Services
{
    public class NewEnrolleeService:INewEnrolleeService
    {
        UserManager<IdentityUser> _userManager;
        INewEnrolleeRepository _repository;
        IUsersService _usersService;
        IMapper _mapper;
        IRTPService _rtpService;
        IParentService _parentService;
        IStudentService _studentService;
        IRTPUsersRepository _rtpUserRepository;
        ISettingsRepository _settingsRepository;
        public NewEnrolleeService(INewEnrolleeRepository repository,
            IMapper mapper,
            IUsersService userService,
            IRTPService rTPService,
            IParentService parentService,
            IStudentService studentService,
            UserManager<IdentityUser> userManager,
            IRTPUsersRepository rTPUsersRepository,
            ISettingsRepository settingsRepository) 
        { 
            _repository = repository;
            _mapper = mapper;
            _usersService = userService;
            _parentService = parentService;
            _rtpService = rTPService;
            _studentService = studentService;
            _userManager = userManager;
            _rtpUserRepository = rTPUsersRepository;
            _settingsRepository = settingsRepository;
        }
        public void RegisterStudent(RegisterStudent student)
        {
                NewEnrollee enrollee = _mapper.Map<NewEnrollee>(student);
                var studentUser = _mapper.Map<UsersPortal>(student);

            if(student.ProfilePicRecieve != null)
            {
                using (MemoryStream profilePic = new MemoryStream())
                {
                    student.ProfilePicRecieve.CopyTo(profilePic);
                    studentUser.ProfilePic = profilePic.ToArray();
                }
            }
            else
            {
                student.ProfilePic = null;
            }


                Constants.Enrollee.id = _usersService.AddUser(studentUser);
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

                //Add Parents Detail to the Parents Table
                parentDetails.UID = id;                
                var parentid = _parentService.AddParent(parentDetails);

                //Add another info to the RTPCommons table
                rtpcommons.UID = id;
                _rtpService.addRTPCommons(rtpcommons);

                //Student Files processing
                if ((student.BirthCertificateFile == null && student.BirthCertificateFile.Length == 0)
                    && (student.CGMFile == null && student.CGMFile.Length == 0)
                    && (student.TORFile == null && student.TORFile.Length == 0))
                {
                    enrollee.BirthCertificate = null;
                    enrollee.CGM = null;
                    enrollee.TOR = null;
                }
                else
                {
                    using (MemoryStream birthCertificateMemory = new MemoryStream())
                    {
                        student.BirthCertificateFile.CopyTo(birthCertificateMemory);
                        enrollee.BirthCertificate = birthCertificateMemory.ToArray();
                    }

                    using (MemoryStream cgmMemory = new MemoryStream())
                    {
                        student.CGMFile.CopyTo(cgmMemory);
                        enrollee.CGM = cgmMemory.ToArray();
                    }

                    using (MemoryStream torMemory = new MemoryStream())
                    {
                        student.TORFile.CopyTo(torMemory);
                        enrollee.TOR = torMemory.ToArray();
                    }
                    using (MemoryStream birthCertificateMemory = new MemoryStream())
                    {
                        student.BirthCertificateFile.CopyTo(birthCertificateMemory);
                        enrollee.BirthCertificate = birthCertificateMemory.ToArray();
                    }

                    using (MemoryStream cgmMemory = new MemoryStream())
                    {
                        student.CGMFile.CopyTo(cgmMemory);
                        enrollee.CGM = cgmMemory.ToArray();
                    }

                    using (MemoryStream torMemory = new MemoryStream())
                    {
                        student.TORFile.CopyTo(torMemory);
                        enrollee.TOR = torMemory.ToArray();
                    }              
                }

               enrollee.ParentID = parentid;
               if (!_repository.RegisterStudent(enrollee))
                   throw new Exception(Constants.Exception.DB);
        }
        public IEnumerable<RegisterStudent> GetAllEnrollees()
        {
            return _repository.GetAllEnrollees();
        }
        public IEnumerable<NewEnrolleeViewModel> GetNewEnrolleeInitView()
        {
            try
            {
                return _repository.GetNewEnrolleeInitView();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                throw;
            }
        }

        public RegisterStudent GetStudent(int id)
        {
            try
            {
                return _repository.GetStudent(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool isScheduleConflict(string schedule)
        {
            try
            {
                var sched = _repository.GetNewEnrolleeInitView().ToList();
                var filteredSched = sched.Where(p => p.examschedule == schedule);
                return filteredSched.Count() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void AddSchedule(int id, string Schedule)
        {
            try
            {
               //DateTime parseSched = DateTime.ParseExact(schedule, "2024-02-10 12:30:45", CultureInfo.InvariantCulture);
                var enrollee = _repository.GetEnrolleeByID(id);
                enrollee.ExamSchedule = Schedule;
                if (isScheduleConflict(Schedule))
                    throw new Exception("Selected schedule conflicts with other schedule");
                _repository.AddSchedule(enrollee);

                var parent = _parentService.GetParentById(enrollee.ParentID);

                // Send Email to parent
                MailMessage mail = new MailMessage();
                mail.To.Add(parent.Email);
                mail.From = new MailAddress(_settingsRepository.GetSchoolEmail());
                mail.Subject = "Enrollment in EBLC";
                mail.Body = "Greetings! Your child has been scheduled to a screening examination on " 
                   + Environment.NewLine
                   + DateTime.Parse(Schedule).ToLongDateString() +" "+ DateTime.Parse(Schedule).ToLongTimeString()
                   + " "
                   + "."
                   + Environment.NewLine
                   + "Please make sure to attend on time."
                   + Environment.NewLine
                   + "Thank you and God bless.";

                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(_settingsRepository.GetSchoolEmail(), _settingsRepository.GetPassword()); // Enter seders User name and password       
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex);
                throw new Exception(ex.Message);
            }
            
        }
        public void RemoveNewEnrollee(NewEnrollee enrollee)
        {
            try
            {
                _repository.RemoveEnrollee(enrollee);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public void RejectNewEnrollee(int id)
        {
            try
            {
                var newEnrolle = _repository.GetEnrolleeByID(id);
                var userNewEnrolle = _usersService.GetUserById(newEnrolle.UID);
                var parent =  _parentService.GetParentById(newEnrolle.ParentID);
                var parentUser = _usersService.GetUserById(parent.UID);
                //var parentRTP = _rtpService.GetRTPCommonsByUID(parent.UID);
                var email = parent.Email;
                _repository.RemoveEnrollee(newEnrolle);
                _usersService.RemoveUser(userNewEnrolle);
                _parentService.RemoveParent(parent);
                //_rtpService.RemoveRTP(parentRTP);
                _usersService.RemoveUser(parentUser);

                // Send Email to parent
                MailMessage mail = new MailMessage();
                mail.To.Add(email);
                mail.From = new MailAddress(_settingsRepository.GetSchoolEmail());
                mail.Subject = "Enrollment in EBLC";
                mail.Body = "Greetings from the EBLC management. We regret to inform you that your application to enroll has been rejected. If you have any questions regarding the rejection, please feel free to reach out to us.";
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(_settingsRepository.GetSchoolEmail(), _settingsRepository.GetPassword()); // Enter seders User name and password       
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public void AdmitNewEnrollee(int id, string lrn)
        {
            try
            {
                var newenrollee = _repository.GetEnrolleeByID(id);                
                var student = new Student();
                student.UID = newenrollee.UID;
                student.ParentId = newenrollee.ParentID;
                student.CurrGrade = newenrollee.GradeEnrolled;
                student.status = "Enrolled";
                student.LRN = lrn;               
                _studentService.AddStudent(student);
                _repository.RemoveEnrollee(newenrollee);

                var parent = _parentService.GetParentById(newenrollee.ParentID);

                // Send Email to parent
                MailMessage mail = new MailMessage();
                mail.To.Add(parent.Email);
                mail.From = new MailAddress(_settingsRepository.GetSchoolEmail());
                mail.Subject = "Enrollment in EBLC";
                mail.Body = "Congratulations! Your child has been accepted. Please go to the registrar's office to complete the enrollment process."
                           + Environment.NewLine
                           + "Additionally, please bring hard copies of the following documents:"
                           + Environment.NewLine
                           + "- Birth Certificate"
                           + Environment.NewLine
                           + "- Good Moral Certificate"
                           + Environment.NewLine
                           + "- Transcript of Records"
                           + Environment.NewLine
                           + "Thank you and God bless."; ;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(_settingsRepository.GetSchoolEmail(),_settingsRepository.GetPassword()); // Enter seders User name and password       
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
    }
}
