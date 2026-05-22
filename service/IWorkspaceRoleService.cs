public interface IWorkspaceRoleService
{
    Task<IEnumerable<WorkspaceRoleResponse>> GetWorkspaceRolesAsync();
    Task<WorkspaceRoleResponse> CreateWorkspaceRoleAsync(CreateWorkspaceRolesRequest request);
}
