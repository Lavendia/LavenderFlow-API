public class ChecklistResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CardId { get; set; }
    public List<ChecklistItemResponse> Items { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ChecklistResponse(Checklist checklist)
    {
        Id = checklist.Id;
        Name = checklist.Name;
        CardId = checklist.CardId;
        Items = checklist.Items.Select(i => new ChecklistItemResponse(i)).ToList();
        CreatedAt = checklist.CreatedAt;
        UpdatedAt = checklist.UpdatedAt;
    }
}