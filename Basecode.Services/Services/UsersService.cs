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
        public int AddUser(UsersPortal user) 
        {
            int id = _userRepository.AddUser(user);
            if (id == -1)
                throw new Exception("An error occured in the UserRepository.See the console for more info");
            return id;
        }
        public int GetMostRecentUsersId()
        {
            int id = _userRepository.GetMostRecentUsersId();
            if (id == -1)
                throw new Exception("An error occured while getting the recent UserID.");
            return id;
        }
    }
}
