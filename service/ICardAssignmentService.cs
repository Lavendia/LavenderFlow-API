public interface ICardAssignmentService
{
    Task<IEnumerable<CardAssignmentResponse>?> GetAssignmentsByCardAsync(int cardId);
    Task<CardAssignmentResponse> CreateAssignmentAsync(CreateCardAssignmentRequest request);
    Task<bool> DeleteAssignmentAsync(int userId, int cardId);
}
