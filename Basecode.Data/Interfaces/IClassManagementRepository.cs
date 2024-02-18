using Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Interfaces
{
    public interface IClassManagementRepository
    {
        public int AddClass(Class classroom);
        public void AddClassSubject(ClassSubjects classSubjects);
        public void AddClassStudent(ClassStudents students);
    }
}
