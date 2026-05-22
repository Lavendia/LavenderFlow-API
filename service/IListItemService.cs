public interface IListItemService
{
    Task<IEnumerable<ListItemResponse>> GetListItemsAsync();
    Task<ListItemResponse?> GetListItemAsync(int id);
    Task<ListItemResponse?> CreateListItemAsync(CreateListItemRequest request);
    Task<ListItemResponse?> UpdateListItemAsync(int id, UpdateListItemRequest request);
    Task<bool> DeleteListItemAsync(int id);
}
