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
        public void AddParent(Parent parent);
        public IEnumerable<Parent> GetAllParents();
    }
}
