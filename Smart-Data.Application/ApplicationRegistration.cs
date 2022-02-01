using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Smart_Data.Application
{
    public static class ApplicationRegistration
    {
        public static void RegisterApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}
