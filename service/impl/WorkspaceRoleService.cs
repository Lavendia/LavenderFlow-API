public class WorkspaceRoleService : IWorkspaceRoleService
{
    private readonly IWorkspaceRoleRepository _repository;

    public WorkspaceRoleService(IWorkspaceRoleRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<WorkspaceRoleResponse>> GetWorkspaceRolesAsync()
    {
        var roles = await _repository.GetAllAsync();
        return roles.Select(r => new WorkspaceRoleResponse(r));
    }

    public async Task<WorkspaceRoleResponse> CreateWorkspaceRoleAsync(CreateWorkspaceRolesRequest request)
    {
        var role = new WorkspaceRole(request.Name);
        _repository.Add(role);
        await _repository.SaveAsync();
        return new WorkspaceRoleResponse(role);
    }
}
