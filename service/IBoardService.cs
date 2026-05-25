public interface IBoardService
{
    Task<IEnumerable<BoardResponse>> GetBoardsAsync();
    Task<BoardResponse?> GetBoardAsync(int id);
    Task<BoardResponse?> CreateBoardAsync(CreateBoardRequest request);
    Task<BoardResponse?> UpdateBoardAsync(int id, UpdateBoardRequest request);
    Task<bool> DeleteBoardAsync(int id);
}
