public class WorkspaceService : IWorkspaceService
{
    private readonly IWorkspaceRepository _repository;

    public WorkspaceService(IWorkspaceRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<WorkspaceResponse>> GetWorkspacesAsync()
    {
        var workspaces = await _repository.GetAllAsync();
        return workspaces.Select(w => new WorkspaceResponse(w));
    }

    public async Task<WorkspaceResponse?> GetWorkspaceAsync(int id)
    {
        var workspace = await _repository.GetByIdAsync(id);
        return workspace == null ? null : new WorkspaceResponse(workspace);
    }

    public async Task<WorkspaceResponse> CreateWorkspaceAsync(CreateWorkspaceRequest request)
    {
        var workspace = new Workspace(request.Name, request.Description);
        _repository.Add(workspace);
        await _repository.SaveAsync();
        return new WorkspaceResponse(workspace);
    }

    public async Task<bool> UpdateWorkspaceAsync(int id, UpdateWorkspaceRequest request)
    {
        var workspace = await _repository.GetByIdAsync(id);
        if (workspace == null)
            return false;

        if (request.Name is not null) workspace.Name = request.Name;
        if (request.Description is not null) workspace.Description = request.Description;
        if (request.IsPublic.HasValue) workspace.IsPublic = request.IsPublic.Value;

        await _repository.SaveAsync();
        return true;
    }

    public async Task<IEnumerable<BoardResponse>?> GetBoardsByWorkspaceAsync(int id)
    {
        var boards = await _repository.GetBoardsByWorkspaceAsync(id);
        return boards == null ? null : boards.Select(b => new BoardResponse(b));
    }

    public async Task<bool> DeleteWorkspaceAsync(int id)
    {
        var workspace = await _repository.GetByIdAsync(id);
        if (workspace == null)
            return false;

        _repository.Delete(workspace);
        await _repository.SaveAsync();
        return true;
    }
}
