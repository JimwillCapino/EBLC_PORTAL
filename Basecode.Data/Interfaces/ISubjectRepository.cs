using Basecode.Data.Models;
using Basecode.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Interfaces
{
    public interface ISubjectRepository
    {
        public int AddSubject(Subject subject);
        public List<Subject> GetSubjects();
        public void AddChildSubject(ChildSubject subject);
        public void AddHeadSubejct(HeadSubject subject);
        public HeadSubject GetHeadSubjectById(int id);
        public void SaveDbChanges();
        public void RemoveSubject(Subject subject); 
        public Subject GetSubjectById(int id);
        public List<ChildSubjectView> GetChildSubject(int headId);
        public List<Subject> GetAllSubjects(int studentId, string schoolYear);
        public List<HeadSubject> GetAllHeadSubject();
    }
}
