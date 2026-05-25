public interface IBoardRoleRepository
{
    Task<List<BoardRole>> GetAllAsync();
    Task<BoardRole?> GetByIdAsync(int id);
    void Add(BoardRole role);
    Task SaveAsync();
}
