using System.Diagnostics.CodeAnalysis;

public class Board : BaseEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }

    public int WorkspaceId { get; set; }
    public Workspace? Workspace { get; set; }

    public List<ListItem> ListItems { get; set; } = new();

    public Board() {}

    [SetsRequiredMembers]
    public Board(string name, string? description, int workspaceId)
    {
        Name = name;
        Description = description;
        WorkspaceId = workspaceId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}