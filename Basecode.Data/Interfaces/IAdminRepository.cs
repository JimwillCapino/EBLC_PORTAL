﻿using Basecode.Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Interfaces
{
    public interface IAdminRepository
    {
        public Task<IdentityResult> CreateRole(string roleName);
        public void AddUser(UsersPortal usersPortal);
    }
}
