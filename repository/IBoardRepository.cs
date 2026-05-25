public interface IBoardRepository
{
    Task<List<Board>> GetAllAsync();
    Task<Board?> GetByIdAsync(int id);
    void Add(Board board);
    void Delete(Board board);
    Task SaveAsync();
}
