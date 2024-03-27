using Basecode.Data.Interfaces;
using Basecode.Data.Models;
using Basecode.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Services.Services
{
    public class SettingsService:ISettingsService
    {
        private readonly ISettingsRepository _settingsRepository;
        public SettingsService(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }
        public void UpdateSchoolYear(DateTime StartofClass, DateTime EndofClass, int Id)
        {
            try
            {
                var settings = _settingsRepository.GetSettings();
                settings.StartofClass = StartofClass;
                settings.EndofClass = EndofClass;
                _settingsRepository.UpdateSchoolYear(settings);
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }

        }
        public void UpdateSettings(Settings settings)
        {
            try
            {
                _settingsRepository.UpdateSchoolYear(settings);
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public Settings GetSettings()
        {
            try
            {
                return _settingsRepository.GetSettings();
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public string GetSchoolYear()
        {
            try
            {
                return _settingsRepository.GetSchoolYear();
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
    }
}
