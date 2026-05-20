public class CreateWorkspaceRequest
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public bool Public { get; set; }
}