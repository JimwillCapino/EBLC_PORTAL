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
    public class ClassManagementService:IClassManagementService
    {
        IClassManagementRepository _repository;
        public ClassManagementService(IClassManagementRepository classManagementRepository) 
        { 
            _repository = classManagementRepository;
        }
        public void AddClass(Class classroom)
        {
            try
            {
                _repository.AddClass(classroom);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public void AddClass(ClassViewModel classViewModel)
        {
            try
            {
                Class classSection = new Class
                {
                    ClassName = classViewModel.ClassName,
                    Adviser = classViewModel.Adviser,
                    Grade = classViewModel.Grade,
                    ClassSize = classViewModel.ClassSize,
                };

                var classid = _repository.AddClass(classSection);

                //Adding Class subjects 
                foreach(var classSubject in classViewModel.ClassSubjects)
                {
                    ClassSubjects subs = new ClassSubjects
                    {
                        ClassId = classid,
                        Subject_Id = classSubject.Subject_Id,
                        Teacher_Id = classSubject.TeacherId
                    };
                    _repository.AddClassSubject(subs);
                }
                //Adding students to a class
                foreach(var classStudent in classViewModel.ClassStudents)
                {
                    var student = new ClassStudents
                    {
                        Class_Id = classid,
                        Student_Id = classStudent.Id
                    };
                    _repository.AddClassStudent(student);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
    }
}
