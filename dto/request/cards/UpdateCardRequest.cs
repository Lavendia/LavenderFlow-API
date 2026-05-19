public class UpdateCardRequest
{
    public string? Name { get; set; }
    public int? Order { get; set; }
    public string? Description { get; set; }
    public bool? Archived { get; set; }
    public DateTime? Deadline { get; set; }
    public int? ListItemId { get; set; }
}