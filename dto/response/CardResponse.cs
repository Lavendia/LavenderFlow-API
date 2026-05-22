public class CardResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }
    public string? Description { get; set; }
    public bool Archived { get; set; }
    public DateTime? Deadline { get; set; }
    public int ListItemId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public CardResponse(Card card)
    {
        Id = card.Id;
        Name = card.Name;
        Order = card.Order;
        Description = card.Description;
        Archived = card.Archived;
        Deadline = card.Deadline;
        ListItemId = card.ListItemId;
        CreatedAt = card.CreatedAt;
        UpdatedAt = card.UpdatedAt;
    }
}