using AuthService.API.Interfaces.Rabbit;
using MassTransit;

namespace AuthService.API.Implementations.Rabbit
{
    public class RabbitSendService : IRabbitSendService
    {
        private readonly IPublishEndpoint _endpoint;
        private readonly ILogger<RabbitSendService> _logger;

        public RabbitSendService(
            IPublishEndpoint endpoint,
            ILogger<RabbitSendService> logger
        )
        {
            _endpoint = endpoint;
            _logger = logger;
        }

        public async Task SendAsync<T>(T message)
            where T : class
        {
            try
            {
                await _endpoint.Publish(message);
                _logger.LogInformation("Posting a message to queue.");
            }
            catch
            {
                _logger.LogError("Error sending a message to the queue.");
                throw;
            }
        }
    }
}
