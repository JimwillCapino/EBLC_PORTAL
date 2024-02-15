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
        public IEnumerable<TeacherViewModel> GetAllTeachersInitView()
        {
            try
            {
                var asproleTeacher = _userManager.GetUsersInRoleAsync("Teacher").Result.AsQueryable();
                var rtpusers = this.GetDbSet<RTPUsers>().AsQueryable();
                var rtpcommons = this.GetDbSet<RTPCommons>().AsQueryable();
                var users = this.GetDbSet<UsersPortal>().AsQueryable();

                var teacher = from asprole in asproleTeacher
                              join rtpu in rtpusers on asprole.Id equals rtpu.AspUserId
                              join rtpc in rtpcommons on rtpu.RTPId equals rtpc.Id
                              join u in users on rtpc.UID equals u.UID
                              select new TeacherViewModel
                              {
                                  Id =asprole.Id,
                                  FirstName = u.FirstName,
                                  MiddleName = u.MiddleName,
                                  LastName = u.LastName,
                                  Gender = u.sex
                              };
                return teacher;
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.InnerException.Message);
            }
        }
    }
}
