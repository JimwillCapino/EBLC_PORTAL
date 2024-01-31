using Basecode.Data.Interfaces;
using Basecode.Data.Models;
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
                                           PhoneNumber = u.PhoneNumber,
                                           email = u.email,
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
    }
}
