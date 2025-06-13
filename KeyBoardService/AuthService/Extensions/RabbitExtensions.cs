using AuthService.API.Implementations.Rabbit;
using AuthService.API.Interfaces.Rabbit;
using MassTransit;

namespace AuthService.API.Extensions
{
    public static class RabbitExtensions
    {
        public static IServiceCollection AddRabbit(
            this IServiceCollection services)
        {
            services.AddScoped<IRabbitSendService, RabbitSendService>();

            services.AddMassTransit(b =>
            {
                b.UsingRabbitMq();
            });

            return services;
        }
    }
}
