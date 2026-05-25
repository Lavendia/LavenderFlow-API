using Microsoft.AspNetCore.SignalR;

public class BoardUserService : IBoardUserService
{
    private readonly IBoardUserRepository _repository;
    private readonly IBoardRepository _boardRepository;
    private readonly IUserRepository _userRepository;
    private readonly IBoardRoleRepository _boardRoleRepository;
    private readonly IHubContext<LavenderFlowHub> _hub;

    public BoardUserService(
        IBoardUserRepository repository,
        IBoardRepository boardRepository,
        IUserRepository userRepository,
        IBoardRoleRepository boardRoleRepository,
        IHubContext<LavenderFlowHub> hub)
    {
        _repository = repository;
        _boardRepository = boardRepository;
        _userRepository = userRepository;
        _boardRoleRepository = boardRoleRepository;
        _hub = hub;
    }

    public async Task<IEnumerable<BoardUserResponse>> GetUsersByBoardAsync(int boardId)
    {
        if (await _boardRepository.GetByIdAsync(boardId) is null)
            throw new KeyNotFoundException("Board does not exist with id " + boardId);

        var boardUsers = await _repository.GetByBoardIdAsync(boardId);
        return boardUsers.Select(bu => new BoardUserResponse(bu));
    }

    public async Task<IEnumerable<BoardUserResponse>> GetBoardsByUserAsync(int userId)
    {
        if (await _userRepository.GetByIdAsync(userId) is null)
            throw new KeyNotFoundException("User does not exist with id " + userId);

        var boardUsers = await _repository.GetByUserIdAsync(userId);
        return boardUsers.Select(bu => new BoardUserResponse(bu));
    }

    public async Task<BoardUserResponse> AddUserToBoardAsync(CreateBoardUserRequest request)
    {
        if (await _boardRepository.GetByIdAsync(request.BoardId) is null)
            throw new KeyNotFoundException("Board does not exist with id " + request.BoardId);

        if (await _userRepository.GetByIdAsync(request.UserId) is null)
            throw new KeyNotFoundException("User does not exist with id " + request.UserId);

        if (await _boardRoleRepository.GetByIdAsync(request.BoardRoleId) is null)
            throw new KeyNotFoundException("BoardRole does not exist with id " + request.BoardRoleId);

        if (await _repository.ExistsAsync(request.UserId, request.BoardId))
            throw new InvalidOperationException("User is already a member of this board.");

        var boardUser = new BoardUser(request.UserId, request.BoardId, request.BoardRoleId);
        _repository.Add(boardUser);
        await _repository.SaveAsync();
        var response = new BoardUserResponse(boardUser);

        await _hub.Clients.Group(request.BoardId.ToString()).SendAsync("UserAddedToBoard", response);

        return response;
    }

    public async Task<BoardUserResponse?> UpdateBoardUserAsync(int userId, int boardId, UpdateBoardUserRequest request)
    {
        var boardUser = await _repository.GetByIdsAsync(userId, boardId);
        if (boardUser == null)
            return null;

        if (await _boardRoleRepository.GetByIdAsync(request.BoardRoleId) is null)
            throw new KeyNotFoundException("BoardRole does not exist with id " + request.BoardRoleId);

        boardUser.BoardRoleId = request.BoardRoleId;
        await _repository.SaveAsync();
        var response = new BoardUserResponse(boardUser);

        await _hub.Clients.Group(boardId.ToString()).SendAsync("BoardUserUpdated", response);

        return response;
    }

    public async Task<bool> RemoveUserFromBoardAsync(int userId, int boardId)
    {
        var boardUser = await _repository.GetByIdsAsync(userId, boardId);
        if (boardUser == null)
            return false;

        _repository.Delete(boardUser);
        await _repository.SaveAsync();

        await _hub.Clients.Group(boardId.ToString()).SendAsync("UserRemovedFromBoard", userId);

        return true;
    }
}
