using Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Interfaces
{
    public interface IRTPRepository
    {
        public void addRTPCommons(RTPCommons rtpcommons);
        public IEnumerable<RTPCommons> getRTPCommons();
        public void RemoveRTPCommons(RTPCommons rtpcommons);
        public RTPCommons GetRTPCommonsByUID(int id);
    }
}
