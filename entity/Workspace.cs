using System.Diagnostics.CodeAnalysis;
public class Workspace : BaseEntity
{
    public int Id {get; set;}
    public required string Name {get; set;}
    public string? Description {get; set;}
    public bool Public {get; set;}
    public List<Board> Boards {get; set;} = new();

    public Workspace() {}

    [SetsRequiredMembers]
    public Workspace(string name, string? description, bool isPublic = false)
    {
        Name = name;
        Description = description;
        Public = isPublic;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}