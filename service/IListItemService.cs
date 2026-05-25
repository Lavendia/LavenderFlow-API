public interface IListItemService
{
    Task<IEnumerable<ListItemResponse>> GetListItemsAsync();
    Task<ListItemResponse?> GetListItemAsync(int id);
    Task<IEnumerable<ListItemResponse>> GetListItemsByBoardAsync(int boardId);
    Task<ListItemResponse?> CreateListItemAsync(CreateListItemRequest request);
    Task<ListItemResponse?> UpdateListItemAsync(int id, UpdateListItemRequest request);
    Task<bool> DeleteListItemAsync(int id);
}
