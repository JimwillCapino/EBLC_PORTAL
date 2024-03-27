using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Models
{
    public class RTPUsers
    {
        [Key]
        public int Id { get; set; }
        public int RTPId { get; set; }
        public string AspUserId { get; set; }
    }
}
