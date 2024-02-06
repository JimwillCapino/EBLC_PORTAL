using Basecode.Data.Interfaces;
using Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Services.Services
{
    public class RTPService:IRTPService
    {
        IRTPRepository _rtpRepository;

        public RTPService(IRTPRepository rtpRepository)
        {
            _rtpRepository = rtpRepository;
        }
        public void addRTPCommons(RTPCommons commons)
        {
            _rtpRepository.addRTPCommons(commons);
        }
        public IEnumerable<RTPCommons> getRTPCommons()
        {
            return _rtpRepository.getRTPCommons();
        }

    }
}
