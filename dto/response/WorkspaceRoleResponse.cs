public class WorkspaceRoleResponse
{
    public int Id { get; set; }
    public string Name { get; set; }

    public WorkspaceRoleResponse(WorkspaceRole role)
    {
        Id = role.Id;
        Name = role.Name;
    }
}