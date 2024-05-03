using Basecode.Data;
using Basecode.Data.Interfaces;
using Basecode.Data.Models;
using Basecode.Data.Repositories;
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
        public void RemoveRTP(RTPCommons rtpcommons)
        {
            try
            {
                _rtpRepository.RemoveRTPCommons(rtpcommons);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }

        }
        public RTPCommons GetRTPCommonsByUID(int id)
        {
            try
            {
                return _rtpRepository.GetRTPCommonsByUID(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }

        }
        public int AddRTPCommonsInt(RTPCommons commons)
        {
            try
            {
                return _rtpRepository.addRTPCommonsInt(commons);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Constants.Exception.DB);
            }
        }
    }
}
