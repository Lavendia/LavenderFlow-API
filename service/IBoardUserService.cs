public interface IBoardUserService
{
    Task<IEnumerable<BoardUserResponse>> GetUsersByBoardAsync(int boardId);
    Task<IEnumerable<BoardUserResponse>> GetBoardsByUserAsync(int userId);
    Task<BoardUserResponse> AddUserToBoardAsync(CreateBoardUserRequest request);
    Task<BoardUserResponse?> UpdateBoardUserAsync(int userId, int boardId, UpdateBoardUserRequest request);
    Task<bool> RemoveUserFromBoardAsync(int userId, int boardId);
}
