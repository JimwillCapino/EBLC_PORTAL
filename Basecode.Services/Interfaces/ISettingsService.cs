using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Services.Interfaces
{
    public interface ISettingsService
    {
        public void UpdateSchoolYear(DateTime StartofClass, DateTime EndofClass, int Id);
    }
}
