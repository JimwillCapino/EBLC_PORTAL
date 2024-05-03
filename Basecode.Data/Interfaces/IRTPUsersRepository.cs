using Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Interfaces
{
    public interface IRTPUsersRepository
    {
        public void AddRTPUsers(RTPUsers rTPUsers);
        public RTPUsers GetRTPuserByRTPId(int rtpId);
        public IEnumerable<RTPUsers> GetRTPUsers();
    }
}
