using Basecode.Data.Interfaces;
using Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Repositories
{
    public class RTPUsersRepository : BaseRepository, IRTPUsersRepository
    {
        BasecodeContext _context;
        public RTPUsersRepository(IUnitOfWork unitOfWork, BasecodeContext context) : base(unitOfWork)
        {
            _context = context; 
        }
        public void AddRTPUsers(RTPUsers rTPUsers)
        {
            try
            {
                _context.RTPUsers.Add(rTPUsers);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.InnerException.Message);
            }
        }
        public RTPUsers GetRTPuserByRTPId(int rtpId)
        {
            try
            {
                return _context.RTPUsers.FirstOrDefault(p => p.RTPId == rtpId);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public IEnumerable<RTPUsers> GetRTPUsers()
        {
            try
            {
                return this.GetDbSet<RTPUsers>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
