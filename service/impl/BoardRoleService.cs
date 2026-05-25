public class BoardRoleService : IBoardRoleService
{
    private readonly IBoardRoleRepository _repository;

    public BoardRoleService(IBoardRoleRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<BoardRoleResponse>> GetBoardRolesAsync()
    {
        var roles = await _repository.GetAllAsync();
        return roles.Select(r => new BoardRoleResponse(r));
    }

    public async Task<BoardRoleResponse> CreateBoardRoleAsync(CreateBoardRolesRequest request)
    {
        var role = new BoardRole(request.Name);
        _repository.Add(role);
        await _repository.SaveAsync();
        return new BoardRoleResponse(role);
    }
}
