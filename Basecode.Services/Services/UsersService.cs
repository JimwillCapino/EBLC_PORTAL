﻿using AutoMapper;
using Basecode.Data;
using Basecode.Data.Interfaces;
using Basecode.Data.Models;
using Basecode.Data.ViewModels;
using Basecode.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Services.Services
{
    public class UsersService: IUsersService 
    {
        IUsersRepository _userRepository;
        IMapper _mapper;
        IRTPRepository _rtpRepository;
        UserManager<IdentityUser> _userManager;
        INewEnrolleeRepository _newEnrolleeRepository;
        ITeacherRepository _teacherRepository;
        IStudentRepository _studentRepository;
        IRTPUsersRepository _rtpUsersRepository;
        IClassManagementRepository _classManagementRepository;
        IStudentManagementRepository _studentManagementRepository;
        IStudentManagementService _studentManagementService;
        IAdminRepository _adminRepository;
        public UsersService(IUsersRepository userRepository,
            IMapper mapper,
            UserManager<IdentityUser> userManager,
            IRTPRepository rtpRepository,
            INewEnrolleeRepository newEnrolleeRepository,
            ITeacherRepository teacherRepository,
            IStudentRepository studentRepository,
            IRTPUsersRepository rtpUsersRepository,
            IStudentManagementRepository studentManagementRepository,
            IClassManagementRepository classManagementRepository,
            IStudentManagementService studentManagementService,
            IAdminRepository adminRepository
            )
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userManager = userManager;
            _rtpRepository = rtpRepository;
            _newEnrolleeRepository = newEnrolleeRepository;
            _teacherRepository = teacherRepository;
            _studentRepository = studentRepository;
            _rtpUsersRepository = rtpUsersRepository;        
            _studentManagementRepository = studentManagementRepository;
            _classManagementRepository = classManagementRepository;
            _studentManagementService = studentManagementService;
            _adminRepository = adminRepository;
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
        public void RemoveUser(UsersPortal user)
        {
            try
            {
                _userRepository.RemoveUser(user);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public UsersPortal GetUserById(int id)
        {
            try
            {
                return _userRepository.GetUserById(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public async Task NewUserDetailsRegistration(ProfileViewModel profile)
        {
            try
            {
                var userPortal = _mapper.Map<UsersPortal>(profile);
                var rtpCommons = _mapper.Map<RTPCommons>(profile);
                RTPUsers rtpUsers = new RTPUsers();
                var user = await _userManager.FindByIdAsync(profile.AspUserId);
                var changePasswordResult = _userManager.ChangePasswordAsync(user, profile.CurrentPassword, profile.NewPassword).Result;

                if (!changePasswordResult.Succeeded)
                {
                    string errors = "";
                    foreach (var error in changePasswordResult.Errors)
                    {
                        errors += error.Description + " ";
                    }
                    throw new Exception(errors);
                }

                if (profile.ProfilePicRecieve != null)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        profile.ProfilePicRecieve.CopyTo(stream);
                        userPortal.ProfilePic = stream.ToArray();
                    }
                }
                else
                {
                    userPortal.ProfilePic = profile.ProfilePic;
                }               
                int uid = _userRepository.AddUser(userPortal);
                rtpCommons.UID = uid;
                int RTPCommonsId =  _rtpRepository.addRTPCommonsInt(rtpCommons);
                rtpUsers.AspUserId = profile.AspUserId;
                rtpUsers.RTPId = RTPCommonsId;
                _rtpUsersRepository.AddRTPUsers(rtpUsers);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(e.Message);
            }
        }
        public async Task<bool> IsNewUser(string AspUserId)
        {
            try
            {
                return await _userRepository.IsNewUser(AspUserId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public async Task UpdateUserProfile(ProfileViewModel profile)
        {
            try
            {
                var userPortal =  _mapper.Map<UsersPortal>(profile);
                var rtpCommons = _mapper.Map<RTPCommons>(profile);

                rtpCommons.Id = profile.RTPCommonsId;
                var user = await _userManager.FindByIdAsync(profile.AspUserId);
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                await _userManager.ChangeEmailAsync(user, profile.Email, token);
                if(profile.ProfilePicRecieve != null)
                {
                    using(MemoryStream stream = new MemoryStream()) 
                    {
                        profile.ProfilePicRecieve.CopyTo(stream);
                        userPortal.ProfilePic = stream.ToArray();
                    }
                }
                else
                {
                    userPortal.ProfilePic = profile.ProfilePic;
                }
                await _userRepository.UpdateUserPortal(userPortal);
                await _rtpRepository.UpdateCommonsAsync(rtpCommons);               
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public async Task ChangePassword(ProfileViewModel profile)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(profile.AspUserId);
                var changePasswordResult = _userManager.ChangePasswordAsync(user, profile.CurrentPassword, profile.NewPassword).Result;
                if(!changePasswordResult.Succeeded)
                {
                    string errors = "";
                    foreach(var error in changePasswordResult.Errors)
                    {
                        errors += error.Description+" ";
                    }
                    throw new Exception(errors);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(e.Message);
            }
        }
        public async Task<ProfileViewModel> GetUserPortal(string AspUserId)
        {
            try
            {
                return await _userRepository.GetUserPortal(AspUserId);
            }            
            catch(NullReferenceException e)
            {
                throw new NullReferenceException(e.Message);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(Constants.Exception.DB);
            }
        }
        
        public async Task<RegistrarDashboard> SetRegisrarDashBoard()
        {
            try
            {
                var registrarDashboard = new RegistrarDashboard();                
                registrarDashboard.NewEnrolleeCount = _newEnrolleeRepository.GetAllEnrollees().Count();
                registrarDashboard.TeacherCount = _teacherRepository.GetAllTeachersInitViewAsync().Result.Count();
                registrarDashboard.StudentEnrolledCount = _studentRepository.GetAllStudent().Where(p => p.status == "Enrolled").Count();
                registrarDashboard.StudentNotEnrolledCount = _studentRepository.GetAllStudent().Where(p => p.status == "Not Enrolled").Count();
                TimeZoneInfo phTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Manila");
                // Get the current time in the Philippines
                DateTime phTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, phTimeZone);
                registrarDashboard.NewEnrolleeList = _newEnrolleeRepository.GetNewEnrolleeInitView().Where(P => P.examschedule != null && phTime.Date.Equals(DateTime.Parse(P.examschedule).Date))

                        .OrderBy(p => DateTime.Parse(p.examschedule).ToShortTimeString()).ToList();
                return registrarDashboard;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public async Task<AdminDashboard> SetDashboardDashBoard()
        {
            try
            {
                var registrarDashboard = new AdminDashboard();                
                registrarDashboard.TeacherCount = _teacherRepository.GetAllTeachersInitViewAsync().Result.Count();
                registrarDashboard.StudentEnrolledCount = _studentRepository.GetAllStudent().Where(p => p.status == "Enrolled").Count();
                registrarDashboard.StudentNotEnrolledCount = _studentRepository.GetAllStudent().Where(p => p.status == "Not Enrolled").Count();
                registrarDashboard.RegistrarsCount = _adminRepository.GetRegistrarsAsync().Result.Count();              
                return registrarDashboard;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public TeacherDashboard SetTeacherDashboard(string teacherid)
        {
            try
            {
                var teacherDashboard = new TeacherDashboard();
                teacherDashboard.NumberOfClass = _classManagementRepository.GetTeacherClassDetails(teacherid).Count();
                teacherDashboard.NumberOfHomeroom = _classManagementRepository.GetTeacherHomeRoom(teacherid).Count();
                //teacherDashboard.ListOfStudentsWithNoGrade = _studentManagementService.
                return teacherDashboard;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(Constants.Exception.DB);
            }

        }

    }
}
