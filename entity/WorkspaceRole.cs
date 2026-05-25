using System.Diagnostics.CodeAnalysis;
public class WorkspaceRole : BaseEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public WorkspaceRole() {}

    [SetsRequiredMembers]
    public WorkspaceRole(string name)
    {
        Name = name;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}

