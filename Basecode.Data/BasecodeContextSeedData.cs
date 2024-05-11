using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data
{
    public class BasecodeContextSeedData
    {
        private BasecodeContext _context;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public BasecodeContextSeedData(BasecodeContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async void SeedAdminUser()
        {
            var user = new IdentityUser
            {
                UserName = "registrarAdmin1@gmail.com",
                Email = "registrarAdmin1@gmail.com"
            };
            var userAdmin = new IdentityUser
            {
                UserName = "Admin1@gmail.com",
                Email = "Admin1@gmail.com"
            };

            //var roleStore = new RoleStore<IdentityRole>(_context);
            bool checkIfRoleExists =  _roleManager.RoleExistsAsync("Registrar").Result;
            bool checkIfAdminRoleExists = _roleManager.RoleExistsAsync("Admin").Result;

            if (!checkIfRoleExists)
            {
                var role = new IdentityRole();
                role.Name = "Registrar";
                await _roleManager.CreateAsync(role);
            }
            if(!checkIfAdminRoleExists)
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                await _roleManager.CreateAsync(role);
            }

            if (!_context.Users.Any(u => u.UserName == user.UserName))
            {               
                 var result = _userManager.CreateAsync(user,"Password_123").Result;
                 var created = _userManager.AddToRoleAsync(user, "Registrar").Result;
                Console.WriteLine(created.Succeeded);
            }
            if (!_context.Users.Any(u => u.UserName == userAdmin.UserName))
            {
                var result = _userManager.CreateAsync(userAdmin, "Password_123").Result;
                var created = _userManager.AddToRoleAsync(userAdmin, "Admin").Result;
                Console.WriteLine(created.Succeeded);
            }

            await _context.SaveChangesAsync();
        }
    }
}
