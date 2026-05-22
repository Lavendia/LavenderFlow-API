public interface IWorkspaceUserService
{
    Task<IEnumerable<WorkspaceUserResponse>?> GetWorkspaceUsersAsync(int workspaceId);
    Task<WorkspaceUserResponse> CreateWorkspaceUserAsync(int workspaceId, CreateWorkspaceUsersRequest request);
    Task<WorkspaceUserResponse?> GetWorkspaceUserAsync(int id);
}
