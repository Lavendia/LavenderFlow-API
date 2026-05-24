public class WorkspaceUserService : IWorkspaceUserService
{
    private readonly IWorkspaceUserRepository _repository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IUserRepository _userRepository;
    private readonly IWorkspaceRoleRepository _workspaceRoleRepository;

    public WorkspaceUserService(
        IWorkspaceUserRepository repository,
        IWorkspaceRepository workspaceRepository,
        IUserRepository userRepository,
        IWorkspaceRoleRepository workspaceRoleRepository)
    {
        _repository = repository;
        _workspaceRepository = workspaceRepository;
        _userRepository = userRepository;
        _workspaceRoleRepository = workspaceRoleRepository;
    }

    public async Task<IEnumerable<WorkspaceUserResponse>?> GetUsersByWorkspaceAsync(int workspaceId)
    {
        if (await _workspaceRepository.GetByIdAsync(workspaceId) is null)
            return null;

        var workspaceUsers = await _repository.GetByWorkspaceIdAsync(workspaceId);
        return workspaceUsers.Select(wu => new WorkspaceUserResponse(wu));
    }

    public async Task<WorkspaceUserResponse> CreateWorkspaceUserAsync(int workspaceId, CreateWorkspaceUsersRequest request)
    {
        if (await _workspaceRepository.GetByIdAsync(workspaceId) is null)
            throw new KeyNotFoundException("Workspace not found.");

        if (await _userRepository.GetByIdAsync(request.UserId) is null)
            throw new KeyNotFoundException("User not found.");

        if (await _workspaceRoleRepository.GetByIdAsync(request.RoleId) is null)
            throw new KeyNotFoundException("Role not found.");

        var workspaceUser = new WorkspaceUser(request.UserId, workspaceId, request.RoleId);
        _repository.Add(workspaceUser);
        await _repository.SaveAsync();
        return new WorkspaceUserResponse(workspaceUser);
    }

    public async Task<IEnumerable<WorkspaceUserResponse>> GetWorkspacesByUserAsync(int userId)
    {
        if (await _userRepository.GetByIdAsync(userId) is null)
            throw new KeyNotFoundException("User not found.");

        var workspaceUsers = await _repository.GetByUserIdAsync(userId);
        return workspaceUsers.Select(wu => new WorkspaceUserResponse(wu));
    }

    public async Task<bool> RemoveUserFromWorkspaceAsync(int workspaceUserId)
    {
        var workspaceUser = await _repository.GetByIdAsync(workspaceUserId);
        if (workspaceUser is null)
            throw new KeyNotFoundException("Workspace user not found.");

        _repository.Delete(workspaceUser);
        await _repository.SaveAsync();
        return true;
    }

    public async Task<WorkspaceUserResponse?> UpdateWorkspaceUserAsync(int workspaceUserId, UpdateWorkspaceUsersRequest request)
    {
        var workspaceUser = await _repository.GetByIdAsync(workspaceUserId);
        if (workspaceUser is null)
            throw new KeyNotFoundException("Workspace user not found.");

        if (await _workspaceRoleRepository.GetByIdAsync(request.RoleId) is null)
            throw new KeyNotFoundException("Role not found.");

        workspaceUser.RoleId = request.RoleId;
        await _repository.SaveAsync();
        return new WorkspaceUserResponse(workspaceUser);
    }

    public async Task<WorkspaceUserResponse?> GetWorkspaceUserAsync(int workspaceUserId)
    {
        var workspaceUser = await _repository.GetByIdAsync(workspaceUserId);
        if (workspaceUser is null)
            throw new KeyNotFoundException("Workspace user not found.");

        return new WorkspaceUserResponse(workspaceUser);
    }
}
