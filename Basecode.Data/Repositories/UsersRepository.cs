using Basecode.Data.Interfaces;
using Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Basecode.Data.Repositories
{
    public class UsersRepository : BaseRepository, IUsersRepository
    {
        private readonly BasecodeContext _context;
        public UsersRepository(IUnitOfWork unitOfWork, BasecodeContext context) : base(unitOfWork)
        {
            _context = context;
        }
        public int AddUser(UsersPortal user)
        {
            try
            {
                var a =_context.UsersPortal.Add(user);
                _context.SaveChanges();
                return a.Entity.UID;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1;
            }
        }
        public int GetMostRecentUsersId()
        {
            try
            {
                var recentuserid = _context.UsersPortal.OrderByDescending(j => j.UID).FirstOrDefault();
                return recentuserid.UID;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1;
            }
        }
    }
}
