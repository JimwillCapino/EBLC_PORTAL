using Basecode.Data;
using Basecode.Data.Interfaces;
using Basecode.Data.Models;
using Basecode.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Services.Services
{
    public class RTPUsesrsService:IRTPUsersService
    {
        IRTPUsersRepository _repository;

        public RTPUsesrsService( IRTPUsersRepository repository )
        {
            _repository = repository;
        }
        public void AddRTPUsers(RTPUsers rTPUsers)
        {
            try
            {
                _repository.AddRTPUsers(rTPUsers);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
    }
}
