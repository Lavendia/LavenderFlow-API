public interface IListItemRepository
{
    Task<List<ListItem>> GetAllAsync();
    Task<ListItem?> GetByIdAsync(int id);
    void Add(ListItem listItem);
    void Delete(ListItem listItem);
    Task SaveAsync();
}
