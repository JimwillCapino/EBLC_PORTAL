using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basecode.Data.Interfaces;
using Basecode.Data.Models;
using Basecode.Data.ViewModels;
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
        public async Task AddRegistrar(UsersRegistration account)
        {
            try
            {
                var user = new IdentityUser()
                {
                    UserName = account.UserName,
                    Email = account.EmailAddress
                };
                var result = await _userManager.CreateAsync(user, account.Password);
                if (!result.Succeeded)
                    throw new Exception("Something went wrong during Registration");
                else
                {
                    var userRole = _roleManager.FindByNameAsync("Registrar").Result;

                    if (userRole != null)
                        await _userManager.AddToRoleAsync(user, userRole.Name);
                    else
                    {
                        var role = new IdentityRole()
                        {
                            Name = "Registrar",
                        };
                        var resultRole = _roleManager.CreateAsync(role).Result;
                        if (resultRole.Succeeded)
                            await _userManager.AddToRoleAsync(user, role.Name);
                        else
                            throw new Exception("Something went wrong during adding of role.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task<List<TeacherViewModel>> GetRegistrarsAsync()
        {
            try
            {
                var aspuser = await _userManager.GetUsersInRoleAsync("Registrar");
                var rtpuser = this.GetDbSet<RTPUsers>().ToList();
                var rtpcommons = this.GetDbSet<RTPCommons>().ToList();
                var usersportal = this.GetDbSet<UsersPortal>().ToList();

                var test = aspuser.Select(p => new
                {
                    Id = p.Id,
                    Email = p.Email
                });

                var teacher = from au in test
                              join rtu in rtpuser on au.Id equals rtu.AspUserId
                              join rtc in rtpcommons on rtu.RTPId equals rtc.Id
                              join users in usersportal on rtc.UID equals users.UID
                              select new TeacherViewModel
                              {
                                  firstname = users.FirstName,
                                  lastname = users.LastName,
                                  middlename = users.MiddleName,
                                  gender = users.sex,
                                  id = au.Id,
                                  email = au.Email,
                                  profilepic = users.ProfilePic
                              };
                return teacher.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.InnerException.Message);
            }
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
