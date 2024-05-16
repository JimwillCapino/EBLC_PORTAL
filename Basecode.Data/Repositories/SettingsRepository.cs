using Basecode.Data.Interfaces;
using Basecode.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Basecode.Data.Repositories
{
    public class SettingsRepository : BaseRepository, ISettingsRepository
    {
        private readonly BasecodeContext _context;
        public SettingsRepository(IUnitOfWork unitOfWork, BasecodeContext context) : base(unitOfWork)
        {
            _context = context;
        }
        public void UpdateSettings(Settings settings)
        {
            try
            {
                _context.Settings.Update(settings);
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public Settings GetSettingsById(int id)
        {
            try
            {
                return _context.Settings.Find(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public Settings GetSettings()
        {
            try
            {
                var query = this.GetDbSet<Settings>().AsNoTracking().ToList();
                return query.FirstOrDefault();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public string GetSchoolYear()
        {
            try
            {
                string schoolYear;  
                var settings = _context.Settings.AsNoTracking().ToList();
                schoolYear = settings.FirstOrDefault()?.StartofClass.Value.Year.ToString() + "-" + settings.FirstOrDefault()?.EndofClass.Value.Year.ToString();
                return schoolYear;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public int? GetWithHighHonor()
        {
            try
            {
                return this.GetDbSet<Settings>().ToList().FirstOrDefault().WithHighHonor;
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
                return this.GetDbSet<Settings>().ToList().FirstOrDefault().WithHighestHonor;
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
                return this.GetDbSet<Settings>().ToList().FirstOrDefault().WithHonor;
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public string? GetSchoolEmail()
        {
            try
            {
                return this.GetDbSet<Settings>().ToList().FirstOrDefault().SchoolEmail;
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public string? GetPassword()
        {
            try
            {
                return this.GetDbSet<Settings>().ToList().FirstOrDefault().twoFPassword;
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
    }
}
