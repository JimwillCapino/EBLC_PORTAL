using AutoMapper;
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
    public class UsersRepository : BaseRepository, IUsersRepository
    {
        private readonly BasecodeContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        public UsersRepository(IUnitOfWork unitOfWork, BasecodeContext context, IMapper mapper, UserManager<IdentityUser> userManager) : base(unitOfWork)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }
        public int AddUser(UsersPortal user)
        {
            try
            {
                var a =_context.UsersPortal.Add(user);
                _context.SaveChanges();
                return a.Entity.UID;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1;
            }
        }
        public int GetMostRecentUsersId()
        {
            try
            {
                var recentuserid = _context.UsersPortal.OrderByDescending(j => j.UID).FirstOrDefault();
                return recentuserid.UID;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1;
            }
        }
        public void RemoveUser(UsersPortal user)
        {
            try
            {
                _context.UsersPortal.Remove(user);
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.InnerException.Message);
            }

        }
        public UsersPortal GetUserById(int id)
        {
            try
            {
                return _context.UsersPortal.Find(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.InnerException.Message);
            }
        }
        public async Task<bool> IsNewUser(string AspUserId)
        {
            try
            {
                var aspuser = await _userManager.FindByIdAsync(AspUserId);
                var RTPUser = _context.RTPUsers.FirstOrDefault(p => p.AspUserId == AspUserId);
                return RTPUser == null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public async Task<ProfileViewModel> GetUserPortal(string AspUserId)
        {
            try
            {
                var aspuser = await _userManager.FindByIdAsync(AspUserId);
                var RTPUser = _context.RTPUsers.FirstOrDefault(p => p.AspUserId == AspUserId);
                if (RTPUser == null)
                    throw new NullReferenceException();
                var RTPCommons = _context.RTPCommons.Find(RTPUser.RTPId);
                var user = _context.UsersPortal.Find(RTPCommons.UID);
                var completeDetails = new ProfileViewModel()
                {
                    UID = user.UID,
                    ProfilePic = user.ProfilePic,
                    RTPCommonsId = RTPCommons.Id,
                    RTPUsersId = RTPUser.Id,
                    AspUserId = AspUserId,
                    FirstName = user.FirstName, 
                    LastName = user.LastName,
                    MiddleName = user.MiddleName,
                    Birthday = user.Birthday,
                    sex =user.sex,
                    Address = RTPCommons.Address,
                    PhoneNumber =RTPCommons.PhoneNumber,
                    Email = aspuser.Email,                  
                };
                return completeDetails;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public async Task UpdateUserPortal(UsersPortal user)
        {
            try
            {
                _context.UsersPortal.Update(user);
                await _context.SaveChangesAsync();              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
