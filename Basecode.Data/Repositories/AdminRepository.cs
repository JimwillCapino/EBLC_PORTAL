using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basecode.Data.Interfaces;
using Basecode.Data.Models;
using Microsoft.AspNetCore.Identity;
namespace Basecode.Data.Repositories
{
    public class AdminRepository:BaseRepository, IAdminRepository
    {
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private BasecodeContext _context; 
        public AdminRepository(IUnitOfWork unitOfWork,UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) : base(unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }   
        public async Task<IdentityResult>CreateRole(string roleName)
        {
            bool checkIfRoleExists = await _roleManager.RoleExistsAsync(roleName);
            //var test = await _roleManager.FindByNameAsync(roleName);
            if(!checkIfRoleExists)
            {
                var role = new IdentityRole();
                role.Name = roleName;
                var result = await _roleManager.CreateAsync(role);
                return result;
            }
            return null;
        }
        public void AddUser(UsersPortal usersPortal)
        {
            try
            {
                _context.UsersPortal.Add(usersPortal);
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
        public void UpdateAdminProfile()
        {
                            
        }
    }
}
