using Basecode.Data.Models;
using Basecode.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Services.Interfaces
{
    public interface ISubjectService
    {
        public int AddSubject(Subject subject);
        public List<Subject> GetSubjects();
        public void AddChildSubject(ChildSubject subject);
        public void AddHeadSubejct(HeadSubject subject);
        public HeadSubject GetHeadSubjectById(int id);
        public void SaveDbChanges();
        public void RemoveSubject(int id);
        public ChildSubjectContainer GetChildSubject(int headId);
        public Subject GetSubject(int id);
        public List<SubjectViewModel> GetSubjectsForDataTable();
        public void AddChildSubject(ChildSubjectContainer childSubject);
        public void UpdateChildSubject(ChildSubjectView subject);
        public void RemoveChildSubject(int id);
        public void UpdateSubject(SubjectViewModel subjectView);
    }

}
