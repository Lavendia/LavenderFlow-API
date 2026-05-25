public interface IWorkspaceService
{
    Task<IEnumerable<WorkspaceResponse>> GetWorkspacesAsync();
    
    Task<WorkspaceResponse?> GetWorkspaceAsync(int id);
    Task<WorkspaceResponse> CreateWorkspaceAsync(CreateWorkspaceRequest request);
    Task<bool> UpdateWorkspaceAsync(int id, UpdateWorkspaceRequest request);
    Task<IEnumerable<BoardResponse>?> GetBoardsByWorkspaceAsync(int id);
    Task<bool> DeleteWorkspaceAsync(int id);
}
