using MassTransit;
using ProductService.API.Implementations.Rabbit;

namespace ProductService.API.Extensions
{
    public static class RabbitExtensions
    {
        public static IServiceCollection AddRabbit(
            this IServiceCollection services)
        {
            services.AddMassTransit(b =>
            {
                b.AddConsumer<RegisteredUserConsumer>();

                b.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ReceiveEndpoint(e =>
                    {
                        e.ConfigureConsumer<RegisteredUserConsumer>(context);
                    });
                });
            });

            return services;
        }
    }
}
