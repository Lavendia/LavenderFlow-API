public interface IUserService
{
    Task<IEnumerable<UserResponse>> GetUsersAsync();
    Task<UserResponse?> GetUserAsync(int id);
    Task<UserResponse?> UpdateUserAsync(
        int id,
        UpdateUserRequest request);

    Task<bool> DeleteUserAsync(int id);
}