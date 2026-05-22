public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task SaveAsync();
    void Update(User user);
    void Delete(User user);
}