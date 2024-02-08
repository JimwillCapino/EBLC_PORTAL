using Basecode.Data.Interfaces;
using Basecode.Data.Models;
using Basecode.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Repositories
{
    public class NewEnrolleeRepository : BaseRepository, INewEnrolleeRepository
    {
        BasecodeContext _context;
        public NewEnrolleeRepository(IUnitOfWork unitOfWork, BasecodeContext context) : base(unitOfWork)
        {
            _context = context;
        }
        private IQueryable<NewEnrollee> GetEnrollees()
        {
            return this.GetDbSet<NewEnrollee>();
        }
        private IQueryable<UsersPortal> GetUsersPortal()
        {
            return this.GetDbSet<UsersPortal>();
        }
        public bool RegisterStudent(NewEnrollee newEnrollee)
        {
            try
            {
                _context.NewEnrollee.Add(newEnrollee);               
                _context.SaveChanges();
                return true;
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            
        }
        public IEnumerable<RegisterStudent> GetAllEnrollees()
        {
            try
            {
                var newEnrollee = this.GetEnrollees();
                var usersPortal = this.GetUsersPortal();

                var registerStudents = from ne in newEnrollee
                                       join u in usersPortal on ne.UID equals u.UID
                                       select new RegisterStudent
                                       {
                                           FirstName = u.FirstName,
                                           LastName = u.LastName,
                                           MiddleName = u.MiddleName,
                                           //PhoneNumber = u.PhoneNumber,
                                           sex = u.sex,
                                           BirthCertificateRecieve = ne.BirthCertificate,
                                           CGMRecieve = ne.CGM,
                                           TORRecieve = ne.TOR
                                       };
                return registerStudents;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception(ex.ToString());
            }

        }
        public IEnumerable<NewEnrolleeViewModel> GetNewEnrolleeInitView()
        {
            try
            {
                var enrollees = this.GetEnrollees();
                var usersPortal = this.GetUsersPortal();

                var newviewmodel = from e in enrollees
                                   join u in usersPortal on e.UID equals u.UID
                                   select new NewEnrolleeViewModel
                                   {
                                       UID = e.UID,
                                       FirstName = u.FirstName,
                                       Middlename = u.MiddleName,
                                       LastName = u.LastName,
                                       sex = u.sex,                                   
                                       GradeEnrolled = e.GradeEnrolled,
                                       Birthday = u.Birthday
                                   };
                return newviewmodel;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception();
            }
        }
        public RegisterStudent GetStudent(int id)
        {
            try
            {
                var student = _context.NewEnrollee.Find(id);
                var parent = _context.Parent.FirstOrDefault(p => p.UID == student.ParentID);
                var userStudent = _context.UsersPortal.FirstOrDefault(us => us.UID == student.UID);
                var userParent = _context.UsersPortal.FirstOrDefault(up => up.UID == parent.UID);
                var rtpcommons = _context.RTPCommons.FirstOrDefault(r => r.UID == parent.UID);

                var CompleteInfo = new RegisterStudent()
                {
                    FirstName = userStudent.FirstName,
                    LastName = userStudent.LastName,
                    MiddleName = userStudent.MiddleName,
                    Birthday = userStudent.Birthday,
                    BirthCertificateRecieve =student.BirthCertificate,
                    CGMRecieve = student.CGM,
                    TORRecieve = student.TOR,
                    sex = userStudent.sex,
                    GradeEnrolled = student.GradeEnrolled,
                    ParentFirstName = userParent.FirstName,
                    ParentLastName = userParent.LastName,
                    ParentMiddleName = userParent.MiddleName,
                    PhoneNumber = rtpcommons.PhoneNumber,
                    email = student.Email,
                    Address = rtpcommons.Address,
                    Gcash = parent.Gcash,
                    ParentBirthday = userParent.Birthday,
                    Parentsex = userParent.sex,
                };
                return CompleteInfo;
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
                throw new Exception(Constants.Exception.DB);
            }
            
        }
    }
}
