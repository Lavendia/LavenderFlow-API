public interface IWorkspaceUserService
{
    Task<IEnumerable<WorkspaceUserResponse>?> GetUsersByWorkspaceAsync(int workspaceId);
    Task<IEnumerable<WorkspaceUserResponse>> GetWorkspacesByUserAsync(int userId);
    Task<WorkspaceUserResponse> CreateWorkspaceUserAsync(int workspaceId, CreateWorkspaceUsersRequest request);
    Task<WorkspaceUserResponse?> UpdateWorkspaceUserAsync(int workspaceUserId, UpdateWorkspaceUsersRequest request);
    Task<WorkspaceUserResponse?> GetWorkspaceUserAsync(int workspaceUserId);
    Task<bool> RemoveUserFromWorkspaceAsync(int workspaceUserId);
}
