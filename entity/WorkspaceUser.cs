using System.Diagnostics.CodeAnalysis;

public class WorkspaceUser
{
    public int Id { get; set; }
    public required int UserId { get; set; }
    public required User User { get; set; }

    public required int WorkspaceId { get; set; }
    public required Workspace Workspace { get; set; }

    public required int RoleId { get; set; }
    public required WorkspaceRole Role { get; set; }

    [SetsRequiredMembers]
    public WorkspaceUser(int userId, int workspaceId, int roleId)
    {
        UserId = userId;
        WorkspaceId = workspaceId;
        RoleId = roleId;
    }
}