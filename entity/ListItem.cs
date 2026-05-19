using System.Diagnostics.CodeAnalysis;

public class ListItem : BaseEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Order { get; set; }

    public int BoardId { get; set; }
    public Board? Board { get; set; }

    public List<Card>? Cards { get; set; }
    public ListItem() { }

    [SetsRequiredMembers]
    public ListItem(string name, int order, int boardId)
    {
        Name = name;
        Order = order;
        BoardId = boardId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}