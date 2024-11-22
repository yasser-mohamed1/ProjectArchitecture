using Microsoft.Extensions.DependencyInjection;
using Project.Repository.Repository;

namespace Project.Repository
{
    public static class ModuleInfrastructureDependences
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection service)
        {
            service.AddTransient<UnitOfWork>();
            return service;
        }
    }
}
