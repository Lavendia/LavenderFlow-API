using System.Diagnostics.CodeAnalysis;

public class BoardRole : BaseEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public BoardRole() {}

    [SetsRequiredMembers]
    public BoardRole(string name)
    {
        Name = name;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}