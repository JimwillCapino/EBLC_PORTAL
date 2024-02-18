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
