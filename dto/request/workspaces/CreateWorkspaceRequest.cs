public class CreateWorkspaceRequest
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool IsPublic { get; set; }
}