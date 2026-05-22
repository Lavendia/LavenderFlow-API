public class WorkspaceResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool Public { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public WorkspaceResponse(Workspace workspace)
    {
        Id = workspace.Id;
        Name = workspace.Name;
        Description = workspace.Description;
        Public = workspace.Public;
        CreatedAt = workspace.CreatedAt;
        UpdatedAt = workspace.UpdatedAt;
    }
}