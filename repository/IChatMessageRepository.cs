public interface IChatMessageRepository
{
    Task<List<ChatMessage>> GetAllAsync();
    Task<ChatMessage?> GetByIdAsync(int id);
    Task<List<ChatMessage>> GetByCardIdAsync(int cardId);
    void Add(ChatMessage message);
    void Delete(ChatMessage message);
    Task SaveAsync();
}
