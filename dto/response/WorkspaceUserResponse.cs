public class WorkspaceUserResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string UserEmail { get; set; }
    public int WorkspaceId { get; set; }
    public int RoleId { get; set; }
    public string? RoleName { get; set; }
    public DateTime CreatedAt { get; set; }

    public WorkspaceUserResponse(WorkspaceUser wu)
    {
        Id = wu.Id;
        UserId = wu.UserId;
        UserName = wu.User?.Name ?? "";
        UserEmail = wu.User?.Email ?? "";
        WorkspaceId = wu.WorkspaceId;
        RoleId = wu.RoleId;
        RoleName = wu.Role?.Name;
        CreatedAt = wu.CreatedAt;
    }
}