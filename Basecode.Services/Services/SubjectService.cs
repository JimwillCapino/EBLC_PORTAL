using Basecode.Data;
using Basecode.Data.Interfaces;
using Basecode.Data.Models;
using Basecode.Data.ViewModels;
using Basecode.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Services.Services
{
    public class SubjectService:ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;
        public SubjectService(ISubjectRepository subjectRepository)
        { 
            _subjectRepository = subjectRepository;
        }
        public Subject GetSubject(int id)
        {
            try
            {
                return _subjectRepository.GetSubjectById(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public int AddSubject(Subject subject)
        {
            try
            {
                return _subjectRepository.AddSubject(subject);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public List<Subject> GetSubjects()
        {
            try
            {
                return _subjectRepository.GetSubjects();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public void AddChildSubject(ChildSubject subject)
        {
            try
            {
                _subjectRepository.AddChildSubject(subject);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public void AddHeadSubejct(HeadSubject subject)
        {
            try
            {
                _subjectRepository.AddHeadSubejct(subject);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public HeadSubject GetHeadSubjectById(int id)
        {
            try
            {
                return _subjectRepository.GetHeadSubjectById(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public void SaveDbChanges()
        {
            try
            {
                _subjectRepository.SaveDbChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }        
        public void RemoveSubject(int id)
        {
            try
            {
                var subject = _subjectRepository.GetSubjectById(id);
                _subjectRepository.RemoveSubject(subject);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public ChildSubjectContainer GetChildSubject(int headId)
        {
            try
            {
                var container = new ChildSubjectContainer();
                container.ChildSubjects = _subjectRepository.GetChildSubject(headId);
                container.HeadId = headId;
                return container;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public List<SubjectViewModel> GetSubjectsForDataTable()
        {
            try
            {
                return _subjectRepository.GetsSubjectsForDataTables();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public void AddChildSubject(ChildSubjectView childSubject)
        {
            try
            {
                Subject subject = new Subject();
                ChildSubject childSub = new ChildSubject();
                var HeadSub = _subjectRepository.GetSubjectById(childSubject.Id);
                subject.Subject_Name = childSubject.Name;
                subject.Grade = HeadSub.Grade;
                subject.HasChild = false;

                int id = _subjectRepository.AddSubject(subject);

                childSub.HeadSubjectId = HeadSub.Subject_Id;
                childSub.Subject_Id = id;
                _subjectRepository.AddChildSubject(childSub);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
    }
}
