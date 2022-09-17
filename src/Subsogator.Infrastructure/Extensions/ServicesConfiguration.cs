using Data.DataAccess.Repositories;
using Data.DataAccess.Repositories.Implementation;
using Data.DataAccess.Repositories.Interfaces;
using Data.DataModels.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Subsogator.Business.Services.Actors;
using Subsogator.Business.Transactions.Implementation;
using Subsogator.Business.Transactions.Interfaces;

namespace Subsogator.Infrastructure.Extensions
{
    public static class ServicesConfiguration
    {
        public static void RegisterServiceLayer(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();

            serviceCollection.AddTransient<IActorService, ActorService>();
        }
    }
}
