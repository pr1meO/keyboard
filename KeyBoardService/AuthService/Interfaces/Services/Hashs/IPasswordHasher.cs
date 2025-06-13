namespace AuthService.API.Interfaces.Services.Hashs
{
    public interface IPasswordHasher
    {
        string Generate(string password);
        bool Verify(string password, string passwordHash);
    }
}
