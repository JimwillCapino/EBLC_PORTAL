using AutoMapper;
using Basecode.Data;
using Basecode.Data.Interfaces;
using Basecode.Data.Models;
using Basecode.Data.ViewModels;
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
        private readonly IMapper _mapper;
        public SettingsService(ISettingsRepository settingsRepository,IMapper mapper)
        {
            _settingsRepository = settingsRepository;
            _mapper = mapper;
        }
        public void UpdateSettings(SettingsViewModel settings)
        {
            try
            {
                var settingsMaps = _mapper.Map<Settings>(settings);                
                if ((settings.SchoolLogoRecieve != null && settings.SchoolLogoRecieve.Length > 0)
                    && (settings.DepEdLogoRecieve != null && settings.DepEdLogoRecieve.Length > 0))
                {
                    using (MemoryStream stream1 = new MemoryStream())
                    {
                        settings.SchoolLogoRecieve.CopyTo(stream1);
                        settingsMaps.SchoolLogo = stream1.ToArray();
                    }
                    using (MemoryStream stream2 = new MemoryStream())
                    {
                        settings.DepEdLogoRecieve.CopyTo(stream2);
                        settingsMaps.DepEdLogo = stream2.ToArray();
                    }
                }
                else
                {                                        
                    settingsMaps.SchoolLogo = settings.SchoolLogo;
                    settingsMaps.DepEdLogo = settings.DepEdLogo;
                }

                //if (settingsMaps.WithHighestHonor == settingsMaps.WithHighHonor && settingsMaps.WithHighestHonor == settingsMaps.WithHonor && settingsMaps.WithHonor == settingsMaps.WithHighHonor)
                //{
                //    throw new Exception("Collision of thresholds occurred, please try again.");
                //}
                _settingsRepository.UpdateSettings(settingsMaps);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                throw ex;
            }
        }
        public void UpdateSchoolYear(DateTime StartofClass, DateTime EndofClass, int Id)
        {
            try
            {
                var settings = _settingsRepository.GetSettings();
                settings.StartofClass = StartofClass;
                settings.EndofClass = EndofClass;
                _settingsRepository.UpdateSettings(settings);
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
                _settingsRepository.UpdateSettings(settings);
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public SettingsViewModel GetSettings()
        {
            try
            {

                var settings = _settingsRepository.GetSettings();
                return _mapper.Map<SettingsViewModel>(settings);
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public Settings GetSettingsById(int id)
        {
            try
            {
                return _settingsRepository.GetSettingsById(id);
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
        public int? GetWithHighestHonor()
        {
            try
            {
                return _settingsRepository.GetWithHighestHonor();
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public int? GetWithHighHonor()
        {
            try
            {
                return _settingsRepository.GetWithHighHonor();
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public int? GetWithHonor()
        {
            try
            {
                return _settingsRepository.GetWithHonor();
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
    }
}
