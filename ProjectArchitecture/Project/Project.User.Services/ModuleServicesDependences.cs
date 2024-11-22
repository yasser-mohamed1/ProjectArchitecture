using Microsoft.Extensions.DependencyInjection;
using Project.Services.Helpers;
using Project.Services.Implementation;
using Project.Services.Interfaces;
using Project.Services.Mapper;

namespace Project.User.Services
{
    public static class ModuleServicesDependences
    {
        public static IServiceCollection AddReposetoriesServices(this IServiceCollection service)
        {
            service.AddAutoMapper(typeof(MappingProfile));
            service.AddTransient<IAuthenticationService, AuthenticationService>();
            service.AddTransient<IHelpureService, HelpureService>();
            service.AddTransient<ICustomerService, CustomerService>();
            return service;
        }
    }
}
