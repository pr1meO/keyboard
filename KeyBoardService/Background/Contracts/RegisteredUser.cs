namespace Background.Contracts
{
    public interface IRegisteredUser
    {
        Guid Id { get; set; }
    }

    public class RegisteredUser : IRegisteredUser
    {
        public Guid Id { get; set; }
    }
}
