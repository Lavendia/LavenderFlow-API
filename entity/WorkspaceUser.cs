using System.Diagnostics.CodeAnalysis;

public class WorkspaceUser
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public int WorkspaceId { get; set; }
    public Workspace? Workspace { get; set; }
    public int RoleId { get; set; }
    public WorkspaceRole? Role { get; set; }

    public WorkspaceUser() {}

    [SetsRequiredMembers]
    public WorkspaceUser(int userId, int workspaceId, int roleId)
    {
        UserId = userId;
        WorkspaceId = workspaceId;
        RoleId = roleId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}