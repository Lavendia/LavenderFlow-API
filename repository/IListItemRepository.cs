public interface IListItemRepository
{
    Task<List<ListItem>> GetAllAsync();
    Task<ListItem?> GetByIdAsync(int id);
    Task<List<ListItem>> GetByBoardIdAsync(int boardId);
    void Add(ListItem listItem);
    void Delete(ListItem listItem);
    Task SaveAsync();
}
