public class CreateCardRequest
{
    public required string Name { get; set; }
    public required int Order { get; set; }
    public string? Description { get; set; }
    public DateTime? Deadline { get; set; }
    public required int ListItemId { get; set; }
}