using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Models
{
    public class NewEnrollee
    {
       public int Enrollee_Id { get; set; }
       public int UID { get; set; }
       public byte[]? CGM { get; set; }
       public byte[]? BirthCertificate { get; set; }
       public byte[]? TOR { get;set; }


    }
}
