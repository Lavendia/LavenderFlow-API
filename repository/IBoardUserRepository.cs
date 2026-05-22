public interface IBoardUserRepository
{
    Task<List<BoardUser>> GetByBoardIdAsync(int boardId);
    Task<List<BoardUser>> GetByUserIdAsync(int userId);
    Task<BoardUser?> GetByIdsAsync(int userId, int boardId);
    Task<bool> ExistsAsync(int userId, int boardId);
    void Add(BoardUser boardUser);
    void Delete(BoardUser boardUser);
    Task SaveAsync();
}
