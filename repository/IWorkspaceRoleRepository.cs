public interface IWorkspaceRoleRepository
{
    Task<List<WorkspaceRole>> GetAllAsync();
    Task<WorkspaceRole?> GetByIdAsync(int id);
    void Add(WorkspaceRole role);
    Task SaveAsync();
}
