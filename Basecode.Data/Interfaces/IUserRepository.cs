using Basecode.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Basecode.Data.Interfaces
{
    public interface IUserRepository
    {
        IdentityUser FindByUsername(string username);
        IdentityUser FindById(string id);
        IdentityUser FindUser(string UserName);
        IEnumerable<IdentityUser> FindAll();
        bool Create(IdentityUser user);
        bool Update(IdentityUser user);
        void Delete(IdentityUser user);
        Task<IdentityResult> RegisterUser(string username, string password, string firstName, string lastName, string email, string role);
        Task<IdentityResult> CreateRole(string roleName);
        Task<IdentityUser> FindUser(string userName, string password);
        Task<IdentityUser> FindUserAsync(string userName, string password);
    }
}
