using Basecode.Data.Interfaces;
using Basecode.Data.Models;
using Basecode.Data.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Repositories
{
    public class ParentRepository:BaseRepository,IParentRepository
    {
        private readonly BasecodeContext _context;
        public ParentRepository(IUnitOfWork unitOfWork, BasecodeContext context):base(unitOfWork) 
        { 
            _context = context;
        }
        public int AddParent(Parent parent)
        {
            try
            {
                var a =_context.Parent.Add(parent);
                _context.SaveChanges();
                return a.Entity.Id;
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }
        public IEnumerable<Parent> GetAllParents()  
        {
            try
            {
                return this.GetDbSet<Parent>();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }
        public void RemoveParent(Parent parent)
        {
            try
            {
                _context.Parent.Remove(parent);
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.InnerException.Message);
            }
        }
        public Parent GetParentById(int id)
        {
            try
            {
                return _context.Parent.Find(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.InnerException.Message);
            }
        }
        public async Task<ParentDetails> GetParentDetailById(int studentId)
        {
            try
            {
                var student = await _context.Student.FindAsync(studentId);
                var parent = await _context.Parent.FindAsync(student.ParentId);
                var parentUserPortal = await _context.UsersPortal.FindAsync(parent.UID);
                var rtpCommons = await _context.RTPCommons.FirstOrDefaultAsync(p => p.UID == parent.UID);
                var parentDetails = new ParentDetails()
                {
                    UID = parent.UID,
                    ParentId = parent.Id,
                    RTPCommonsId = rtpCommons.Id,
                    ParentFirstName = parentUserPortal.FirstName,
                    ParentMiddleName = parentUserPortal.MiddleName,
                    ParentLastName = parentUserPortal.LastName,
                    ParentBirthday = parentUserPortal.Birthday,
                    Parentsex = parentUserPortal.sex,
                    PhoneNumber = rtpCommons.PhoneNumber,
                    Address = rtpCommons.Address,
                    Email = parent.Email
                };
                return parentDetails;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }
    }
}
