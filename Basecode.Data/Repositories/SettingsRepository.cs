using Basecode.Data.Interfaces;
using Basecode.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                return _context.Settings.AsNoTracking().FirstOrDefault();
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
                var settings = this.GetSettings();
                var schoolYear = settings.StartofClass.Value.Year.ToString() + "-" + settings.EndofClass.Value.Year.ToString();
                return schoolYear;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
