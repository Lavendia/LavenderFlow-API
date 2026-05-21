using System.Diagnostics.CodeAnalysis;

public class Card : BaseEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Order { get; set; }
    public string? Description { get; set; }
    public bool Archived { get; set; }
    public DateTime? Deadline { get; set; }

    public int ListItemId { get; set; }
    public ListItem? ListItem { get; set; }

    public List<Checklist> Checklists { get; set; } = new();

    public Card() {}

    [SetsRequiredMembers]
    public Card(string name, int order, string? description, bool archived, DateTime? deadline, int listItemId)
    {
        Name = name;
        Order = order;
        Description = description;
        Archived = archived;
        Deadline = deadline;
        ListItemId = listItemId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}