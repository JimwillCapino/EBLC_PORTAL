﻿using Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Services.Interfaces
{
    public interface IParentService
    {
        public int AddParent(Parent parent, string status);
        public IEnumerable<Parent> GetAllParents();
        public void RemoveParent(Parent parent);
        public Parent GetParentById(int id);
    }
}