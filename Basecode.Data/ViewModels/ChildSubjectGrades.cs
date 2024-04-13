﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class ChildSubjectGrades
    {
        public int HeadId { get; set; }
        public int StudentId { get; set; }
        public List<ChildSubjectView> ChildSubjects { get; set; }
        public List<GradesDetail> GradesContainer { get; set;}
    }
}