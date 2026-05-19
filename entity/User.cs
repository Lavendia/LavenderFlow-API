using System.Diagnostics.CodeAnalysis;

public class User : BaseEntity
{
    public int Id {get; set;}
    public required string Name {get; set;}
    public required string Email {get; set;}
    public required string PasswordHash {get; set;}

    public User() {}

    [SetsRequiredMembers]
    public User(string name, string email, string password)
    {
        Name = name;
        Email = email;
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}