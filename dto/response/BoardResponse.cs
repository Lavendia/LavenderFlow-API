public class BoardResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int WorkspaceId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public BoardResponse(Board board)
    {
        Id = board.Id;
        Name = board.Name;
        Description = board.Description;
        WorkspaceId = board.WorkspaceId;
        CreatedAt = board.CreatedAt;
        UpdatedAt = board.UpdatedAt;
    }
}