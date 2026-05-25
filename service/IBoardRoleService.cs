public interface IBoardRoleService
{
    Task<IEnumerable<BoardRoleResponse>> GetBoardRolesAsync();
    Task<BoardRoleResponse> CreateBoardRoleAsync(CreateBoardRolesRequest request);
}
