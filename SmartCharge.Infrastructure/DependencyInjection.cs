using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartCharge.Core.Repositories;
using SmartCharge.Infrastructure.Mongo.Repositories;
using SmartCharge.Infrastructure.Mongo.Repositories.Persistence;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {        
            services.AddTransient<IChargeGroupRepository, ChargeGroupRepository>();
            services.AddTransient<IChargeStationRepository, ChargeStationRepository>();
            services.AddTransient<IMongoDbContext, MongoDbContext>();
            return services;
        }
    }
}