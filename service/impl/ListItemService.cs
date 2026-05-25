using Microsoft.AspNetCore.SignalR;

public class ListItemService : IListItemService
{
    private readonly IListItemRepository _repository;
    private readonly IBoardRepository _boardRepository;
    private readonly IHubContext<LavenderFlowHub> _hub;

    public ListItemService(
        IListItemRepository repository,
        IBoardRepository boardRepository,
        IHubContext<LavenderFlowHub> hub)
    {
        _repository = repository;
        _boardRepository = boardRepository;
        _hub = hub;
    }

    public async Task<IEnumerable<ListItemResponse>> GetListItemsAsync()
    {
        var listItems = await _repository.GetAllAsync();
        return listItems.Select(li => new ListItemResponse(li));
    }

    public async Task<ListItemResponse?> GetListItemAsync(int id)
    {
        var listItem = await _repository.GetByIdAsync(id);
        return listItem == null ? null : new ListItemResponse(listItem);
    }

    public async Task<ListItemResponse?> CreateListItemAsync(CreateListItemRequest request)
    {
        if (await _boardRepository.GetByIdAsync(request.BoardId) is null)
            return null;

        var listItem = new ListItem(request.Name, request.Order, request.BoardId);
        _repository.Add(listItem);
        await _repository.SaveAsync();
        var response = new ListItemResponse(listItem);

        await _hub.Clients.Group(listItem!.BoardId.ToString()).SendAsync("ListCreated", response);

        return response;
    }

    public async Task<ListItemResponse?> UpdateListItemAsync(int id, UpdateListItemRequest request)
    {
        var listItem = await _repository.GetByIdAsync(id);
        if (listItem == null)
            return null;

        if (request.Name is not null) listItem.Name = request.Name;
        if (request.Order is not null) listItem.Order = request.Order.Value;

        await _repository.SaveAsync();
        var response = new ListItemResponse(listItem);

        await _hub.Clients.Group(listItem!.BoardId.ToString()).SendAsync("ListUpdated", response);
        return response;
    }

    public async Task<bool> DeleteListItemAsync(int id)
    {
        var listItem = await _repository.GetByIdAsync(id);
        if (listItem == null)
            return false;

        _repository.Delete(listItem);
        await _repository.SaveAsync();

        await _hub.Clients.Group(listItem!.BoardId.ToString()).SendAsync("ListDeleted", id);
        return true;
    }

    public async Task<IEnumerable<ListItemResponse>> GetListItemsByBoardAsync(int boardId)
    {
        var listItems = await _repository.GetByBoardIdAsync(boardId);
        return listItems.Select(li => new ListItemResponse(li));
    }
}
