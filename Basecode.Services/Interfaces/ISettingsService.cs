using Basecode.Data.Models;
using Basecode.Data.ViewModels;
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
        public void UpdateSettings(Settings settings);
        public void UpdateSettings(SettingsViewModel settings);
        //public Settings GetSettings();
        public string GetSchoolYear();
        public SettingsViewModel GetSettings();
        public Settings GetSettingsById(int id);
        public int? GetWithHighestHonor();
        public int? GetWithHighHonor();
        public int? GetWithHonor();
    }
}
