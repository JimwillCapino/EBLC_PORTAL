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
    public class AdminService:IAdminService
    {
        IAdminRepository _adminRepository;
        ITeacherRepository _TeacherRepository;
        IUsersRepository _UsersRepository;
        IMapper _Mapper;
        UserManager<IdentityUser> _UserManager;
        RoleManager<IdentityRole> _roleManager;
        IRTPRepository _rtpCommons;
        IRTPUsersRepository _rtpusers;
        public AdminService(IAdminRepository adminRepository, ITeacherRepository teacherRepository, IUsersRepository usersRepository, IRTPRepository rTPRepository, IRTPUsersRepository rTPUsersRepository
            ,UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _adminRepository = adminRepository;
            _TeacherRepository = teacherRepository;
            _UsersRepository = usersRepository;
            _rtpCommons = rTPRepository;
            _rtpusers = rTPUsersRepository;
            _UserManager = userManager;
            _roleManager = roleManager;

        }
        public async Task<IdentityResult> CreateRole(string roleName)
        {
            return await _adminRepository.CreateRole(roleName);
        }
        public void AddUser(UsersPortal usersPortal)
        {
            _adminRepository.AddUser(usersPortal);
        }
        public async Task AddRegistrar(UsersRegistration registration)
        {
            try
            {
                await _adminRepository.AddRegistrar(registration);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<TeacherViewModel>> GetRegistrarsAsync()
        {
            try
            {
                return await _adminRepository.GetRegistrarsAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<TeacherViewModel>> GetTeachersAsync()
        {
            try
            {
                return await _TeacherRepository.GetAllTeachersInitViewAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task RemoveRegistrar(string aspId)
        {
            try
            {
                var user = await _UserManager.FindByIdAsync(aspId);
                var rtpid = _rtpusers.GetRTPUsers().FirstOrDefault(p => p.AspUserId == aspId).RTPId;
                var uid = _rtpCommons.getRTPCommons().FirstOrDefault(p => p.Id == rtpid).UID;
                var userportal = _UsersRepository.GetUserById(uid);
                await _UserManager.DeleteAsync(user);
                _UsersRepository.RemoveUser(userportal);
            }
            catch (NullReferenceException ex)
            {
                throw new Exception("User not found");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB + ". Ensure that the teacher is not an adviser on any class.");
            }
        }
    }
}
