using Basecode.Data.Interfaces;
using Basecode.Data.ViewModels;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basecode.Data;
using Basecode.Data.Models;
using Basecode.Services.Interfaces;
namespace Basecode.Services.Services
{
    public class StudentManagementService:IStudentManagementService
    {
        private readonly IStudentManagementRepository _studentManagementRepository;
        public StudentManagementService(IStudentManagementRepository studentManagementRepository) 
        {
            _studentManagementRepository = studentManagementRepository;
        }
        public List<GradesViewModel> GetStudentGradeBySubject(int student_Id, int subject_Id)
        {
            try
            {
                return _studentManagementRepository.GetStudentGradeBySubject(student_Id, subject_Id);
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public void SubmitGrade(Grades grades)
        {
            try
            {
                _studentManagementRepository.SubmitGrade(grades);
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
    }
}
