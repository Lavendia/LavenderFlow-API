using System.Diagnostics.CodeAnalysis;

public class WorkspacesUsers
{
    public int Id { get; set; }
    public required int UserId { get; set; }
    public required User User { get; set; }

    public required int WorkspaceId { get; set; }
    public required Workspace Workspace { get; set; }

    public required int RoleId { get; set; }
    public required WorkspacesRoles Role { get; set; }

    [SetsRequiredMembers]
    public WorkspacesUsers(int userId, int workspaceId, int roleId)
    {
        UserId = userId;
        WorkspaceId = workspaceId;
        RoleId = roleId;
    }
}