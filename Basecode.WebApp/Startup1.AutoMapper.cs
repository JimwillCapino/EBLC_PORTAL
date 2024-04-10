using AutoMapper;
using Basecode.Data.Models;
using Basecode.Data.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Basecode.WebApp
{
    public partial class Startup1
    {
        private void ConfigureMapper(IServiceCollection services)
        {
            var Config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RegisterStudent, UsersPortal>();
                cfg.CreateMap<RegisterStudent, NewEnrollee>();
                cfg.CreateMap<RegisterStudent, Parent>();
                cfg.CreateMap<RegisterStudent, RTPCommons>();
                cfg.CreateMap<SettingsViewModel, Settings>();
                cfg.CreateMap<Settings, SettingsViewModel>();
                cfg.CreateMap<TeacherRegistration, TeacherRegistrarionViewModel>();
                cfg.CreateMap<UsersPortal, TeacherRegistrarionViewModel>();
                cfg.CreateMap<TeacherRegistrarionViewModel, TeacherRegistration>();
                cfg.CreateMap<TeacherRegistrarionViewModel, UsersPortal>();
            });

            services.AddSingleton(Config.CreateMapper());
        }
    }
}