public class WorkspaceUserResponse
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public int WorkspaceId { get; set; }
    public string? RoleName { get; set; }
    public DateTime CreatedAt { get; set; }

    public WorkspaceUserResponse(WorkspaceUser wu)
    {
        UserId = wu.UserId;
        UserName = wu.User?.Name ?? "";
        WorkspaceId = wu.WorkspaceId;
        RoleName = wu.Role?.Name;
        CreatedAt = wu.CreatedAt;
    }
}