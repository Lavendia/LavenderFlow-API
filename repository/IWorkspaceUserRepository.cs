public interface IWorkspaceUserRepository
{
    Task<List<WorkspaceUser>> GetByWorkspaceIdAsync(int workspaceId);
    Task<List<WorkspaceUser>> GetByUserIdAsync(int userId);
    Task<WorkspaceUser?> GetByIdAsync(int workspaceUserId);
    Task<bool> ExistsAsync(int workspaceUserId);
    void Add(WorkspaceUser workspaceUser);
    void Delete(WorkspaceUser workspaceUser);
    Task SaveAsync();
}
