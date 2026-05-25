public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<UserResponse>> GetUsersAsync()
    {
        var users = await _repository.GetAllAsync();

        return users.Select(u => new UserResponse(u));
    }

    public async Task<UserResponse?> GetUserAsync(int id)
    {
        var user = await _repository.GetByIdAsync(id);

        return user == null
            ? null
            : new UserResponse(user);
    }

    public async Task<UserResponse?> GetUserByEmailAsync(string email)
    {
        var user = await _repository.GetUserByEmailAsync(email);
        if (user == null) return null;

        return user == null
            ? null
            : new UserResponse(user);
    }

    public async Task<UserResponse?> UpdateUserAsync(
        int id,
        UpdateUserRequest request)
    {
        var user = await _repository.GetByIdAsync(id);

        if (user == null)
            return null;

        if (request.Name != null)
            user.Name = request.Name;

        if (request.Email != null)
            user.Email = request.Email;

        if (request.Password != null)
            user.PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(request.Password);

        _repository.Update(user);
        await _repository.SaveAsync();

        return new UserResponse(user);
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _repository.GetByIdAsync(id);

        if (user == null)
            return false;

        _repository.Delete(user);
        await _repository.SaveAsync();

        return true;
    }
}