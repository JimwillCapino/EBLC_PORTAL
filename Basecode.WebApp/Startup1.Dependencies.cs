using Basecode.WebApp.Authentication;
using Basecode.Data;
using Basecode.Data.Interfaces;
using Basecode.Data.Repositories;
using Basecode.Services.Interfaces;
using Basecode.Services.Services;

namespace Basecode.WebApp
{
    public partial class Startup1
    {
        private void ConfigureDependencies(IServiceCollection services)
        {            
            // Common
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ClaimsProvider, ClaimsProvider>();

            // Services 
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<INewEnrolleeService, NewEnrolleeService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IParentService, ParentService>();
            services.AddScoped<IRTPService, RTPService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<IRTPUsersService, RTPUsesrsService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<IClassManagementService, ClassManagementService>();
            services.AddScoped<IStudentManagementService, StudentManagementService>();
            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<INewEnrolleeRepository, NewEnrolleeRepository>();
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IParentRepository, ParentRepository>();
            services.AddScoped<IRTPRepository, RTPRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<ITeacherRepository, TeacherRepository>();
            services.AddScoped<IRTPUsersRepository, RTPUsersRepository>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            services.AddScoped<IClassManagementRepository, ClassManagementRepository>();
            services.AddScoped<IStudentManagementRepository, StudentManagementRepository>();
        }
    }
}