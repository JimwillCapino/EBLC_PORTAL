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
    public class UsersService: IUsersService 
    {
        IUsersRepository _userRepository;
        public UsersService(IUsersRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public int AddUser(Users user) 
        {
            return _userRepository.AddUser(user);
        }
    }
}
