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
        IRTPRepository _rTPRepository;
        public NewEnrolleeRepository(IUnitOfWork unitOfWork, 
            BasecodeContext context,
            IRTPRepository rTPRepository) : base(unitOfWork)
        {
            _context = context;
            _rTPRepository = rTPRepository;           
        }
        private IQueryable<NewEnrollee> GetEnrollees()
        {
            return this.GetDbSet<NewEnrollee>();
        }
        private IQueryable<UsersPortal> GetUsersPortal()
        {
            return this.GetDbSet<UsersPortal>();
        }
        public NewEnrollee GetEnrolleeByID(int id)
        {
            try
            {
                return _context.NewEnrollee.Find(id);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
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
                                       UID = e.Enrollee_Id,
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
                var userStudent = _context.UsersPortal.Find(student.UID);

                var parent = _context.Parent.Find(student.ParentID);
                var userParent = _context.UsersPortal.Find(parent.UID);
                var rtpCommons = _context.RTPCommons.FirstOrDefault(r => r.UID == parent.UID);

                var CompleteInfo = new RegisterStudent()
                {
                    Id = student.Enrollee_Id,
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
                    PhoneNumber = rtpCommons.PhoneNumber,
                    email = student.Email,
                    Address = rtpCommons.Address,
                    Gcash = parent.Gcash,
                    ParentBirthday = userParent.Birthday,
                    Parentsex = userParent.sex,
                };
                return CompleteInfo;
            }
            catch(Exception ex) 
            {              
                throw new Exception(ex.Message +"\n"+ex.Source+"\n" +ex.StackTrace+"\n"+ex.InnerException.Message);
            }
            
        }
        public void AddSchedule(NewEnrollee enrollee)
        {
            try
            {
                _context.NewEnrollee.Update(enrollee);
                _context.SaveChanges();
            }
            catch(Exception ex)
            {

                throw new Exception(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.InnerException.Message);
            }
        }
       public void RemoveEnrollee(NewEnrollee enrollee)
        {
            try
            {
                _context.NewEnrollee.Remove(enrollee);
                _context.SaveChanges();
                
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.InnerException.Message);
            }
        }
    }
}
