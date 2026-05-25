using System.Diagnostics.CodeAnalysis;

public class Checklist : BaseEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int CardId { get; set; }
    public Card? Card { get; set; }
    public List<ChecklistItem> Items { get; set; } = new();

    public Checklist() {}

    [SetsRequiredMembers]
    public Checklist(string name, int cardId)
    {
        Name = name;
        CardId = cardId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}