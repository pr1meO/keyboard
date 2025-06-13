using Background.Contracts;
using MassTransit;
using ProductService.API.Interfaces.Services;

namespace ProductService.API.Implementations.Rabbit
{
    public class RegisteredUserConsumer : IConsumer<IRegisteredUser>
    {
        private readonly ICartService _cartService;
        private readonly ILogger<RegisteredUserConsumer> _logger;

        public RegisteredUserConsumer(
            ICartService cartService,
            ILogger<RegisteredUserConsumer> logger
        )
        {
            _cartService = cartService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IRegisteredUser> context)
        {
            try
            {
                var userId = context.Message.Id;

                _logger.LogInformation("The message was received.");

                await _cartService.CreateAsync(userId);
            }
            catch
            {
                _logger.LogError("An error occurred when receiving a message from the queue.");
                throw;
            }
        }
    }
}
