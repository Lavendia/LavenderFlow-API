using System.Diagnostics.CodeAnalysis;

public class ChecklistItem : BaseEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public bool Finished { get; set; }
    public int ChecklistId { get; set; }
    public Checklist? Checklist { get; set; }

    public ChecklistItem() {}

    [SetsRequiredMembers]
    public ChecklistItem(string name, bool finished, int checklistId)
    {
        Name = name;
        Finished = finished;
        ChecklistId = checklistId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}