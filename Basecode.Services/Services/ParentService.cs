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
    public class ParentService: IParentService
    {
        private readonly IParentRepository _parentRepository;

        public ParentService(IParentRepository parentRepository)
        {
            _parentRepository = parentRepository;
        }
        public int AddParent(Parent parent)
        {            
            return _parentRepository.AddParent(parent);
        }
        public IEnumerable<Parent> GetAllParents()
        {
            return _parentRepository.GetAllParents();
        }
        public void RemoveParent(Parent parent)
        {
            try
            {
                _parentRepository.RemoveParent(parent);
            }           
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }

        }
        public Parent GetParentById(int id)
        {
            try
            {
                return _parentRepository.GetParentById(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
        public async Task<ParentDetails> GetParentDetailsById(int studentId)
        {
            try
            {
                return await _parentRepository.GetParentDetailById(studentId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
    }
}
