using Basecode.Data.Models;
using Basecode.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Services.Interfaces
{
    public interface IUsersService
    {
        public int AddUser(UsersPortal user);
        public int GetMostRecentUsersId();
        public void RemoveUser(UsersPortal user);
        public UsersPortal GetUserById(int id);
        public Task UpdateUserProfile(ProfileViewModel profile);
        public Task ChangePassword(ProfileViewModel profile);
        public Task<ProfileViewModel> GetUserPortal(string AspUserId);
    }
}
