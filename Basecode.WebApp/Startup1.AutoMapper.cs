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
                cfg.CreateMap<TeacherRegistrarionViewModel, RTPCommons>();
                cfg.CreateMap<RTPCommons,TeacherRegistrarionViewModel>();
                cfg.CreateMap<TeacherRegistrarionViewModel, RTPUsers>();
                cfg.CreateMap<RTPUsers, TeacherRegistrarionViewModel>();
                cfg.CreateMap<ProfileViewModel,RTPCommons>();
                cfg.CreateMap<ProfileViewModel,UsersPortal>();
            });

            services.AddSingleton(Config.CreateMapper());
        }
    }
}