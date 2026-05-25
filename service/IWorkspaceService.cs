public interface IWorkspaceService
{
    Task<IEnumerable<WorkspaceResponse>> GetWorkspacesForUserAsync(int userId);
    Task<WorkspaceResponse?> GetWorkspaceForUserAsync(int id, int userId);
    Task<WorkspaceResponse> CreateWorkspaceAsync(CreateWorkspaceRequest request, int userId);
    Task<bool> UpdateWorkspaceAsync(int id, UpdateWorkspaceRequest request, int userId);
    Task<IEnumerable<BoardResponse>?> GetBoardsByWorkspaceForUserAsync(int id, int userId);
    Task<bool> DeleteWorkspaceAsync(int id, int userId);
}
