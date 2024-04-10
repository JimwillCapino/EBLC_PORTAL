using AutoMapper;
using Basecode.Data;
using Basecode.Data.Interfaces;
using Basecode.Data.Models;
using Basecode.Data.ViewModels;
using Basecode.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Services.Services
{
    public class TeacherService:ITeacherService
    {
        ITeacherRepository _TeacherRepository;
        IUsersRepository _UsersRepository;
        IMapper _Mapper;
        UserManager<IdentityUser> _UserManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        IUserStore<IdentityUser> _userStore;
        public TeacherService(ITeacherRepository teacherRepository,
            IMapper mapper,
            IUsersRepository usersRepository,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserStore<IdentityUser> userStore,) 
        {
            _TeacherRepository = teacherRepository;
            _Mapper = mapper;
            _UsersRepository = usersRepository;
            _UserManager = userManager;
            _roleManager = roleManager;
            _userStore = userStore;
        }
        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
        public void AddTeacher(Teacher teacher)
        {
            try
            {
                _TeacherRepository.AddTeacher(teacher);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public void AddTeacherRegistration(TeacherRegistrarionViewModel teacher)
        {
            try
            {
                var teacherToRegister = _Mapper.Map<TeacherRegistration>(teacher);
                var userPortal = _Mapper.Map<UsersPortal>(teacher);
                _TeacherRepository.AddTeacherRegistration(teacherToRegister);
                _UsersRepository.AddUser(userPortal);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public async void ApproveTeacherRegistration(int id)
        {
            try
            {
                var teacher = _TeacherRepository.GetTeacherRegistration(id);
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, teacher.Email, CancellationToken.None);
                var result = await _UserManager.CreateAsync(user, teacher.Password);
                if(result.Succeeded)
                {
                    var userRole = _roleManager.FindByNameAsync("Teacher").Result;

                    if (userRole != null)
                        await _UserManager.AddToRoleAsync(user, userRole.Name);
                    _TeacherRepository.RemoveTeacherRegistration(id);
                }
                else
                {
                    throw new Exception("Registration of the teacher failed!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public void RejectTeacherRegistration(int id)
        {
            try
            {
                var teacher = _TeacherRepository.GetTeacherRegistration(id);
                var userdetails = _UsersRepository.GetUserById(teacher.UserPortalID);
                _UsersRepository.RemoveUser(userdetails);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public async Task<List<TeacherViewModel>> GetTeacherinitView()
        {
            try
            {
                return await _TeacherRepository.GetAllTeachersInitViewAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
    }
}
