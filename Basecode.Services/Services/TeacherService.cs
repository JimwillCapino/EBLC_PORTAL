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
using System.Net.Mail;
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
        RoleManager<IdentityRole> _roleManager;
        IRTPRepository _rtpCommons;
        IRTPUsersRepository _rtpusers;
        ISettingsRepository _settingsRepository;
        public TeacherService(ITeacherRepository teacherRepository,
            IMapper mapper,
            IUsersRepository usersRepository,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IRTPRepository rtpRepoistory,
            IRTPUsersRepository rtpusers,
            ISettingsRepository settingsRepository
            ) 
        {
            _TeacherRepository = teacherRepository;
            _Mapper = mapper;
            _UsersRepository = usersRepository;
            _UserManager = userManager;
            _roleManager = roleManager;
            _rtpCommons = rtpRepoistory;
            _rtpusers = rtpusers;
            _settingsRepository = settingsRepository;
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
                var rtpCommons = _Mapper.Map<RTPCommons>(teacher);                
                var uid =_UsersRepository.AddUser(userPortal);
                rtpCommons.UID = uid;
                teacherToRegister.UserPortalID = uid;              
                _TeacherRepository.AddTeacherRegistration(teacherToRegister);
                _rtpCommons.addRTPCommons(rtpCommons);                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public async Task AddTeacherUserAccount(UsersRegistration account)
        {
            try
            {
                var teacherUser = new IdentityUser
                {
                    UserName = account.EmailAddress, 
                    Email = account.EmailAddress                    
                };

                var result = await _UserManager.CreateAsync(teacherUser, account.Password);
                             
                if (result.Succeeded)
                {
                    var userRole = _roleManager.FindByNameAsync("Teacher").Result;                  
                    if (userRole != null)
                        await _UserManager.AddToRoleAsync(teacherUser, userRole.Name);


                    MailMessage mail = new MailMessage();
                    mail.To.Add(teacherUser.Email);
                    mail.From = new MailAddress(_settingsRepository.GetSchoolEmail());
                    mail.Subject = "Account in EBLC Portal";

                    mail.Body = "Greetings from the EBLC management. Your account has been created." + Environment.NewLine +
                   "Your credentials are:" + Environment.NewLine +
                   "Username: " + teacherUser.Email + Environment.NewLine +
                   "Password: " + account.Password;

                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(_settingsRepository.GetSchoolEmail(), _settingsRepository.GetPassword()); // Enter seders User name and password       
                    smtp.EnableSsl = true;
                    smtp.Send(mail);

                }
                else
                {
                    throw new Exception(result.Errors.First().Code);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB+". Specifically "+ex.Message);
            }
        }
        public async Task ApproveTeacherRegistration(int id)
        {
            try
            {
                var teacher = _TeacherRepository.GetTeacherRegistration(id);                
                var rtpCommons = _rtpCommons.GetRTPCommonsByUID(teacher.UserPortalID);
                var rtpusers = new RTPUsers();
                rtpusers.RTPId = rtpCommons.Id;
                var teacherUser = new IdentityUser
                {
                    UserName = teacher.Email, // Set the teacher's email address
                    Email = teacher.Email
                    // Add other properties as needed
                };                

                var result = await _UserManager.CreateAsync(teacherUser, teacher.Password);
                if(result.Succeeded)
                {
                    var userRole = _roleManager.FindByNameAsync("Teacher").Result;
                    rtpusers.AspUserId = teacherUser.Id;
                    if (userRole != null)
                        await _UserManager.AddToRoleAsync(teacherUser, userRole.Name);
                    _TeacherRepository.RemoveTeacherRegistration(id);
                    _rtpusers.AddRTPUsers(rtpusers);
                }
                else
                {
                    throw new Exception(result.Errors.ToString());
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
        public List<TeacherRegistrarionViewModel> GetAllTeacherRegistration()
        {
            try
            {
                return _TeacherRepository.GetAllTeacherApplicants();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public async Task RemoveTeacher(string aspid)
        {
            try
            {
                var user = await _UserManager.FindByIdAsync(aspid);
                var rtpid = _rtpusers.GetRTPUsers().FirstOrDefault(p => p.AspUserId == aspid).RTPId;
                var uid = _rtpCommons.getRTPCommons().FirstOrDefault(p => p.Id == rtpid).UID;
                var userportal = _UsersRepository.GetUserById(uid);
                await _UserManager.DeleteAsync(user);
                _UsersRepository.RemoveUser(userportal);               
            }
            catch(NullReferenceException ex)
            {
                throw new Exception("User not found");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB +". Ensure that the teacher is not an adviser on any class.");
            }
        }
    }
}
