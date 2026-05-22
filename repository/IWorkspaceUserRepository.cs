public interface IWorkspaceUserRepository
{
    Task<List<WorkspaceUser>> GetByWorkspaceIdAsync(int workspaceId);
    Task<WorkspaceUser?> GetByIdAsync(int id);
    void Add(WorkspaceUser workspaceUser);
    Task SaveAsync();
}
