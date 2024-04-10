using Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Interfaces
{
    public interface ISettingsRepository
    {
        public void UpdateSettings(Settings settings);
        public Settings GetSettings();
        public string GetSchoolYear();
        public Settings GetSettingsById(int id);
    }
}
