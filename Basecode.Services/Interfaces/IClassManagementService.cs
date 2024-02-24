﻿using Basecode.Data.Models;
using Basecode.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Services.Interfaces
{
    public interface IClassManagementService
    {
        public void AddClass(Class classroom);
        public void AddClass(ClassViewModel classViewModel);
        public void AddClassSubjects(ClassSubjects classSubjects);
        public void AddClassStudent(ClassStudents student);
        public ClassViewModel InitilaizeClassViewModel(int grade);
        public Task<ClassDetailsViewModel> GetAllClass();
        public Task<ClassViewModel> GetClassViewModelById(int id);
        public void RemoveClassStudent(int id);
        public void RemoveClassSubject(int id);
        public void RemoveClass(int id);
    }
}