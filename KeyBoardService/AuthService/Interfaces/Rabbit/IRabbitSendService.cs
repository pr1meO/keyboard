namespace AuthService.API.Interfaces.Rabbit
{
    public interface IRabbitSendService
    {
        Task SendAsync<T>(T message) where T : class;
    }
}
