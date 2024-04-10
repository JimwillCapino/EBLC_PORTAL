using Basecode.Data.Interfaces;
using Basecode.Data.Models;
using Basecode.Data.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Repositories
{
    public class TeacherRepository : BaseRepository,ITeacherRepository
    {
        BasecodeContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public TeacherRepository(IUnitOfWork unitOfWork, 
            BasecodeContext context, 
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager
            ) : base(unitOfWork)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public void AddTeacher(Teacher teacher)
        {
            try
            {
                _context.Teacher.Add(teacher);
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.InnerException.Message);
            }
        }
        public void AddTeacherRegistration(TeacherRegistration teacher)
        {
            try
            {
                _context.TeacherRegistration.Add(teacher);
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                Console.Write(ex);
                throw;
            }
        }
        public void RemoveTeacherRegistration(int id)
        {
            try
            {
                var teacher = _context.TeacherRegistration.Find(id);
                _context.TeacherRegistration.Remove(teacher);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                throw;
            }
        }
        public TeacherRegistration GetTeacherRegistration(int id)
        {
            try
            {
                return _context.TeacherRegistration.Find(id);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                throw;
            }
        }
        public List<TeacherRegistrarionViewModel> GetAllTeacherApplicants()
        {
            try
            {
                var usersPortal = this.GetDbSet<UsersPortal>();
                var teacherRegistration = this.GetDbSet<TeacherRegistration>();

                var teachers = from u in usersPortal join
                               t in teacherRegistration
                               on u.UID equals t.UserPortalID
                               select new TeacherRegistrarionViewModel
                               {
                                   Id = t.Id,
                                   FirstName = u.FirstName,
                                   MiddleName = u.MiddleName,
                                   LastName = u.LastName,
                                   Email = t.Email,
                                   Password = t.Password,
                                   ProfilePic = u.ProfilePic,
                                   sex = u.sex
                               };
                return teachers.ToList();
                               
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                throw;
            }
        }
        public async Task<List<TeacherViewModel>> GetAllTeachersInitViewAsync()
        {
            try
            {
                var aspuser = await _userManager.GetUsersInRoleAsync("Teacher");                
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
                                  FirstName = users.FirstName,
                                  LastName = users.LastName,
                                  MiddleName = users.MiddleName,
                                  Gender = users.sex,
                                  Id = au.Id,
                                  Email = au.Email
                              };
                return teacher.ToList();                             
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.InnerException.Message);
            }
        }       
    }
}
