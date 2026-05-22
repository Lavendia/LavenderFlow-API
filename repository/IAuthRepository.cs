public interface IAuthRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task CreateUserAsync(User user);
}