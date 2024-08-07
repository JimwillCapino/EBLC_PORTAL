﻿using Basecode.Data.Interfaces;
using Basecode.Data.Models;
using Basecode.Services .Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Basecode.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IdentityUser FindByUsername(string username)
        {
            return _userRepository.FindByUsername(username);
        }

        public IdentityUser FindById(string id)
        {
            return _userRepository.FindById(id);
        }

        public IdentityUser FindUser(string userName)
        {
            return _userRepository.FindUser(userName);
        }

        public IEnumerable<IdentityUser> FindAll()
        {
            return _userRepository.FindAll();
        }

        public bool Create(IdentityUser user)
        {
            return _userRepository.Create(user);
        }

        public bool Update(IdentityUser user)
        {
            return _userRepository.Update(user);
        }

        public void Delete(IdentityUser user)
        {
            _userRepository.Delete(user);
        }

        public Task<IdentityUser> FindUserAsync(string userName, string password)
        {
            return _userRepository.FindUserAsync(userName, password);
        }

        public async Task<IdentityResult> RegisterUser(string username, string password, string firstName, string lastName, string email, string role)
        {
            return await _userRepository.RegisterUser(username, password, firstName, lastName, email, role);
        }
        public async Task<IdentityResult> CreateRole(string roleName)
        {
            return await _userRepository.CreateRole(roleName);
        }

        public async Task<IdentityUser> FindUser(string username, string password)
        {
            return await _userRepository.FindUser(username, password);
        }
    }
}
