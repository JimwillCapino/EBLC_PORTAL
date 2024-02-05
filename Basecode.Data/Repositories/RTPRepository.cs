using Basecode.Data.Interfaces;
using Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Repositories
{
    public class RTPRepository:BaseRepository,IRTPRepository
    {
        BasecodeContext _context;

        public  RTPRepository(IUnitOfWork unitOfWork,BasecodeContext context) : base(unitOfWork)
        {
            _context = context;
        }
        public void addRTPCommons(RTPCommons rtpcommons)
        {
            try
            {
                _context.RTPCommons.Add(rtpcommons);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
        }
        public IEnumerable<RTPCommons> getRTPCommons()
        {
            try
            {
                return this.GetDbSet<RTPCommons>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception("Error");
            }
        }
    }
}
