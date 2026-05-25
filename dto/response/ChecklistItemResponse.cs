public class ChecklistItemResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool Finished { get; set; }
    public int ChecklistId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ChecklistItemResponse(ChecklistItem item)
    {
        Id = item.Id;
        Name = item.Name;
        Finished = item.Finished;
        ChecklistId = item.ChecklistId;
        CreatedAt = item.CreatedAt;
        UpdatedAt = item.UpdatedAt;
    }
}