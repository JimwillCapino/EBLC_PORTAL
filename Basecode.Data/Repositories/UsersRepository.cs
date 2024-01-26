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
        public int AddUser(Users user)
        {
            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return user.UID;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1;
            }
        }
    }
}
