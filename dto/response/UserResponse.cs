public class UserResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public UserResponse(User user)
    {
        Id = user.Id;
        Name = user.Name;
        Email = user.Email;
        CreatedAt = user.CreatedAt;
        UpdatedAt = user.UpdatedAt;
    }
}