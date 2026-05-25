public interface IWorkspaceUserService
{
    Task<IEnumerable<WorkspaceUserResponse>?> GetUsersByWorkspaceAsync(int workspaceId);
    Task<IEnumerable<WorkspaceUserResponse>> GetWorkspacesByUserAsync(int userId);
    Task<WorkspaceUserResponse> CreateWorkspaceUserAsync(CreateWorkspaceUserRequest request);
    Task<WorkspaceUserResponse?> UpdateWorkspaceUserAsync(int workspaceUserId, UpdateWorkspaceUserRequest request);
    Task<WorkspaceUserResponse?> GetWorkspaceUserAsync(int workspaceUserId);
    Task<bool> RemoveUserFromWorkspaceAsync(int workspaceUserId);
}
