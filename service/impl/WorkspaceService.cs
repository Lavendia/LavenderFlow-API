public class WorkspaceService : IWorkspaceService
{
    private readonly IWorkspaceRepository _repository;
    private readonly IWorkspaceUserRepository _workspaceUserRepository;
    private readonly IWorkspaceRoleRepository _workspaceRoleRepository;

    public WorkspaceService(
        IWorkspaceRepository repository,
        IWorkspaceUserRepository workspaceUserRepository,
        IWorkspaceRoleRepository workspaceRoleRepository)
    {
        _repository = repository;
        _workspaceUserRepository = workspaceUserRepository;
        _workspaceRoleRepository = workspaceRoleRepository;
    }

    public async Task<IEnumerable<WorkspaceResponse>> GetWorkspacesForUserAsync(int userId)
    {
        var workspaces = await _repository.GetByUserIdAsync(userId);
        return workspaces.Select(w => new WorkspaceResponse(w));
    }

    public async Task<WorkspaceResponse?> GetWorkspaceForUserAsync(int id, int userId)
    {
        if (!await _workspaceUserRepository.UserBelongsToWorkspaceAsync(userId, id))
            return null;

        var workspace = await _repository.GetByIdAsync(id);
        return workspace == null ? null : new WorkspaceResponse(workspace);
    }

    public async Task<WorkspaceResponse> CreateWorkspaceAsync(CreateWorkspaceRequest request, int userId)
    {
        var workspace = new Workspace(request.Name, request.Description, request.IsPublic);
        _repository.Add(workspace);
        await _repository.SaveAsync();

        var adminRoleId = await GetAdminRoleIdAsync();
        _workspaceUserRepository.Add(new WorkspaceUser(userId, workspace.Id, adminRoleId));
        await _workspaceUserRepository.SaveAsync();

        return new WorkspaceResponse(workspace);
    }

    public async Task<bool> UpdateWorkspaceAsync(int id, UpdateWorkspaceRequest request, int userId)
    {
        if (!await _workspaceUserRepository.UserBelongsToWorkspaceAsync(userId, id))
            return false;

        var workspace = await _repository.GetByIdAsync(id);
        if (workspace == null)
            return false;

        if (request.Name is not null) workspace.Name = request.Name;
        if (request.Description is not null) workspace.Description = request.Description;
        if (request.IsPublic.HasValue) workspace.IsPublic = request.IsPublic.Value;

        await _repository.SaveAsync();
        return true;
    }

    public async Task<IEnumerable<BoardResponse>?> GetBoardsByWorkspaceForUserAsync(int id, int userId)
    {
        if (!await _workspaceUserRepository.UserBelongsToWorkspaceAsync(userId, id))
            return null;

        var boards = await _repository.GetBoardsByWorkspaceAsync(id);
        return boards == null ? null : boards.Select(b => new BoardResponse(b));
    }

    public async Task<bool> DeleteWorkspaceAsync(int id, int userId)
    {
        if (!await _workspaceUserRepository.UserBelongsToWorkspaceAsync(userId, id))
            return false;

        var workspace = await _repository.GetByIdAsync(id);
        if (workspace == null)
            return false;

        _repository.Delete(workspace);
        await _repository.SaveAsync();
        return true;
    }

    private async Task<int> GetAdminRoleIdAsync()
    {
        var roles = await _workspaceRoleRepository.GetAllAsync();
        var adminRole = roles.FirstOrDefault(r =>
            string.Equals(r.Name, "Admin", StringComparison.OrdinalIgnoreCase));

        return adminRole?.Id ?? roles.First().Id;
    }
}
