using Microsoft.AspNetCore.SignalR;

public class BoardService : IBoardService
{
    private readonly IBoardRepository _repository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IHubContext<LavenderFlowHub> _hub;

    public BoardService(
        IBoardRepository repository,
        IWorkspaceRepository workspaceRepository,
        IHubContext<LavenderFlowHub> hub)
    {
        _repository = repository;
        _workspaceRepository = workspaceRepository;
        _hub = hub;
    }

    public async Task<IEnumerable<BoardResponse>> GetBoardsAsync()
    {
        var boards = await _repository.GetAllAsync();
        return boards.Select(b => new BoardResponse(b));
    }

    public async Task<BoardResponse?> GetBoardAsync(int id)
    {
        var board = await _repository.GetByIdAsync(id);
        return board == null ? null : new BoardResponse(board);
    }

    public async Task<BoardResponse?> CreateBoardAsync(CreateBoardRequest request)
    {
        if (await _workspaceRepository.GetByIdAsync(request.WorkspaceId) is null)
            return null;

        var board = new Board(request.Name, request.Description, request.WorkspaceId);
        _repository.Add(board);
        await _repository.SaveAsync();
        var response = new BoardResponse(board);

        await _hub.Clients.Group(board.Id.ToString()).SendAsync("BoardCreated", response);

        return response;
    }

    public async Task<BoardResponse?> UpdateBoardAsync(int id, UpdateBoardRequest request)
    {
        var board = await _repository.GetByIdAsync(id);
        if (board == null)
            return null;

        if (request.Name is not null) board.Name = request.Name;
        if (request.Description is not null) board.Description = request.Description;

        await _repository.SaveAsync();
        var response = new BoardResponse(board);

        await _hub.Clients.Group(board.Id.ToString()).SendAsync("BoardUpdated", response);

        return response;
    }

    public async Task<bool> DeleteBoardAsync(int id)
    {
        var board = await _repository.GetByIdAsync(id);
        if (board == null)
            return false;

        _repository.Delete(board);
        await _repository.SaveAsync();

        await _hub.Clients.Group(id.ToString()).SendAsync("BoardDeleted", id);

        return true;
    }
}
