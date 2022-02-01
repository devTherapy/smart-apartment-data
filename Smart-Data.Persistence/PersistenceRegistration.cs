using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using Smart_Data.Application.Contracts;
using Smart_Data.Persistence.ElasticSearchRepository;

namespace Smart_Data.Persistence
{
    public static class PersistenceRegistration
    {
        public static void AddPersistence(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddScoped<IPropertySearchRepository, PropertySearchRepository>();
            service.AddScoped<IManagementSearchRepository, ManagementSearchRepository>();
            service.AddSingleton<IElasticClient>(s =>
            {
                var settings = new ConnectionSettings(configuration["ElasticCloudId"], new BasicAuthenticationCredentials(configuration["ElasticUsername"], configuration["ElasticCloudPassword"]))
                .EnableDebugMode();
               return new ElasticClient(settings);
            });
        }
    }
}
