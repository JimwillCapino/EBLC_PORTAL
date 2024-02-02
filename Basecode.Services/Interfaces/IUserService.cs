using Basecode.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Basecode.Services.Interfaces
{
    public interface IUserService
    {
        IdentityUser FindByUsername(string username);
        IdentityUser FindById(string id);
        IdentityUser FindUser(string userName);
        IEnumerable<IdentityUser> FindAll();
        bool Create(IdentityUser user);
        bool Update(IdentityUser user);
        void Delete(IdentityUser user);
        Task<IdentityResult> RegisterUser(string username, string password, string firstName, string lastName, string email, string role);
        Task<IdentityResult> CreateRole(string roleName);
        Task<IdentityUser> FindUser(string username, string password);
        Task<IdentityUser> FindUserAsync(string userName, string password);
    }
}
